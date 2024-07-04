import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Helmet } from 'react-helmet-async';
import 'bootstrap/dist/css/bootstrap.min.css';
import './GiveawayDetailPage.css';
import CgvMateApi from '../../../../../api/cgvmateApi';
import DisplayAds from '../../../../../components/DisplayAds'

const api = new CgvMateApi();

const GiveawayDetailPage = () => {
  const [searchParams] = useSearchParams();
  const eventIndex = searchParams.get('eventIndex');
  const areaCode = searchParams.get('areaCode') || '13';
  const [model, setModel] = useState(null);
  const [info, setInfo] = useState(null);
  const [currentArea, setCurrentArea] = useState(areaCode || '13');

  useEffect(() => {
    const fetchModelData = async () => {
      try {
        const modelResponse = await api.getGiveawayEventModelAsync(eventIndex);
        setModel(modelResponse);
        // Fetch info data after model data is set
        const infoResponse = await api.getGiveawayInfoAsync(modelResponse.giveawayIndex, currentArea);
        setInfo(infoResponse);
      } catch (error) {
        console.error('Error fetching event data:', error);
      }
    };
    if (eventIndex) {
      fetchModelData();
    }
  }, [eventIndex, currentArea]);

  const selectAreaTheaterList = async (areaCode) => {
    try {
      const infoResponse = await api.getGiveawayInfoAsync(model.giveawayIndex, areaCode);
      setInfo(infoResponse);
      setCurrentArea(areaCode);
    } catch (error) {
      console.error('Error selecting area theater list:', error);
    }
  };

  const countTypeCodeToText = (type) => {
    switch (type) {
      case "type4": return "마감 되었습니다.";
      case "type3": return "재고 소진 임박 입니다.";
      case "type2.5": return "재고 소진 중입니다.";
      case "type2": return "재고 보유 가능성이 높습니다.";
      default: return "unknown";
    }
  };

  const countTypeCodeToColor = (type) => {
    switch (type) {
      case "type4": return "#d3d3d3";
      case "type3": return "#fb4357";
      case "type2.5": return "#ffd966";
      case "type2": return "#25c326";
      default: return "unknown";
    }
  };

  return (
    <div className="tl">
      {model && (
        <Helmet>
          <title>{model.contents}</title>
          <meta name="description" content={`${model.contents} 수량 확인`} />
          <meta property="og:type" content="website" />
          <meta property="og:title" content={`${model.contents} 수량 확인`} />
          <meta property="og:description" content={`${model.contents} 수량 확인`} />
          <meta property="og:url" content={window.location.href} />
        </Helmet>
      )}
      <div className="c-g-d-header">
        <div className="c-g-d-header-title">
          <p id="header-text">잔여 수량 확인</p>
        </div>
      </div>

      <div className="content-wrap">
        <div className="c-g-d-header-title">
          <span className="c-g-d-header-content">
            {model && model.title}
          </span>
        </div>
        <DisplayAds adSlot='8167919304' />
        <div className="content">
          {info && info.AreaList ? (
            <>
              {info.AreaList.reduce((sum, x) => sum + parseInt(x.TheaterCount), 0) > 10 && (
                <div className="contentL">
                  <ul className="area-list">
                    {info.AreaList.map((item) => (
                      <li key={item.AreaCode} id={`area-${item.AreaCode}`} className={item.AreaCode === currentArea ? 'active' : ''} style={{ display: 'block' }} onClick={() => selectAreaTheaterList(item.AreaCode)}>
                        <a href={`?eventIndex=${eventIndex}&areaCode=${item.AreaCode}`}>
                          {item.AreaName} <span>({item.TheaterCount})</span>
                        </a>
                      </li>
                    ))}
                  </ul>
                </div>
              )}
              <div className="c-g-d-contentR">
                <ul className="theater-list">
                  {info.TheaterList.map((item, i) => (
                    <li key={i} className="theater">
                      <div className="theater-name">
                        <span className="theater-name">{item.TheaterName}</span>
                      </div>
                      <div>
                        <span className="c-g-d-theater-count" style={{ backgroundColor: countTypeCodeToColor(item.CountTypeCode) }}>
                          {item.GiveawayRemainCount}
                        </span>
                        <span className="theater-state">&nbsp;{countTypeCodeToText(item.CountTypeCode)}</span>
                      </div>
                    </li>
                  ))}
                </ul>
              </div>
            </>
          ) : (
            <>
              <div className="contentL">
                <ul className="area-list">
                  {[...Array(10)].map((_, i) => (
                    <li key={i} className="theater" style={{ display: 'block' }}>
                      <a href="javascript:void(0)"></a>
                    </li>
                  ))}
                </ul>
              </div>
              <div className="contentR">
                <ul className="theater-list">
                  {[...Array(15)].map((_, i) => (
                    <li key={i} className="theater" style={{ display: 'block', color: 'white' }}>
                      ...
                    </li>
                  ))}
                </ul>
              </div>
            </>
          )}
        </div>
        <div>
          {/* FooterAd component could be added here */}
        </div>
      </div>
    </div>
  );
};

export default GiveawayDetailPage;
