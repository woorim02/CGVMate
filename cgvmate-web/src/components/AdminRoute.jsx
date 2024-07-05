import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Constants from '../constants';
import LoginPage from '../pages/admin/login/LoginPage';

const AdminRoute = () => {
  return (
    <>
      <main>
        <BrowserRouter>
          <Routes>
            <Route path={Constants.admin_login} element={<LoginPage />} />
          </Routes>
        </BrowserRouter>
      </main>
    </>
  )
}

export default AdminRoute