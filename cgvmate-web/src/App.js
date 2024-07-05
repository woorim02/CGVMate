import React, { useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import './App.css';
import UserRoute from './components/UserRoute';
import AdminRoute from './components/AdminRoute';
import ReactGA from "react-ga4";


function App() {
  const isAdminRoute = window.location.pathname.startsWith('/admin');
  const location = window.location.pathname;

  useEffect(() => {
    ReactGA.send({ hitType: "pageview", page: location.pathname + location.search });
  }, [location]);
  return (
    <div className="App">
      {!isAdminRoute && <UserRoute/>}
      {isAdminRoute && <AdminRoute/>}
    </div>
  );
}

export default App;
