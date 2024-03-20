namespace CgvMate.Service;

public class OpenNotificationService
{
    private readonly CgvService service;
    private readonly List<OpenNotificationInfo> infos = new List<OpenNotificationInfo>();
    private TaskProcessor executor;
    private TimeSpan interval;

    /// <summary>
    /// 서비스 실행 중 처리할 수 없는 예외가 발생했을때 발생하는 이벤트입니다.
    /// </summary>
    public event EventHandler<ExceptionOccurredEventArgs>? ExceptionOccurred;

    /// <summary>
    /// 서비스가 실행중인지 여부를 나타냅니다.
    /// </summary>
    public bool IsRunning = false;

    public OpenNotificationService(CgvService service, TimeSpan interval)
    {
        this.service = service;
        this.interval = interval;
        executor = new TaskProcessor(30, 1, interval);
        executor.ExceptionOccurred += Executor_ExceptionOccurred;
    }

    #region Public Methods
    /// <summary>
    /// 오픈알림 서비스를 시작합니다.
    /// </summary>
    /// <param name="preCallback">예매준비중 상태로 바뀌었을때 발생하는 콜백</param>
    /// <param name="openCallback">예매 오픈되었을 때 발생하는 콜백</param>
    /// <param name="token">취소 토큰</param>
    public void Start(Action<OpenNotificationInfo> preCallback, Action<OpenNotificationInfo> openCallback, CancellationToken token)
    {
        if (IsRunning)
            return;

        IsRunning = true;
        Task.Run(async () =>
        {
            while (IsRunning)
            {
                for (int i = 0; i < infos.Count; i++)
                {
                    var result = executor.TryEnqueue(async () => await CheckOpen(infos[i], preCallback, openCallback));
                    if (result is false)
                    {
                        i--;
                        await Task.Delay(interval);
                        continue;
                    }
                }
            }
        });
        executor.Start();
        token.Register(Stop);
    }
    // 외부에서 호출과 동시에 token에 register된 stop이 실행될 경우 방지.
    private object lockObject = new object();
    public void Stop()
    {
        lock (lockObject)
        {
            if (!IsRunning)
                return;
            IsRunning = false;
        }
        executor.Stop();
        executor.Clear();
    }

    /// <summary>
    /// EqualityComparer을 이용하여 아이템이 존재하는지 확인하고, 존재하지 않는다면 추가합니다.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>아이템이 추가되었는지 여부를 나타냅니다.</returns>
    public bool AddNotificationInfo(OpenNotificationInfo item)
    {
        var contains = infos.Contains(item, EqualityComparer<OpenNotificationInfo>.Create((x, y) =>
            x.MovieIndex == y.MovieIndex
            && x.TheaterCode != y.TheaterCode
            && x.TargetDate == y.TargetDate));

        if (contains)
            return false;

        infos.Add(item);
        return true;
    }

    /// <summary>
    /// EqualityComparer을 이용하여 아이템이 존재하는지 확인하고, 존재한다면 삭제합니다.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>아이템이 삭제되었는지 여부를 나타냅니다.</returns>
    public bool RemoveNotificationInfo(OpenNotificationInfo item)
    {
        var i = infos.RemoveAll(x =>
        x.MovieIndex == item.MovieIndex
        && x.TheaterCode != item.TheaterCode
        && x.TargetDate == item.TargetDate);
        return i > 0;
    }
    #endregion

    #region Private Methods
    private async Task CheckOpen(OpenNotificationInfo info, Action<OpenNotificationInfo> preCallback, Action<OpenNotificationInfo> openCallback)
    {
        // 오픈되었고 예매가 가능하다면 건너뛰기
        if (info.IsPreOpen && info.IsOpen)
            return;

        var theaterCode = info.TheaterCode.PadLeft(4, '0');
        var movieGroupcd = info.Movie!.MovieGroupCd;
        var targetDate = info.TargetDate;
        var movieTypeCode = GetMovieTypeCode(info.ScreenType);
        var list = await service.Reservation.GetScheduleListAsync(theaterCode, movieGroupcd, targetDate, movieTypeCode);

        // 목표 스케쥴 추출
        var targetScheduleList = list.ResultSchedule.ScheduleList
            .Where(x => info.Movie == null ? true : x.MovieIdx == info.MovieIndex)
            .ToList();

        // 목표 스케쥴 수가 0이면 리턴
        if (targetScheduleList.Count == 0)
            return;

        // 예매준비중 콜백
        if (!info.IsPreOpen)
        {
            preCallback(info);
            info.IsPreOpen = true;
        }
        // 예매 가능하다면 오픈 콜백
        if (!info.IsOpen)
        {
            openCallback(info);
            info.IsOpen = true;
            return;
        }
    }

    private void Executor_ExceptionOccurred(object? sender, ExceptionOccurredEventArgs e) => ExceptionOccurred?.Invoke(sender, e);
    #endregion

    #region Static Methods
    /// <param name="movieType">(2D, IMAX, 4DX....등등)</param>
    /// <returns>movieTypeCd</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static string GetMovieTypeCode(string movieType)
    {
        switch (movieType.ToUpper())
        {
            case "ALL": return "00";
            case "2D": return "01";
            case "4DX": return "03";
            case "IMAX": return "04";
            case "SCREENX": return "05";
            case "ULTRA 4DX": return "09";
            default:
                throw new NotImplementedException();
        }
    }
    #endregion
}