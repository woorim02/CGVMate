import React, { useEffect } from 'react';

const BannerAds = () => {
  useEffect(() => {
    const pushAd = () => {
      try {
        const adsbygoogle = window.adsbygoogle;
        adsbygoogle.push({});
      } catch (e) {
        console.error(e);
      }
    };

    let interval = setInterval(() => {
      // Check if Adsense script is loaded every 300ms
      if (window.adsbygoogle) {
        pushAd();
        // clear the interval once the ad is pushed so that function isn't called indefinitely
        clearInterval(interval);
      }
    }, 300);

    return () => {
      clearInterval(interval);
    };
  }, []);

  return (
    <ins
      className="adsbygoogle"
      style={{display: 'inline-block', width: '320px', height: '50px'}}
      data-ad-client="ca-pub-2422895337222657"
      data-ad-slot="4357139561"
    ></ins>
  );
};

export default BannerAds;
