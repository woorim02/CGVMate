import React, { useState, useEffect } from 'react';
import './Home.css';

const Home = () => {
  
  useEffect(() => {
    document.title = 'CGV 도우미 | 홈';
  }, []);

  return (
    <article className="main_article">
      <div className="px-4 py-5 my-5 text-center">
        <h2 className="fw-bold">CGV 도우미</h2>
        <div className="col-lg-6 mx-auto">
          <p className="mb-0">
            CGV 도우미는 다양한 이벤트와 경품 관련 정보를<br /> 손쉽게 확인할 수 있는 안내 사이트입니다.
          </p>
          <div style={{height: '5px'}}></div>
          <p className="mb-5">
            공식 웹사이트에서 지원하지 않는 다양한 기능들을 제공합니다. :)
          </p>
        </div>
        <div>
          <a className="cgv-button button" href="/cgv/event">CGV 이벤트 목록</a>
          <a className="cgv-button button" href="/cgv/event/giveaway">CGV 경품 이벤트 현황</a>
          <a className="cgv-button button" href="/cgv/event/cupon/speed">스피드쿠폰 조회</a>
          <a className="cgv-button button" href="/cgv/event/cupon/surprise">서프라이즈 쿠폰 조회</a>
        </div>
        <div>
          <a className="cgv-button button" href="/lotte/event">롯데시네마 이벤트 목록</a>
          <a className="cgv-button button" href="/lotte/event/giveaway">롯데시네마 경품 이벤트 현황</a>
        </div>
        <div>
          <a className="cgv-button button" href="/megabox/event/giveaway">메가박스 경품 이벤트 현황</a>
        </div>
      </div>
    </article>
  );
};

export default Home;
