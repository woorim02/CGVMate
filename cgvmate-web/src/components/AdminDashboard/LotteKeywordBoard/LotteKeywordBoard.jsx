import React, { useEffect, useState } from 'react';
import axios from 'axios';
import styles from './LotteKeywordBoard.module.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import Constants from '../../../constants';

const LotteKeywordBoard = () => {
  const [keywords, setKeywords] = useState([]);
  const [newKeyword, setNewKeyword] = useState('');

  useEffect(() => {
    const fetchKeywords = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        window.location.href = '/admin/login';
        return;
      }
      try {
        const response = await axios.get(`${Constants.API_HOST}/admin/lotte/event/giveaway/keywords`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setKeywords(response.data);
      } catch (error) {
        console.error('Error fetching keywords:', error);
      }
    };

    fetchKeywords();
  }, []);

  const addKeyword = async () => {
    const token = localStorage.getItem('token');
    try {
      const response = await axios.post(`${Constants.API_HOST}/admin/lotte/event/giveaway/keywords`,
        { 
          method : 'add',
          keyword: newKeyword 
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
      setKeywords([...keywords, response.data]);
      setNewKeyword('');
    } catch (error) {
      console.error('Error adding keyword:', error);
    }
  };

  const deleteKeyword = async (keyword) => {
    const token = localStorage.getItem('token');
    try {
      await axios.post(`${Constants.API_HOST}/admin/lotte/event/giveaway/keywords`,
        { 
          method : 'delete',
          keyword: keyword
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
      setKeywords(keywords.filter((k) => k !== keyword));
    } catch (error) {
      console.error('Error deleting keyword:', error);
    }
  };

  return (
    <div className={`${styles.dashboardPage}`}>
      <h1>Keyword Management</h1>
      <div className={`input-group mb-3 mt-3 ${styles.inputGroup}`}>
        <input
          type="text"
          className="form-control"
          placeholder="Add a new keyword"
          value={newKeyword}
          onChange={(e) => setNewKeyword(e.target.value)}
        />
        <button className="btn btn-primary" onClick={addKeyword}>
          Add
        </button>
      </div>
      <ul className="list-group">
        {keywords.map((keyword) => (
          <li className="list-group-item d-flex justify-content-between align-items-center" key={keyword}>
            {keyword}
            <button className="btn btn-danger btn-sm" onClick={() => deleteKeyword(keyword)}>
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default LotteKeywordBoard;
