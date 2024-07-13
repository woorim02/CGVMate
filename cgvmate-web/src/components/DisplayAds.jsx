import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import { useLocation } from 'react-router-dom';

const DisplayAds = ({ adSlot, style }) => {
  const location = useLocation();

  const pushAd = () => {
    try {
      const adsbygoogle = window.adsbygoogle;
      if (adsbygoogle) {
        adsbygoogle.push({});
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
    // 광고 요소가 지워지고 다시 렌더링될 시간을 주기 위한 딜레이 추가
    const timeout = setTimeout(() => {
      pushAd();
    }, 1000);

    return () => clearTimeout(timeout);
  }, [location]);

  return (
    <ins
      className="adsbygoogle"
      style={style}
      data-ad-client="ca-pub-2422895337222657"
      data-ad-slot={adSlot}
      data-ad-format="auto"
      data-full-width-responsive="true"
    ></ins>
  );
};

DisplayAds.propTypes = {
  adSlot: PropTypes.string.isRequired,
  style: PropTypes.object,
};

DisplayAds.defaultProps = {
  style: { display: 'block' },
};

export default DisplayAds;
