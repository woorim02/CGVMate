namespace CgvMate.Core;

/// <summary>
/// 비동기 래핑된 작업에서 발생한 예외 데이터를 제공합니다.
/// </summary>
public class ExceptionOccurredEventArgs : EventArgs
{
    /// <summary>
    /// 예외가 발생한 Task
    /// </summary>
    public Task Task { get; private set; }

    /// <summary>
    /// 처리되지 않은 예외
    /// </summary>
    public Exception Exception { get; private set; }

    public ExceptionOccurredEventArgs(Task task, Exception exception)
    {
        Task = task;
        Exception = exception;
    }
}
