import React, { useState, useEffect, useRef } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import './GiveawayPage.css';
import CgvMateApi from '../../../../api/cgvmateApi';

const GiveawayPage = () => {
  const [eventList, setEventList] = useState([]);
  const [filteredEventList, setFilteredEventList] = useState([]);
  const api = useRef(new CgvMateApi()).current;

  useEffect(() => {
    const fetchEvents = async () => {
      const events = await api.getGiveawayEventListAsync();
      setEventList(events);
      setFilteredEventList(events);
    };
    fetchEvents();
  }, [api]);

  const handleSearchChange = (event) => {
    const searchText = event.target.value.toLowerCase().trim();
    if (!searchText) {
      setFilteredEventList(eventList);
    } else {
      const filtered = eventList.filter(event => event.title.toLowerCase().includes(searchText));
      setFilteredEventList(filtered);
    }
  };

  const renderAds = (index, total) => {
    if (index === Math.floor(total / 3) || (index === Math.floor(total * 2 / 3) && total >= 20)) {
      return (
        <div key={`ad-${index}`} className="ad-container">
          <script async src="https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-2422895337222657" crossorigin="anonymous"></script>
          <ins className="adsbygoogle" style={{ display: 'block' }} data-ad-client="ca-pub-2422895337222657" data-ad-slot="1843074752" data-ad-format="auto" data-full-width-responsive="true"></ins>
          <script>
            {`(adsbygoogle = window.adsbygoogle || []).push({});`}
          </script>
        </div>
      );
    }
    return null;
  };

  return (
    <div className="article">
      <header className="header">
        <div className="title">
          <p>
            진행중인 경품 이벤트&nbsp;
            <strong>
              {filteredEventList && <span id="listcount">{filteredEventList.length}</span>}
            </strong>
            개
          </p>
        </div>
        <div className="search-bar">
          <form className="search-form" onSubmit={(e) => e.preventDefault()}>
            <input
              className="event-input form-control"
              onChange={handleSearchChange}
              placeholder="이벤트 검색"
            />
            <button className="btn btn-outline-success cgv-gradient" type="button">검색</button>
          </form>
        </div>
      </header>
      <div className="body">
        <ul className="eventlist">
          {filteredEventList ? filteredEventList.map((item, index) => (
            <React.Fragment key={item.eventIndex}>
              {renderAds(index, filteredEventList.length)}
              <li className="item" style={{ cursor: 'pointer' }}>
                <a href={`/cgv/event/giveaway/detail?eventIndex=${item.eventIndex}`}>
                  <div className="text-container">
                    <strong className="item-title">{item.title}</strong>
                    <span className="item-period">
                      <span style={{ marginRight: '5px' }}>{item.period}</span>
                      <svg width="13px" height="13px" viewBox="0 0 16 16" className="bi bi-eye" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                        <path fillRule="evenodd" d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8zM1.173 8a13.134 13.134 0 0 0 1.66 2.043C4.12 11.332 5.88 12.5 8 12.5c2.12 0 3.879-1.168 5.168-2.457A13.134 13.134 0 0 0 14.828 8a13.133 13.133 0 0 0-1.66-2.043C11.879 4.668 10.119 3.5 8 3.5c-2.12 0-3.879 1.168-5.168 2.457A13.133 13.133 0 0 0 1.172 8z" />
                        <path fillRule="evenodd" d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5zM4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0z" />
                      </svg>
                      <span className="item-views">{item.views}</span>
                    </span>
                  </div>
                  <span className="item-dday">{item.dDay}</span>
                </a>
              </li>
            </React.Fragment>
          )) : (
            <React.Fragment>
              {Array.from({ length: 20 }).map((_, index) => (
                <li key={index} className="item" style={{ cursor: 'pointer' }}>
                  <p>로딩중...</p>
                </li>
              ))}
            </React.Fragment>
          )}
        </ul>
      </div>
    </div>
  );
};

export default GiveawayPage;
