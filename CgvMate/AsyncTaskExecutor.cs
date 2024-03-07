namespace CgvMate;

public class AsyncTaskExecutor
{
    public event EventHandler<ExceptionOccurredEventArgs>? ExceptionOccurred;

    private readonly Queue<Task<Task>> _queue = new Queue<Task<Task>>();
    /// <summary>
    /// Do Not Use It. IsRunning 사용. Run(CancellationToken token)을 위한 필드. 
    /// </summary>
    private CancellationToken? _token;

    /// <summary>
    /// 작업이 진행중인지 나타냅니다.
    /// </summary>
    public bool IsRunning { get => _token?.IsCancellationRequested ?? false; }

    /// <summary>
    /// 작업 큐 용량. //TODO
    /// </summary>
    public int Capacity { get; set; } = 30;

    /// <summary>
    /// 최대 동시 작업 수
    /// </summary>
    public int ConcurrencyLimit { get; set; } = 1;

    /// <summary>
    /// 작업 주기. 동시 작업 수가 n(n>=2)인 경우 주기마다 n개의 작업을 실행합니다.
    /// </summary>
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// 기본값(30, 1, 1s)을 사용하여 클래스의 새 인스턴스를 초기화합니다. 
    /// </summary>
    public AsyncTaskExecutor() { }

    /// <summary>
    /// 작업 큐 용량, 최대 동시 작업 수, 작업 주기를 사용자화 하여 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="capacity">작업 큐 용량</param>
    /// <param name="concurrencyLimit">최대 동시 작업 수</param>
    /// <param name="interval">작업 주기</param>
    public AsyncTaskExecutor(int capacity, int concurrencyLimit, TimeSpan interval)
    {
        Capacity = capacity;
        ConcurrencyLimit = concurrencyLimit;
        Interval = interval;
    }

    /// <summary>
    /// 작업 큐를 실행합니다.
    /// </summary>
    public void Run() => Run(new CancellationToken());

    /// <summary>
    /// CancellationToken을 사용하여 작업 큐를 실행합니다.
    /// </summary>
    public void Run(CancellationToken token)
    {
        _token = token;
        Task.Run(async () =>
        {
            while (IsRunning)
            {
                // TODO 작업 동시진행 구현
                Task<Task>? task = null;
                var isDequeued = _queue.TryDequeue(out task);
                if (!isDequeued)
                {
                    await Task.Delay(Interval);
                    continue;
                }

                if (task!.IsCanceled)
                {
                    continue;
                }

                task.Start();
                try
                {
                    await task.Result;
                }
                catch (Exception ex)
                {
                    ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(task, ex));
                }
                await Task.Delay(Interval);
            }
        });

    }

    /// <summary>
    /// 작업 큐를 정지합니다.
    /// </summary>
    public void Stop()
    {
        if (!IsRunning)
            return;
        _token = null;
    }

    /// <summary>
    /// 작업 큐의 대기중인 모든 작업을 제거합니다. 현재 실행중인 작업은 취소되지 않습니다.
    /// </summary>
    public void Clear() => _queue.Clear();

    /// <summary>
    /// 작업 큐에 작업을 추가합니다.
    /// </summary>
    public void EnQueue(Func<Task> func) => EnQueue(new Task<Task>(func));

    /// <summary>
    /// 작업 큐에 작업을 추가합니다. CancellationToken으로 작업 취소가 가능합니다.
    /// </summary>
    public void EnQueue(Func<Task> func, CancellationToken token) => EnQueue(new Task<Task>(func, token));

    private void EnQueue(Task<Task> task) => _queue.Enqueue(task);
}
/// <summary>
/// AsyncTaskExecutor에서 발생한 예외 데이터를 제공합니다.
/// </summary>
public class ExceptionOccurredEventArgs : EventArgs
{
    /// <summary>
    /// 예외가 발생한 Task
    /// </summary>
    public Task<Task> Task { get; private set; }

    /// <summary>
    /// AsyncTaskExecutor에서 처리되지 않은 예외
    /// </summary>
    public Exception Exception { get; private set; }

    public ExceptionOccurredEventArgs(Task<Task> task, Exception exception)
    {
        Task = task;
        Exception = exception;
    }
}
