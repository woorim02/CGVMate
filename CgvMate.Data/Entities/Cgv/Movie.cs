namespace CgvMate.Data.Entities.Cgv;

public class Movie
{
    public string Index { get; set; }
    public string Title { get; set; }
    public string ThumbnailSource { get; set; }
    /// <summary>
    /// 영화코드 - 모코드
    /// </summary>
    /// <remarks>
    /// null 체크 필수.
    /// </remarks>
    public string? MovieGroupCd { get; set; }

    /// <summary>
    /// 스크린타입(2D, IMAX, 4DX....등)
    /// </summary>
    /// <remarks>
    /// <c>null</c>
    /// </remarks>
    public string[]? ScreenTypes { get; set; }
}