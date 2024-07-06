import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Constants from '../constants';
import LoginPage from '../pages/admin/login/LoginPage';
import DashboardPage from '../pages/admin/dashboard/DashboardPage';

const AdminRoute = () => {
  return (
    <>
      <main>
        <BrowserRouter>
          <Routes>
            <Route path={Constants.admin_login} element={<LoginPage />} />
            <Route path={Constants.admin_dashboard} element={<DashboardPage/>}/>
          </Routes>
        </BrowserRouter>
      </main>
    </>
  )
}

export default AdminRoute