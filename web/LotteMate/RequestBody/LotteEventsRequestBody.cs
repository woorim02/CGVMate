namespace LotteMate;

public partial class LotteService
{
    public class LotteEventsRequestBody : RequestBodyBase
    {
        public string EventClassificationCode;
        public string SearchText = "";
        public string CinemaID = "";
        public string MemberNo = "0";
        public int PageNo;
        public int PageSize;

        public LotteEventsRequestBody(LotteEventType type ,int pageNo = 1, int pageSize = 30, string searchText = "")
        {
            MethodName = "GetEventLists";
            EventClassificationCode = $"{(int)type}";
            PageNo = pageNo;
            PageSize = pageSize;
            SearchText = searchText;
        }
    }
}
