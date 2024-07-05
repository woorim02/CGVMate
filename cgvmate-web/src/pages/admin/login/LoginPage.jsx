import React, { useState, useEffect } from 'react';
import styles from './LoginPage.module.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import axios from 'axios';
import Constants from '../../../constants';

function LoginPage() {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(`${Constants.API_HOST}/admin/login`, { userName, password });
      const { token } = response.data;
      localStorage.setItem('token', token);
      // 로그인 성공 후 리다이렉트
      window.location.href = Constants.admin_dashboard;
    } catch (error) {
      alert('아이디 또는 비밀번호가 잘못되었습니다.');
    }
  };

  return (
    <div className={`d-flex align-items-center justify-content-center ${styles.loginContainer}`}>
      <div className={`card ${styles.loginCard}`}>
        <div className="card-body">
          <h2 className="card-title text-center">Login</h2>
          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label htmlFor="userName" className="form-label">username</label>
              <input
                type="userName"
                className={`form-control ${styles.input}`}
                id="userName"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <label htmlFor="password" className="form-label">password</label>
              <input
                type="password"
                className={`form-control ${styles.input}`}
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            <button type="submit" className={`cgv-button btn btn-primary w-100 ${styles.btnNoBorder}`}>Login</button>
          </form>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;
