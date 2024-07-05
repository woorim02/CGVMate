import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import './App.css';
import UserRoute from './components/UserRoute';
import AdminRoute from './components/AdminRoute';


function App() {
  const isAdminRoute = window.location.pathname.startsWith('/admin');
  return (
    <div className="App">
      {!isAdminRoute && <UserRoute/>}
      {isAdminRoute && <AdminRoute/>}
    </div>
  );
}

export default App;
