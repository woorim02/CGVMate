namespace CgvMate.Data;

public class SurpriseCupon
{
    public string Index { get; set; }
    public string Title { get; set; }
    public int Count { get; set; }
    public bool IsAvailable { get; set; }

    public SurpriseCupon() { }

    public SurpriseCupon(string index, string title, int count, bool isAvailable)
    {
        Index = index;
        Title = title;
        Count = count;
        IsAvailable = isAvailable;
    }
}
