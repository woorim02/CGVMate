import Constants from '../constants';

class MegaboxMateApi {
  constructor() {
    this.apiHost = Constants.API_HOST;
  }

  async getGiveawayEventListAsync() {
    try {
      const response = await fetch(`${this.apiHost}/megabox/event/giveaway/list`);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const events = await response.json();
      return events;
    } catch (error) {
      console.error('Error fetching giveaway event list:', error);
      throw error;
    }
  }

  async getGiveawayEventDetailAsync(goodsNo) {
    try {
      const response = await fetch(`${this.apiHost}/megabox/event/giveaway/detail?goodsNo=${goodsNo}`);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const detail = await response.json();
      return detail;
    } catch (error) {
      console.error(`Error fetching giveaway event detail for goodsNo ${goodsNo}:`, error);
      throw error;
    }
  }
}

export default MegaboxMateApi;