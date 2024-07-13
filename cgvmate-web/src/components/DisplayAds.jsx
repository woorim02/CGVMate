import React, { useEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import { useLocation } from 'react-router-dom';

const DisplayAds = ({ adSlot, style }) => {
  const location = useLocation();
  const adRef = useRef(null);

  const pushAd = () => {
    try {
      if (window.adsbygoogle && adRef.current) {
        // Remove the existing ad element
        adRef.current.innerHTML = '';
        // Create a new ad element
        const adElement = document.createElement('ins');
        adElement.className = 'adsbygoogle';
        Object.assign(adElement.style, style);
        adElement.setAttribute('data-ad-client', 'ca-pub-2422895337222657');
        adElement.setAttribute('data-ad-slot', adSlot);
        adElement.setAttribute('data-ad-format', 'auto');
        adElement.setAttribute('data-full-width-responsive', 'true');
        adRef.current.appendChild(adElement);
        // Push the new ad element
        window.adsbygoogle.push({});
      }
    } catch (e) {
      console.error(e);
    }
  };

  useEffect(() => {
    let interval = setInterval(() => {
      // 300ms마다 Adsense 스크립트가 로드되었는지 확인
      if (window.adsbygoogle) {
        pushAd();
        // 광고가 푸시된 후 인터벌을 클리어하여 함수가 무한정 호출되지 않도록 함
        clearInterval(interval);
      }
    }, 300);

    return () => {
      clearInterval(interval);
    };
  }, []);

  useEffect(() => {
    // URL이 변경될 때 광고 갱신
    pushAd();
  }, [location]);

  return <div ref={adRef}></div>;
};

DisplayAds.propTypes = {
  adSlot: PropTypes.string.isRequired,
  style: PropTypes.object,
};

DisplayAds.defaultProps = {
  style: { display: 'block' },
};

export default DisplayAds;
