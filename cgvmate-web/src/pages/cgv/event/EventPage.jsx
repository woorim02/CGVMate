import React, { useState, useEffect, useRef } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import './EventPage.css'
import CgvMateApi from '../../../api/cgvmateApi';

const EventPage = () => {
  const [events, setEvents] = useState([]);
  const [filteredEvents, setFilteredEvents] = useState([]);
  const [eventType, setEventType] = useState('Special');
  const api = useRef(new CgvMateApi()).current;  // useRef를 사용하여 api 객체를 고정

  useEffect(() => {
      const fetchEvents = async () => {
          const eventTypeKey = CgvEventType[eventType];
          const eventData = await api.getEventListAsync(eventTypeKey);
          const processedData = eventData.map(item => {
              if (item.eventName.includes("선착순 무료 쿠폰")) {
                  item.eventName = item.eventName.replace("선착순 무료 쿠폰", "서프라이즈 쿠폰");
              }
              return item;
          });
          setEvents(processedData);
          setFilteredEvents(processedData);
      };
      fetchEvents();
  }, [eventType]);  // api를 의존성 배열에 포함하지 않음

  const toggleNavItem = (type) => {
      setEventType(type);
  };

  const handleSearchChange = (event) => {
      const searchText = event.target.value.toLowerCase().trim();
      if (!searchText) {
          setFilteredEvents(events);
      } else {
          const filtered = events.filter(event => event.eventName.toLowerCase().trim().includes(searchText));
          setFilteredEvents(filtered);
      }
  };

  const handleCardClick = (event) => {
    window.location.href = `https://m.cgv.co.kr/WebApp/EventNotiV4/EventDetailGeneralUnited.aspx?seq=${event.eventId}`;
  };

  return (
      <div className="article">
          <header className="e-header">
              <ul className="nav nav-tabs">
                  {Object.keys(CgvEventType).map((type, index) => (
                      <li key={index} className="nav-item">
                          <button
                              className={`nav-link ${eventType === type ? 'active' : ''}`}
                              onClick={() => toggleNavItem(type)}
                              type="button"
                              role="tab"
                              aria-selected={eventType === type}
                          >
                              {type}
                          </button>
                      </li>
                  ))}
              </ul>
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
          <div className="container">
              <div className="wrap">
                  {filteredEvents.length > 0 ? filteredEvents.map(event => (
                      <a key={event.eventId} className="card" onClick={() => handleCardClick(event)} style={{ textDecoration: 'none' }}>
                          <img src={event.imageSource} alt={event.eventName} />
                          <div className="card-body">
                              <p className="card-title" dangerouslySetInnerHTML={{ __html: event.eventName }}></p>
                              {event.period !== "상시진행" ? (
                                  <p className="card-text">{`${new Date(event.startDate).toLocaleDateString()} ~ ${new Date(event.endDate).toLocaleDateString()}`}</p>
                              ) : (
                                  <p className="card-text">상시진행</p>
                              )}
                          </div>
                      </a>
                  )) : <p>이벤트가 없습니다.</p>}
              </div>
          </div>
      </div>
  );
};

const CgvEventType = {
  Special: '001',
  영화: '004',
  극장: '005',
  제휴: '006',
  멤버십_클럽: '008',
  지난이벤트: '100'
};

export default EventPage;
