class LotteMateApi {
  async getLotteEventListAsync(type) {
    const response = await fetch(`https://api.cgvmate.com/lotte/event/list?type=${type}`);
    const data = await response.json();
    return data;
  }

  async getLotteGiveawayEventListAsync() {
    const response = await fetch(`https://api.cgvmate.com/lotte/event/giveaway/list`);
    const data = await response.json();
    return data;
  }

  async getLotteGiveawayEventModelAsync(eventID) {
    const response = await fetch(`https://api.cgvmate.com/lotte/event/giveaway/model?event_id=${eventID}`);
    const data = await response.json();
    return data;
  }

  async getLotteGiveawayInfoAsync(event_id, gift_id) {
    const response = await fetch(`https://api.cgvmate.com/lotte/event/giveaway/info?event_id=${event_id}&gift_id=${gift_id}`);
    const data = await response.json();
    return data;
  }
}

export default LotteMateApi;