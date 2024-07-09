import Constants from '../constants';

class LotteMateApi {
  async getLotteEventListAsync(type) {
    const response = await fetch(`${Constants.API_HOST}/lotte/event/list?type=${type}`);
    const data = await response.json();
    return data;
  }

  async getCuponEventList(){
    const response = await fetch(`${Constants.API_HOST}/lotte/event/cupon/list`);
    const json = await response.json();
    return json;
  }

  async getLotteGiveawayEventListAsync() {
    const response = await fetch(`${Constants.API_HOST}/lotte/event/giveaway/list`);
    const data = await response.json();
    return data;
  }

  async getLotteGiveawayEventModelAsync(eventID) {
    const response = await fetch(`${Constants.API_HOST}/lotte/event/giveaway/model?event_id=${eventID}`);
    const data = await response.json();
    return data;
  }

  async getLotteGiveawayInfoAsync(event_id, gift_id) {
    const response = await fetch(`${Constants.API_HOST}/lotte/event/giveaway/info?event_id=${event_id}&gift_id=${gift_id}`);
    const data = await response.json();
    return data;
  }
}

export default LotteMateApi;