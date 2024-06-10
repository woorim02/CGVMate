namespace LotteMate;
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class LotteGiveawayEventModel
    {
        public string EventID { get; set; }
        public string FrGiftID { get; set; }
        public string FrGiftNm { get; set; }
    }

    public class InfomationDeliveryEventDetail
    {
        public List<LotteGiveawayEventModel> GoodsGiftItems { get; set; }
    }

    public class InfomationDeliveryEventDetailResponse
    {
        public List<InfomationDeliveryEventDetail> InfomationDeliveryEventDetail { get; set; }
        public string IsOK { get; set; }
        public string ResultMessage { get; set; }
        public object ResultCode { get; set; }
        public object EventResultYn { get; set; }
    }