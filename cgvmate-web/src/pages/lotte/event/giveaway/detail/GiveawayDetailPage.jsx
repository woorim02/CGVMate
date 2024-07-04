import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Helmet } from 'react-helmet-async';
import LotteMateApi from '../../../../../api/lotteApi';
import 'bootstrap/dist/css/bootstrap.min.css';
import './GiveawayDetailPage.css';
import DisplayAds from '../../../../../components/DisplayAds';

const api = new LotteMateApi();

const GiveawayDetailPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const eventID = searchParams.get('eventIndex');
  const initialAreaCode = searchParams.get('areaCode') || '0001';
  const [currentArea, setCurrentArea] = useState(initialAreaCode);
  const [giftId, setGiftId] = useState(null);
  const [giftName, setGiftName] = useState(null);
  const [info, setInfo] = useState(null);

  useEffect(() => {
    const fetchEventData = async () => {
      if (!eventID) return;
      try {
        const eventModel = await api.getLotteGiveawayEventModelAsync(eventID);
        setGiftId(eventModel.frGiftID);
        setGiftName(eventModel.frGiftNm);
        const eventInfo = await api.getLotteGiveawayInfoAsync(eventID, eventModel.frGiftID);
        setInfo(eventInfo);
      } catch (error) {
        console.error('Error fetching event data:', error);
      }
    };

    fetchEventData();
  }, [eventID]);

  useEffect(() => {
    const fetchAreaData = async () => {
      if (!eventID || !giftId) return;
      try {
        const eventInfo = await api.getLotteGiveawayInfoAsync(eventID, giftId);
        setInfo(eventInfo);
      } catch (error) {
        console.error('Error fetching area data:', error);
      }
    };

    fetchAreaData();
  }, [eventID, currentArea, giftId]);

  const selectAreaTheaterList = (areaCode) => {
    setCurrentArea(areaCode);
    setSearchParams({ eventIndex: eventID, areaCode: areaCode }, { replace: true });
  };

  return (
    <div className="tl">
    {giftName && (
      <Helmet>
        <title>{giftName}</title>
        <meta name="description" content={`${giftName} 수량 확인`} />
        <meta property="og:type" content="website" />
        <meta property="og:title" content={`${giftName} 수량 확인`} />
        <meta property="og:description" content={`${giftName} 수량 확인`} />
        <meta property="og:url" content={window.location.href} />
      </Helmet>
    )}
      <div className="header">
        <div className="header-title">
          <p id="header-text">잔여 수량 확인</p>
        </div>
      </div>
      <div className="content-wrap">
        <div className="header-title">
          <span className="header-content">{giftName}</span>
        </div>
        <DisplayAds adSlot='8167919304'/>
        <div className="content">
          {info ? (
            <>
              <div className="contentL">
                <ul className="area-list">
                  {info.cinemaDivisions.map((item) => (
                    <li
                      key={item.detailDivisionCode}
                      id={`area-${item.detailDivisionCode}`}
                      className={item.detailDivisionCode === currentArea ? 'active' : ''}
                      style={{ display: 'block' }}
                      onClick={() => selectAreaTheaterList(item.detailDivisionCode)}
                    >
                      <a href={`#`}>
                        {item.groupNameKR} <span>({item.cinemaCount})</span>
                      </a>
                    </li>
                  ))}
                </ul>
              </div>
              <div className="l-g-d-contentR">
                <ul className="theater-list">
                  {info.cinemaDivisionGoods
                    .filter((item) => item.detailDivisionCode === currentArea)
                    .map((item, i) => (
                      <li key={i} className="theater">
                        <div className="theater-name">
                          <span className="theater-name">{item.cinemaNameKR}</span>
                        </div>
                        <div>
                          <span className="theater-state">
                            <span className="num">{item.cnt}</span> 개 이상
                          </span>
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
                    <li key={i} className="theater" style={{ display: 'block', color: 'white' }}>
                      ...
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
      </div>
    </div>
  );
};

export default GiveawayDetailPage;
