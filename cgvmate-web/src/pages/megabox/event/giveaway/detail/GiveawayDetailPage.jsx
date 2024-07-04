import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import MegaboxMateApi from '../../../../../api/megaboxApi';
import 'bootstrap/dist/css/bootstrap.min.css';
import './GiveawayDetailPage.css';

const api = new MegaboxMateApi();

const GiveawayDetailPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const eventID = searchParams.get('eventIndex');
  const initialAreaCode = searchParams.get('areaCode') || '10';
  const [currentArea, setCurrentArea] = useState(initialAreaCode);
  const [giftId, setGiftId] = useState(null);
  const [giftName, setGiftName] = useState(null);
  const [info, setInfo] = useState(null);

  useEffect(() => {
    const fetchEventData = async () => {
      if (!eventID) return;
      try {
        const eventInfo = await api.getGiveawayEventDetailAsync(eventID);
        setGiftId(eventInfo.id);
        setGiftName(eventInfo.title);
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
        const eventInfo = await api.getGiveawayEventDetailAsync(eventID);
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
      <div className="header">
        <div className="header-title">
          <p id="header-text">잔여 수량 확인</p>
        </div>
      </div>
      <div className="content-wrap">
        <div className="header-title">
          <p className="header-content">{giftName}</p>
        </div>
        <div className="content">
          {info ? (
            <>
              <div className="contentL">
                <ul className="area-list">
                  {info.areas.map((item) => (
                    <li
                      key={item.code}
                      id={`area-${item.code}`}
                      className={item.code === currentArea ? 'active' : ''}
                      style={{ display: 'block' }}
                      onClick={() => selectAreaTheaterList(item.code)}
                    >
                      <a href={`#`}>
                        {item.name}
                      </a>
                    </li>
                  ))}
                </ul>
              </div>
              <div className="contentR">
                <ul className="theater-list">
                  {info.areas
                    .find((area) => area.code === currentArea)
                    .infos.map((item, i) => (
                      <li key={i} className="theater">
                        <div className="theater-name">
                          <b className="theater-name" dangerouslySetInnerHTML={{ __html: item.name }} />
                        </div>
                        <div>
                          <b className={`theater-state ${item.fAc}`}>{item.fAc}</b>
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
