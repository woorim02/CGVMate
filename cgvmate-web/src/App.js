import React from 'react';
import { BrowserRouter , Route, Routes } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import './App.css';
import Constants from './constants';
import Navbar from './components/Navbar/Navbar';
import Sidebar from './components/Sidebar/Sidebar';
import Offcanvas from './components/Offcanvas/Offcanvas';
import Home from './pages/Home';

import CgvEventPage from './pages/cgv/event/EventPage';
import CgvGiveawayPage from './pages/cgv/event/giveaway/GiveawayPage';
import CgvGiveawayDetailPage from './pages/cgv/event/giveaway/detail/GiveawayDetailPage';
import SpeedCuponPage from './pages/cgv/cupon/speed/SpeedCuponPage';
import SurpriseCuponPage from './pages/cgv/cupon/surprise/SurpriseCuponPage';

import LotteEventPage from './pages/lotte/event/EventPage';
import LotteGiveawayPage from './pages/lotte/event/giveaway/GiveawayPage';
import LotteGiveawayDetailPage from './pages/lotte/event/giveaway/detail/GiveawayDetailPage';

import MegaboxGiveawayPage from './pages/megabox/event/giveaway/GiveawayPage';
import MegaboxGiveawayDetailPage from './pages/megabox/event/giveaway/detail/GiveawayDetailPage';

import NotFound from './pages/NotFound';

function App() {
  return (
    <div className="App">
      <Offcanvas />
      <div className="navbar-container">
        <Navbar />
        <div className="cgv-gradient"></div>
      </div>
      <div className="body-container">
        <div className="sidebar-container shadow-sm">
          <Sidebar className="sidebar" isOpen={true} />
        </div>
        <main>
          <BrowserRouter>
            <Routes>
              <Route path="/" element={<Home />}></Route>
              <Route path={Constants.event_} element={<CgvEventPage />} />
              <Route path={Constants.event_giveaway} element={<CgvGiveawayPage />} />
              <Route path={Constants.event_giveaway_detail} element={<CgvGiveawayDetailPage />} />
              <Route path={Constants.event_cupon_speed} element={<SpeedCuponPage />} />
              <Route path={Constants.event_cupon_surprise} element={<SurpriseCuponPage />} />
              
              <Route path={Constants.lotte_event} element={<LotteEventPage />} />
              <Route path={Constants.lotte_event_giveaway} element={<LotteGiveawayPage />} />
              <Route path={Constants.lotte_event_giveaway_detail} element={<LotteGiveawayDetailPage />} />

              <Route path={Constants.megabox_event_giveaway} element={<MegaboxGiveawayPage />} />
              <Route path={Constants.megabox_event_giveaway_detail} element={<MegaboxGiveawayDetailPage />} />
              <Route path="*" element={<NotFound />}></Route>
            </Routes>
          </BrowserRouter>
        </main>
      </div>
    </div>
  );
}

export default App;
