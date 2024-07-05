import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import LotteMateApi from "../../../api/lotteApi";
import 'bootstrap/dist/css/bootstrap.min.css';
import './EventPage.css';

const api = new LotteMateApi();

const EventPage = () => {
  const [events, setEvents] = useState([]);
  const [filteredEvents, setFilteredEvents] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    document.title = '롯데시네마 이벤트 목록';
  }, []);

  useEffect(() => {
    const fetchEvents = async () => {
      const data = await api.getLotteEventListAsync(20); // 초기 이벤트 타입을 영화로 설정
      setEvents(data);
      setFilteredEvents(data);
    };
    fetchEvents();
  }, []);

  const toggleNavItem = async (type) => {
    const data = await api.getLotteEventListAsync(type);
    setEvents(data);
    setFilteredEvents(data);
  };

  const searchBarOnChange = (event) => {
    const text = event.target.value.toLowerCase().trim();
    if (!text) {
      setFilteredEvents(events);
    } else {
      setFilteredEvents(events.filter(e => e.eventName.toLowerCase().includes(text)));
    }
  };

  const cardOnClick = (eventId) => {
    window.location.href = `https://event.lottecinema.co.kr/NLCHS/Event/EventTemplateInfo?eventId=${eventId}`;
  };

  return (
    <div className="article">
      <div className="header">
        <ul className="nav nav-tabs">
          <li className="nav-item">
            <button className="nav-link active" onClick={() => toggleNavItem(20)} data-bs-toggle="tab" type="button" role="tab" aria-selected="true">영화</button>
          </li>
          <li className="nav-item">
            <button className="nav-link" onClick={() => toggleNavItem(40)} data-bs-toggle="tab" type="button" role="tab" aria-selected="false">시사회/무대인사</button>
          </li>
          <li className="nav-item">
            <button className="nav-link" onClick={() => toggleNavItem(10)} data-bs-toggle="tab" type="button" role="tab" aria-selected="false">HOT</button>
          </li>
          <li className="nav-item">
            <button className="nav-link" onClick={() => toggleNavItem(30)} data-bs-toggle="tab" type="button" role="tab" aria-selected="false">극장</button>
          </li>
          <li className="nav-item">
            <button className="nav-link" onClick={() => toggleNavItem(50)} data-bs-toggle="tab" type="button" role="tab" aria-selected="false">제휴</button>
          </li>
        </ul>
        <div className="search-bar">
          <form className="search-form" onSubmit={e => e.preventDefault()}>
            <input className="event-input form-control" onChange={searchBarOnChange} placeholder="이벤트 검색" />
            <button className="btn btn-outline-success cgv-gradient" type="button">검색</button>
          </form>
        </div>
      </div>
      <div className="container">
        <div className="wrap">
          {filteredEvents.length > 0 ? (
            filteredEvents.map(item => (
              <div key={item.eventId} className="card" onClick={() => cardOnClick(item.eventId)}>
                <img src={item.imageUrl} alt={item.eventName} />
                <div className="card-body">
                  <p className="card-title" dangerouslySetInnerHTML={{ __html: item.eventName }} />
                  <p className="card-text">{`${item.progressStartDate} ~ ${item.progressEndDate}`}</p>
                </div>
              </div>
            ))
          ) : (
            <p>이벤트가 없습니다.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default EventPage;
