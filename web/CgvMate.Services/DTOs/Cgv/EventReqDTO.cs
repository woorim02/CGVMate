namespace CgvMate.Services.DTOs.Cgv;

internal class EventReqDTO
{
    public EventReqDTO(CgvEventType type)
    {
        mC = ((int)type).ToString("D3");
    }
    public string mC;
    public string rC = "GEN";
    public string tC = "";
    public string iP = "1";
    public string pRow = "100";
    public string rnd6 = "0";
    public string fList = "";
}
