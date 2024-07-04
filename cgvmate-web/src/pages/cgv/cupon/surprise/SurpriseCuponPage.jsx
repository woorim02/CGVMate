import React, { useState, useEffect, useRef } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import './SurpriseCuponPage.css';
import CgvMateApi from '../../../../api/cgvmateApi';
import DisplayAds from '../../../../components/DisplayAds';

const api = new CgvMateApi();

const SurpriseCuponPage = () => {
  const [cupons, setCupons] = useState([]);
  const [processTexts, setProcessTexts] = useState([]);
  const [isRunning, setIsRunning] = useState(false);
  const [selectedCuponIndex, setSelectedCuponIndex] = useState('');
  const intervalRef = useRef(null);

  useEffect(() => {
    document.title = '서프라이즈 쿠폰 조회';
  }, []);

  useEffect(() => {
    const fetchCupons = async () => {
      const cuponEvents = await api.getEventListAsync(4);
      const filteredCupons = cuponEvents
        .filter(event => event.eventName.includes('선착순 무료 쿠폰'))
        .map(event => ({
          index: event.eventId,
          title: event.eventName,
          count: 0,
          isAvailable: false
        }));
      setCupons(filteredCupons);
      if (filteredCupons.length > 0) {
        setSelectedCuponIndex(filteredCupons[0].index);
      }
    };
    fetchCupons();

    return () => {
      setIsRunning(false);
      if (intervalRef.current) {
        clearInterval(intervalRef.current);
      }
    };
  }, []);

  useEffect(() => {
    if (isRunning) {
      intervalRef.current = setInterval(async () => {
        const cupon = await api.getSurpriseCuponCountAsync(selectedCuponIndex);
        const timeString = `${new Date().toLocaleTimeString()}.${new Date().getMilliseconds()}`;
        const countText = cupon.isAvailable
          ? `<span class='count' style='color:lawngreen;'>${cupon.count}</span>`
          : `<span class='count'>${cupon.count}</span>`;
        setProcessTexts(prevTexts => [
          `[${timeString}] [${cupon.title}] : ${countText}`,
          ...prevTexts
        ]);
      }, 500);

      return () => clearInterval(intervalRef.current);
    }
  }, [isRunning, selectedCuponIndex]);

  const handleToggleChange = () => {
    setIsRunning(!isRunning);
  };

  const handleCuponIndexChange = (event) => {
    setSelectedCuponIndex(event.target.value);
  };

  return (
    <div className="article" id="speed-cupon-article">
      <div className="article-header">
        <div className="switch-container">
          <label id="toggle-switch-label" htmlFor="toggle_switch1">
            <span>서프라이즈 쿠폰 자동 확인</span>
            <span className="switch-wrap">
              <input
                type="checkbox"
                id="toggle_switch1"
                style={{ paddingLeft: '1rem', margin: 'auto', float: 'right', width: '40px', height: '20px' }}
                onChange={handleToggleChange}
              />
            </span>
          </label>
        </div>
        <div className="select-container border-top">
          <select className="form-select" style={{ paddingLeft: '0.5rem', marginLeft: '0.5rem' }} value={selectedCuponIndex} onChange={handleCuponIndexChange}>
            {cupons.map((cupon, index) => (
              <option key={index} value={cupon.index}>{cupon.title}</option>
            ))}
          </select>
        </div>
      </div>
      <div className="article-body">
        <DisplayAds adSlot='2485625472'/>
        <ul className="list-group">
          {processTexts.map((text, index) => (
            <li key={index}>
              <p className="list-group-item" dangerouslySetInnerHTML={{ __html: text }} />
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default SurpriseCuponPage;
