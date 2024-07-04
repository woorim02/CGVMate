import React, { useState, useEffect } from 'react';
import MegaboxMateApi from '../../../../api/megaboxApi';
import 'bootstrap/dist/css/bootstrap.min.css';
import './GiveawayPage.css';
import DisplayAds from '../../../../components/DisplayAds';

const api = new MegaboxMateApi();

const GiveawayPage = () => {
  const [events, setEvents] = useState([]);
  const [filteredEvents, setFilteredEvents] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    document.title = '메가박스 경품 이벤트 현황';
  }, []);

  useEffect(() => {
    const fetchEvents = async () => {
      try {
        const eventList = await api.getGiveawayEventListAsync();
        setEvents(eventList);
        setFilteredEvents(eventList);
        setLoading(false);
      } catch (error) {
        console.error('Error fetching event list:', error);
      }
    };

    fetchEvents();
  }, []);

  const handleSearchChange = (event) => {
    const text = event.target.value.toLowerCase();
    if (!text) {
      setFilteredEvents(events);
    } else {
      setFilteredEvents(events.filter(e => e.title.toLowerCase().includes(text)));
    }
  };

  return (
    <div className="article">
      <div className="m-header">
        <div className="title">
          <p>
            진행중인 경품 이벤트
            <strong>
              <span id="listcount">{filteredEvents.length}</span>
            </strong>
            개
          </p>
        </div>
        <div className="search-bar">
          <form className="search-form" onSubmit={e => e.preventDefault()}>
            <input className="event-input form-control" onChange={handleSearchChange} placeholder="이벤트 검색" />
            <button className="btn btn-outline-success cgv-gradient" type="button">검색</button>
          </form>
        </div>
      </div>

      <div className="body">
        <ul className="m-eventlist">
        <DisplayAds adSlot='3730871491'/>
          {loading ? (
            Array.from({ length: 20 }).map((_, i) => (
              <li key={i} className="item" style={{ cursor: 'pointer' }}>
                <p>로딩중...</p>
              </li>
            ))
          ) : (
            filteredEvents.map((item, i) => (
              <li key={item.id} className="item" style={{ cursor: 'pointer' }}>
                <a href={`/megabox/event/giveaway/detail?eventIndex=${item.id}`}>
                  <div className="text-container">
                    <strong className="item-title" dangerouslySetInnerHTML={{ __html: item.title }} />
                    <span className="item-period">
                      <svg
                        width="13px"
                        height="13px"
                        viewBox="0 0 16 16"
                        className="bi bi-eye"
                        fill="currentColor"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          fillRule="evenodd"
                          d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8zM1.173 8a13.134 13.134 0 0 0 1.66 2.043C4.12 11.332 5.88 12.5 8 12.5c2.12 0 3.879-1.168 5.168-2.457A13.134 13.134 0 0 0 14.828 8a13.133 13.133 0 0 0-1.66-2.043C11.879 4.668 10.119 3.5 8 3.5c-2.12 0-3.879 1.168-5.168 2.457A13.133 13.133 0 0 0 1.172 8z"
                        />
                        <path
                          fillRule="evenodd"
                          d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5zM4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0z"
                        />
                      </svg>
                      <span className="item-views">{item.viewCount}</span>
                    </span>
                  </div>
                </a>
              </li>
            ))
          )}
        </ul>
      </div>
    </div>
  );
};

export default GiveawayPage;
