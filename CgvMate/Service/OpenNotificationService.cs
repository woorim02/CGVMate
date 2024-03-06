using System.Diagnostics;

namespace CgvMate.Service;

public class OpenNotificationService
{
    private readonly CgvService service;
    private readonly List<OpenNotificationInfo> infos;
    private readonly Queue<Task<Task>> taskQueue = new Queue<Task<Task>>(30);
    private readonly TimeSpan timeSpan = TimeSpan.FromSeconds(3);

    public bool IsRunning = false;

    public OpenNotificationService(CgvService service, List<OpenNotificationInfo> infos)
    {
        this.service = service;
        this.infos = infos;
    }

    #region Public Methods
    public void Start(Action<OpenNotificationInfo> preCallback, Action<OpenNotificationInfo> openCallback, CancellationToken token)
    {
        if (IsRunning)
            return;

        IsRunning = true;
        infos.Clear();
        taskQueue.Clear();
        Task.Run(async () => await RunCheckOpenTaskGenerator(preCallback, openCallback));
        Task.Run(RunTaskQueue);

        token.Register(Stop);
    }

    public void Stop()
    {
        IsRunning = false;
        infos.Clear();
        taskQueue.Clear();
    }
    #endregion

    #region Private Methods
    private async Task RunCheckOpenTaskGenerator(Action<OpenNotificationInfo> preCallback, Action<OpenNotificationInfo> openCallback)
    {
        while (IsRunning)
        {
            if(taskQueue.Count >= 30)
            {
                await Task.Delay(timeSpan);
                continue;
            }
            for(int i = 0; i < infos.Count; i++)
            {
                if (taskQueue.Count >= 30)
                {
                    await Task.Delay(timeSpan);
                    i--;
                    continue;
                }
                var asyncTask = new Task<Task>(async () =>
                {
                    await CheckOpen(infos[i], preCallback, openCallback);
                });
                taskQueue.Enqueue(asyncTask);
                await Task.Delay(timeSpan);
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

    private async Task RunTaskQueue()
    {
        while (IsRunning)
        {
            Task<Task>? task;
            var isDequeued = taskQueue.TryDequeue(out task);
            if (!isDequeued)
            {
                await Task.Delay(timeSpan);
                continue;
            }
            task!.Start();
            await task.Result;
            await Task.Delay(timeSpan);
        }
    }
    #endregion

    #region Static Methods
    /// <param name="movieType">��ũ��Ÿ��(2D, IMAX, 4DX....��)</param>
    /// <returns>strScreenTypeCd</returns>
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