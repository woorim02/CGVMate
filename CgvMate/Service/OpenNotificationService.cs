namespace CgvMate.Service;

public class OpenNotificationService
{
    private readonly CgvService service;
    private readonly List<OpenNotificationInfo> infos;
    private TaskProcessor executor;
    private readonly TimeSpan interval = TimeSpan.FromSeconds(3);

    /// <summary>
    /// 서비스 실행 중 처리할 수 없는 예외가 발생했을때 발생하는 이벤트입니다.
    /// </summary>
    public event EventHandler<ExceptionOccurredEventArgs>? ExceptionOccurred;
    public bool IsRunning = false;

    public OpenNotificationService(CgvService service, List<OpenNotificationInfo> infos)
    {
        this.service = service;
        this.infos = infos;
        executor = new TaskProcessor(30, 1, TimeSpan.FromSeconds(3));
    }

    #region Public Methods
    public void Start(Action<OpenNotificationInfo> preCallback, Action<OpenNotificationInfo> openCallback, CancellationToken token)
    {
        if (IsRunning)
            return;

        IsRunning = true;
        Task.Run(async () => await RunService(preCallback, openCallback));
        executor.Start(token);
        token.Register(Stop);
    }

    public void Stop()
    {
        IsRunning = false;
        executor.Stop();
        executor.Clear();
    }
    #endregion

    #region Private Methods
    private async Task RunService(Action<OpenNotificationInfo> preCallback, Action<OpenNotificationInfo> openCallback)
    {
        executor.ExceptionOccurred += Executor_ExceptionOccurred;
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
    }

    private async Task CheckOpen(OpenNotificationInfo info, Action<OpenNotificationInfo> preCallback, Action<OpenNotificationInfo> openCallback)
    {
        if (info.IsOpen && info.CanReservation)
            return;
        var theaterCode = info.TheaterCode.PadLeft(4, '0');
        var movieGroupcd = info.Movie!.MovieGroupCd;
        var targetDate = info.TargetDate;
        var movieTypeCode = GetMovieTypeCode(info.ScreenType);
        var list = await service.Reservation.GetScheduleListAsync(theaterCode, movieGroupcd, targetDate, movieTypeCode);

        var targetScheduleList = list.ResultSchedule.ScheduleList
            .Where(x => info.Movie == null ? true : x.MovieIdx == info.MovieIndex)
            .ToList();

        if (targetScheduleList.Count == 0)
            return;

        if (!info.IsOpen)
        {
            preCallback(info);
            info.IsOpen = true;
            return;
        }
        if (!info.CanReservation)
        {
            openCallback(info);
            info.CanReservation = true;
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