using System.Threading.Tasks;

namespace CgvMate.Core;

public class TaskProcessor
{
    /// <summary>
    /// 작업 대기열에서 작업 실행 중 처리할 수 없는 오류가 발생했을때 발생하는 이벤트입니다.
    /// </summary>
    public event EventHandler<ExceptionOccurredEventArgs>? ExceptionOccurred;

    /// <summary>
    /// 작업 대기열
    /// </summary>
    private readonly List<Func<Task>> _funcList;

    /// <summary>
    /// Do Not Use It. IsRunning 사용. Start(CancellationToken token)을 위한 필드. 
    /// </summary>
    private CancellationToken? _token;

    /// <summary>
    /// 작업이 진행중인지 나타냅니다.
    /// </summary>
    public bool IsRunning { get => !(_token?.IsCancellationRequested ?? true); }

    /// <summary>
    /// 작업 대기열 용량.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// 최대 동시 작업 수 //TODO
    /// </summary>
    public int ConcurrencyLimit { get; set; }

    /// <summary>
    /// 작업 주기. 동시 작업 수가 n(n>=2)인 경우 주기마다 n개의 작업을 실행합니다.
    /// </summary>
    public TimeSpan Interval { get; set; }

    /// <summary>
    /// 작업 대기열 용량, 최대 동시 작업 수, 작업 주기를 사용하여 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="capacity">작업 대기열 용량</param>
    /// <param name="concurrencyLimit">최대 동시 작업 수</param>
    /// <param name="interval">작업 주기</param>
    public TaskProcessor(int capacity, int concurrencyLimit, TimeSpan interval)
    {
        Capacity = capacity;
        ConcurrencyLimit = concurrencyLimit;
        Interval = interval;
        _funcList = new List<Func<Task>>(Capacity);
    }

    /// <summary>
    /// 작업 대기열을 실행합니다.
    /// </summary>
    public void Start() => Start(new CancellationToken());

    /// <summary>
    /// CancellationToken을 사용하여 작업 대기열을 실행합니다.
    /// </summary>
    public void Start(CancellationToken token)
    {
        if (IsRunning)
            return;

        _token = token;
        Task.Run(async () =>
        {
            while (IsRunning)
            {
                Func<Task>? func;
                lock (_funcList)
                {
                    if (_funcList.Count == 0)
                    {
                        func = null;
                    }
                    else
                    {
                        func = (Func<Task>?)_funcList[0];
                        _funcList.RemoveAt(0); 
                    }
                }
                if (func == null)
                {
                    await Task.Delay(Interval);
                    continue;
                }
                var task = func();
                try
                {
                    await task;
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
    /// 작업 대기열을 정지합니다. 현재 실행중인 작업은 취소되지 않습니다.
    /// </summary>
    public void Stop()
    {
        if (!IsRunning)
            return;
        _token = null;
    }

    /// <summary>
    /// 작업 대기열의 대기중인 모든 작업을 제거합니다. 현재 실행중인 작업은 취소되지 않습니다.
    /// </summary>
    public void Clear() => _funcList.Clear();

    /// <summary>
    /// 작업을 대기열에 추가하려고 시도합니다.
    /// </summary>
    /// <returns>작업 추가가 성공하면 True, 실패하면 False</returns>
    public bool TryEnqueue(Func<Task> func)
    {
        if (_funcList.Count > Capacity)
        {
            return false;
        }
        _funcList.Add(func);
        return true;
    }

    /// <summary>
    /// 작업을 대기열에 추가하려고 시도합니다. CancellationToken으로 작업 취소가 가능합니다.
    /// </summary>
    /// <returns>작업 추가가 성공하면 True, 실패하면 False</returns>
    public bool TryEnqueue(Func<Task> func, CancellationToken token)
    {
        token.Register(() => _funcList.Remove(func));
        return TryEnqueue(func);
    }
}