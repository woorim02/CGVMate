import Constants from '../constants';
import cheerio from 'cheerio';

class CgvMateApi {
  async getEventListAsync(type) {
    const response = await fetch(`${Constants.API_HOST}/cgv/event/list?type=${type}`);
    const json = await response.json();
    return json;
  }

  async getCuponEventList(){
    const response = await fetch(`${Constants.API_HOST}/cgv/event/cupon/list`);
    const json = await response.json();
    return json;
  }

  async getGiveawayEventListAsync() {
    const response = await fetch(`${Constants.API_HOST}/cgv/event/giveaway/list`);
    const events = await response.json();
    return events;
  }

  async getGiveawayEventModelAsync(eventIndex) {
    const response = await fetch(`${Constants.API_HOST}/cgv/event/giveaway/model?eventIndex=${eventIndex}`);
    const model = await response.json();
    return model;
  }

  async getGiveawayInfoAsync(giveawayIndex, areaCode = '') {
    const response = await fetch(`${Constants.API_HOST}/cgv/event/giveaway/info?giveawayIndex=${giveawayIndex}&areaCode=${areaCode ? areaCode : ''} `);
    const info = await response.json();
    return info;
  }

  async getSpeedCuponCountsAsync() {
    const response = await fetch(`https://api.cgvmate.com//proxy/Event/2021/fcfs/default.aspx?idx=6`);
    const html = await response.text();
    const $ = cheerio.load(html);

    const nameNodes = $('.btn_reserve');
    const countNodes = $('.progress-number');

    const list = [];
    nameNodes.each((i, elem) => {
      const name = $(elem).attr('href');
      const splitValue = name.split(',');
      const movieIndex = splitValue[0].replace(/\D/g, '');
      const movieGroupCd = splitValue[1].replace(/\D/g, '');
      const movieTitle = splitValue[2].replace(/'/g, '');
      const countStr = countNodes.eq(i).attr('aria-valuenow');
      const count = parseInt(countStr.replace(/\D/g, ''), 10);
      list.push({ count, movieIndex, movieGroupCd, movieTitle });
    });
    return list;
  }

  async getSurpriseCuponCountAsync(index) {
    const response = await fetch(`https://api.cgvmate.com/proxy/WebApp/EventNotiV4/EventDetailGeneralUnited.aspx?seq=${index}`);
    const msg = await response.text();

    let url = null;
    msg.split('\n').forEach(s => {
      if (s.includes('https://m.cgv.co.kr/Event/2021/fcfs/default.aspx')) {
        url = s.trim().replace(/[\t\n\r]/g, '').replace('window.location.href = "', '').replace('";', '');
      }
    });

    const newResponse = await fetch(url.replace('m.cgv.co.kr', 'api.cgvmate.com/proxy'));
    const newHtml = await newResponse.text();
    const $ = cheerio.load(newHtml);

    const titleStr = $('meta[property="og:title"]').attr('content');
    const countStr = $('.progress-number').attr('aria-valuenow').trim().replace(',', '').replace('쿠폰 사용 수량', '');
    const count = parseInt(countStr, 10);
    const ava = !$('.btn_reserve').attr('href').includes('소진되었습니다');

    return { title: titleStr, count, index, isAvailable: ava };
  }

  async getGiveawayEventCountAsync(idx, gidx, tcd) {
    const response = await fetch(`https://api.cgvmate.com/proxy/Event/GiveawayEventSignup.aspx?idx=${idx}&gidx=${gidx}&tcd=${tcd}`);
    const msg = await response.text();
    
    const remainCntMatch = msg.match(/var remainCnt = "(.*?)";/);
    const totalCntMatch = msg.match(/var totalCnt = "(.*?)";/);

    const remainCnt = remainCntMatch ? remainCntMatch[1] : null;
    const totalCnt = totalCntMatch ? totalCntMatch[1] : null;

    return { remainCnt: this.decrypt(remainCnt), totalCnt: this.decrypt(totalCnt) };
  }

  decrypt(data) {
    const iv = CryptoJS.enc.Base64.parse('YjUxMWM3MWI5M2E3NDhmNA==');
    const key = CryptoJS.enc.Base64.parse('YjUxMWM3MWI5M2E3NDhmNDc1YzM5YzY1ZGQwZTFlOTQ=');

    const encryptedData = CryptoJS.enc.Base64.parse(data);
    
    const decrypted = CryptoJS.AES.decrypt(
        { ciphertext: encryptedData }, 
        key, 
        { iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 }
    );
    
    return decrypted.toString(CryptoJS.enc.Utf8);
  }
}

export default CgvMateApi;
