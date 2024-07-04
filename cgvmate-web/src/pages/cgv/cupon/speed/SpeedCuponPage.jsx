import React, { useState, useEffect, useRef } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import './SpeedCuponPage.css';
import CgvMateApi from '../../../../api/cgvmateApi';
import DisplayAds from '../../../../components/DisplayAds';

const api = new CgvMateApi();

const SpeedCuponPage = () => {
  const [cupons, setCupons] = useState([]);
  const [processTexts, setProcessTexts] = useState([]);
  const [isRunning, setIsRunning] = useState(false);
  const [selectedMovieIndex, setSelectedMovieIndex] = useState('nullmoviewqwegv');
  const intervalRef = useRef(null);

  useEffect(() => {
    document.title = '스피드쿠폰 조회';
  }, []);

  useEffect(() => {
    const fetchCupons = async () => {
      const response = await api.getSpeedCuponCountsAsync();
      setCupons(response);
    };
    fetchCupons();

    return () => {
      setIsRunning(false);
    };
  }, []);

  useEffect(() => {
    if (isRunning) {
      intervalRef.current = setInterval(async () => {
        const response = await api.getSpeedCuponCountsAsync();
        const selectedCupons = selectedMovieIndex === 'nullmoviewqwegv'
          ? response
          : response.filter(x => x.movieIndex === selectedMovieIndex);
        const newProcessTexts = selectedCupons.map(cupon => {
            const now = new Date();
            const timeString = `${now.toLocaleTimeString()}.${now.getMilliseconds()}`;
          const countText = cupon.count % 100 !== 0
            ? `<span class='count' style='color:lawngreen;'>${cupon.count}</span>`
            : `<span class='count'>${cupon.count}</span>`;
          return `[${timeString}] [${cupon.movieTitle}] : ${countText}`;
        });
        setProcessTexts(newProcessTexts);
      }, 500);

      return () => clearInterval(intervalRef.current);
    }
  }, [isRunning, selectedMovieIndex]);

  const handleToggleChange = () => {
    setIsRunning(!isRunning);
  };

  const handleMovieIndexChange = (event) => {
    setSelectedMovieIndex(event.target.value);
  };

  return (
    <div className="article" id="speed-cupon-article">
      <div className="article-header">
        <div className="switch-container">
          <label id="toggle-switch-label" htmlFor="toggle_switch1">
            <span>스피드쿠폰 자동 확인</span>
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
          <select className="form-select" style={{ paddingLeft: '0.5rem', marginLeft: '0.5rem' }} value={selectedMovieIndex} onChange={handleMovieIndexChange}>
            <option value="nullmoviewqwegv">All</option>
            {cupons.map((cupon, index) => (
              <option key={index} value={cupon.movieIndex}>{cupon.movieTitle}</option>
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

export default SpeedCuponPage;
