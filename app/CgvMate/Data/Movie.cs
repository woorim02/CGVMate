namespace CgvMate.Data;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
public class Movie
{
    public string Index { get; set; }
    public string Title { get; set;}
    public string ThumbnailSource { get; set;}
    /// <summary>
    /// 영화코드 - 모코드
    /// </summary>
    /// <remarks>
    /// null 체크 필수.
    /// </remarks>
    public string? MovieGroupCd { get; set;}

    /// <summary>
    /// 스크린타입(2D, IMAX, 4DX....등)
    /// </summary>
    /// <remarks>
    /// <c>null</c>
    /// </remarks>
    public string[]? ScreenTypes { get; set;}
}
