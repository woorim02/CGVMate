import React, { useState } from 'react';
import Constants from '../../constants.js';
import './Sidebar.css'

const Sidebar = () => {
  const [sections, setSections] = useState({
    cgv: true,
    lotte: true,
    megabox: true,
    github: true
  });

  const toggleCollapse = (section) => {
    setSections(prevSections => ({
      ...prevSections,
      [section]: !prevSections[section]
    }));
  };

  return (
    <div className="sidebar sidebar-body">
      <ul className="list-unstyled ps-0">
        <li className="mb-1">
          <button
            className="btn btn-toggle d-inline-flex align-items-center rounded border-0 collapsed"
            type="button"
            onClick={() => toggleCollapse('cgv')}
            aria-expanded={sections.cgv}
          >
            CGV
          </button>
          <div className={sections.cgv ? "collapse show" : "collapse"} id="home-collapse">
            <ul className="btn-toggle-nav list-unstyled fw-normal pb-1 small">
              <li>
                <a
                  href={Constants.event_}
                  className="link-body-emphasis d-inline-flex text-decoration-none rounded"
                >
                  이벤트 목록
                </a>
              </li>
              <li>
                <a
                  href={Constants.event_giveaway}
                  className="link-body-emphasis d-inline-flex text-decoration-none rounded"
                >
                  경품 이벤트 현황
                </a>
              </li>
              <li>
                <a
                  href={Constants.event_cupon_speed}
                  className="link-body-emphasis d-inline-flex text-decoration-none rounded"
                >
                  스피드쿠폰 조회
                </a>
              </li>
              <li>
                <a
                  href={Constants.event_cupon_surprise}
                  className="link-body-emphasis d-inline-flex text-decoration-none rounded"
                >
                  서프라이즈 쿠폰 조회
                </a>
              </li>
            </ul>
          </div>
        </li>
        <li className="border-top my-3"></li>
        <li className="mb-1">
          <button
            className="btn btn-toggle d-inline-flex align-items-center rounded border-0 collapsed"
            type="button"
            onClick={() => toggleCollapse('lotte')}
            aria-expanded={sections.lotte}
          >
            롯데시네마
          </button>
          <div className={sections.lotte ? "collapse show" : "collapse"} id="lotte-collapse">
            <ul className="btn-toggle-nav list-unstyled fw-normal pb-1 small">
              <li>
                <a
                  href={Constants.lotte_event}
                  className="link-body-emphasis d-inline-flex text-decoration-none rounded"
                >
                  이벤트 목록
                </a>
              </li>
              <li>
                <a
                  href={Constants.lotte_event_giveaway}
                  className="link-body-emphasis d-inline-flex text-decoration-none rounded"
                >
                  경품 이벤트 현황
                </a>
              </li>
            </ul>
          </div>
        </li>
        <li className="border-top my-3"></li>
        <li className="mb-1">
          <button
            className="btn btn-toggle d-inline-flex align-items-center rounded border-0 collapsed"
            type="button"
            onClick={() => toggleCollapse('megabox')}
            aria-expanded={sections.megabox}
          >
            메가박스
          </button>
          <div className={sections.megabox ? "collapse show" : "collapse"} id="megabox-collapse">
            <ul className="btn-toggle-nav list-unstyled fw-normal pb-1 small">
              <li>
                <a
                  href={Constants.megabox_event_giveaway}
                  className="link-body-emphasis d-inline-flex text-decoration-none rounded"
                >
                  경품 이벤트 현황
                </a>
              </li>
            </ul>
          </div>
        </li>
        <li className="border-top my-3"></li>
        <li className="mb-3">
          <button
            className="btn btn-toggle d-inline-flex align-items-center rounded border-0 collapsed"
            type="button"
            onClick={() => toggleCollapse('github')}
            aria-expanded={sections.github}
          >
            Github
          </button>
          <div className={sections.github ? "collapse show" : "collapse"} id="github-collapse">
            <ul className="btn-toggle-nav list-unstyled fw-normal pb-1 small">
              <li>
                <a
                  href="https://github.com/woorim02/CGVMate"
                  className="link-body-emphasis d-inline-flex text-decoration-none rounded"
                >
                  woorim02/CGVMate
                </a>
              </li>
            </ul>
          </div>
        </li>
      </ul>
      <div className="contact-wrap">
        <p className="contact">버그 제보/문의</p>
        <p className="contact">
          kakao: <a href="https://open.kakao.com/o/sSS6JJsg">오픈채팅</a>
        </p>
      </div>
    </div>
  );
};

export default Sidebar;
