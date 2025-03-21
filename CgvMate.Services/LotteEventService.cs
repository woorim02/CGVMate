﻿using CgvMate.Data.Entities.LotteCinema;
using CgvMate.Data.Enums;
using CgvMate.Services.DTOs.LotteCinema;
using CgvMate.Services.Repos;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using static System.Net.WebRequestMethods;

namespace CgvMate.Services;

public class LotteService
{
    private readonly HttpClient _client;
    private readonly ILotteGiveawayEventRepository _giveawayEventRepository;
    private readonly ILotteGiveawayEventKeywordRepository _giveawayEventKeywordRepository;
    private readonly ILotteGiveawayEventModelRepository _giveawayEventModelRepository;

    public LotteService(HttpClient client,
                        ILotteGiveawayEventRepository giveawayEventRepository,
                        ILotteGiveawayEventKeywordRepository giveawayEventKeywordRepository,
                        ILotteGiveawayEventModelRepository giveawayEventModelRepository)
    {
        _client = client;
        _giveawayEventRepository = giveawayEventRepository;
        _giveawayEventKeywordRepository = giveawayEventKeywordRepository;
        _giveawayEventModelRepository = giveawayEventModelRepository;
    }

    public async Task<List<Event>> GetEventsAsync(LotteEventType type)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        var body = new EventsReqDTO(type);
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var root = JsonConvert.DeserializeObject<EventsResDTO>(json);

        return root.Items;
    }

    public async Task<List<CuponEvent>> GetCuponEventsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        request.Headers.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 16_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.6 Mobile/15E148 Safari/604.1");
        var body = new CuponEventReqDto()
        {
            MethodName = "GetSpeedEventDetailMulti",
            EventID = "",
            MainEventID = "201210016922014",
        };
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var res = JsonConvert.DeserializeObject<CuponEventResDto>(json);
        var cupons = new List<CuponEvent>();
        foreach (var detail in res.SpeedEventDetail[0].ItemGroup)
        {
            var item = detail.Items[0];
            var cupon = new CuponEvent()
            {
                Title = item.MovieNm,
                ImageUrl = item.Img5Url,
                StartDateTime = DateTime.ParseExact($"{item.ProgressStartDate} {item.ProgressStartTime}", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
            };
            cupons.Add(cupon);
        }
        return cupons;
    }

    public async Task<List<Event>> GetGiveawayEventsAsync()
    {
        // 영화 이벤트 목록 가져오기
        var events = await GetEventsAsync(LotteEventType.영화);
        // 필터링할 키워드 가져오기
        var keywords = await _giveawayEventKeywordRepository.GetAllKeywordsAsync();
        // 필터링된 이벤트 리스트 생성
        var keywordSet = new HashSet<string>(keywords);
        IReadOnlyList<Event> list = events
            .Where(e => keywordSet.Any(k => e.EventName.Contains(k)))
            .ToList();
        // 데이터베이스에 저장된 모델 목록 불러오기
        var eventModels = (await _giveawayEventModelRepository.GetAllAsync()).ToDictionary( x => x.EventID, x => x);
        var finalList = new List<Event>();
        foreach(var @event in list) 
        {
            finalList.Add(@event);
            // 아직 모델이 조회된 적 없으면 건너뛰기
            if (!eventModels.ContainsKey(@event.EventID))
                continue;
            // 다음 모델이 없으면 건너뛰기
            var model = eventModels[@event.EventID];
            if (!model.HasNext)
                continue;
            // list(서버에서 가져온 값)에 다음 이벤트 아이디가 존재하면 건너뛰기
            var nextID = (long.Parse(@event.EventID) + 1).ToString();
            var count = list.Where(e => e.EventID == nextID).Count();
            if (count > 0)
                continue;
            // 다음 모델 불러오기
            var nextModel = await GetLotteGiveawayEventModelAsync(nextID);
            // 다음 이벤트를 final리스트에 추가
            Event nextEvent = DeepCopy(@event);
            nextEvent.EventID = nextID;
            nextEvent.EventName = nextModel.FrGiftNm;
            finalList.Add(nextEvent);
        }

        var kvp = new Dictionary<string, Event>();
        foreach (var item in finalList)
        {
            kvp.Add(item.EventID, item);
        }
        var views = await _giveawayEventRepository.GetViewsAsync(kvp.Select(x => x.Value.EventID));
        foreach (var view in views)
        {
            kvp[view.Key].Views = view.Value;
        }
        var updateList = new List<Event>();
        foreach (var item in kvp)
        {
            if (!views.Keys.Contains(item.Key))
            {
                updateList.Add(item.Value);
            }
        }
        await _giveawayEventRepository.AddAsync(updateList);

        return finalList;
    }

    public async Task<LotteGiveawayEventModel?> GetLotteGiveawayEventModelAsync(string eventID, bool updateView = false)
    {
        // 데이터베이스에서 모델 정보 가져오기
        var model = await _giveawayEventModelRepository.GetAsync(eventID);
        if (model != null)
        {
            // 조회수 올리기
            if (updateView)
                await _giveawayEventRepository.UpdateViewAsync(eventID);
            return model;// 존재하면 리턴
        }

        //존재하지 않으면 서버에서 불러오기
        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        var body = new InfomationDeliveryEventDetailReqDTO(eventID);
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var root = JsonConvert.DeserializeObject<InfomationDeliveryEventDetailResDTO>(json);
        if (root.IsOK != "true")
            return null;
        if (root.InfomationDeliveryEventDetail.Count == 0)
            return null;
        if (root.InfomationDeliveryEventDetail[0].GoodsGiftItems.Count == 0)
            return null;
        model = root.InfomationDeliveryEventDetail[0].GoodsGiftItems[0];
        // 조회수 올리기
        if (updateView)
            await _giveawayEventRepository.UpdateViewAsync(eventID);
        // 다음 이벤트 존재하는지 확인
        var nextModel = await GetLotteGiveawayEventModelAsync((long.Parse(eventID) + 1).ToString());
        model.HasNext = nextModel is not null;
        // 모델 저장
        await _giveawayEventModelRepository.AddAsync(model);
        return model;
    }

    public async Task<GiveawayEventDetail> GetLotteGiveawayInfoAsync(string eventID, string giftID)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        var body = new CinemaGoodsReqDTO(eventID, giftID);
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var root = JsonConvert.DeserializeObject<CinemaGoodsResDTO>(json);
        var detail = new GiveawayEventDetail()
        {
            CinemaDivisionGoods = root.CinemaDivisionGoods,
            CinemaDivisions = root.CinemaDivisions,
            EventID = eventID,
            GiftID = giftID
        };
        return detail;
    }

    private async Task<string> SendForm(HttpRequestMessage request, string body)
    {
        var form = new MultipartFormDataContent("WebKitFormBoundaryfFeN6uLtzIIBZZ2x");
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        form.Add(content, "paramList");
        request.Content = form;
        request.Headers.Add("Referer", "https://www.lottecinema.co.kr/NLCHS/Event/DetailList?code=20");
        var response = await _client.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }

    public static T DeepCopy<T>(T obj)
    {
        string serialized = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}