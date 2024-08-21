import React, { useState, useEffect, useRef } from 'react';
import { Helmet } from "react-helmet-async";
import { Box, FormControl, InputLabel, Select, MenuItem, Switch, Typography, List, ListItem, ListItemText, Container, FormControlLabel } from '@mui/material';
import CgvMateApi from '../../../../api/cgvmateApi';
import DisplayAds from '../../../../components/DisplayAds';

const api = new CgvMateApi();

const SpeedCuponPage = () => {
  const [cupons, setCupons] = useState([]);
  const [processTexts, setProcessTexts] = useState([]);
  const [isRunning, setIsRunning] = useState(false);
  const [selectedMovieIndex, setSelectedMovieIndex] = useState('nullmoviewqwegv');
  const intervalRef = useRef(null);

  useEffect(() => {
    document.title = '스피드쿠폰 조회';
  }, []);

  useEffect(() => {
    const fetchCupons = async () => {
      const response = await api.getSpeedCuponCountsAsync();
      setCupons(response);
    };
    fetchCupons();

    return () => {
      setIsRunning(false);
    };
  }, []);

  useEffect(() => {
    if (isRunning) {
      intervalRef.current = setInterval(async () => {
        const response = await api.getSpeedCuponCountsAsync();
        const selectedCupons = selectedMovieIndex === 'nullmoviewqwegv'
          ? response
          : response.filter(x => x.movieIndex === selectedMovieIndex);
        const newProcessTexts = selectedCupons.map(cupon => {
          const now = new Date();
          const timeString = `${now.toLocaleTimeString()}.${now.getMilliseconds()}`;
          const countText = cupon.count % 100 !== 0
            ? `<span class='count' style='color:lawngreen;'>${cupon.count}</span>`
            : `<span class='count'>${cupon.count}</span>`;
          return `[${timeString}] [${cupon.movieTitle}] : ${countText}`;
        });
        setProcessTexts(newProcessTexts);
      }, 500);

      return () => clearInterval(intervalRef.current);
    }
  }, [isRunning, selectedMovieIndex]);

  const handleToggleChange = () => {
    setIsRunning(!isRunning);
  };

  const handleMovieIndexChange = (event) => {
    setSelectedMovieIndex(event.target.value);
  };

  return (
    <Container>
      서버 부하로 인해 일시적으로 사용 중단
    </Container>
  );
};

export default SpeedCuponPage;

