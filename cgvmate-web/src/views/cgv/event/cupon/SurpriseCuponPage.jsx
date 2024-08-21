import React, { useState, useEffect, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { Box, FormControl, InputLabel, Select, MenuItem, Switch, Typography, List, ListItem, ListItemText, Container, FormControlLabel } from '@mui/material';
import CgvMateApi from '../../../../api/cgvmateApi';
import DisplayAds from '../../../../components/DisplayAds';

const api = new CgvMateApi();

const SurpriseCuponPage = () => {
  const [cupons, setCupons] = useState([]);
  const [processTexts, setProcessTexts] = useState([]);
  const [isRunning, setIsRunning] = useState(false);
  const [selectedCuponIndex, setSelectedCuponIndex] = useState('');
  const intervalRef = useRef(null);

  useEffect(() => {
    document.title = '서프라이즈 쿠폰 조회';
  }, []);

  useEffect(() => {
    const fetchCupons = async () => {
      const cuponEvents = await api.getEventListAsync(4);
      const filteredCupons = cuponEvents
        .filter(event => event.eventName.includes('선착순 무료 쿠폰'))
        .map(event => ({
          index: event.eventId,
          title: event.eventName,
          count: 0,
          isAvailable: false
        }));
      setCupons(filteredCupons);
      if (filteredCupons.length > 0) {
        setSelectedCuponIndex(filteredCupons[0].index);
      }
    };
    fetchCupons();

    return () => {
      setIsRunning(false);
      if (intervalRef.current) {
        clearInterval(intervalRef.current);
      }
    };
  }, []);

  useEffect(() => {
    if (isRunning) {
      intervalRef.current = setInterval(async () => {
        const cupon = await api.getSurpriseCuponCountAsync(selectedCuponIndex);
        const timeString = `${new Date().toLocaleTimeString()}.${new Date().getMilliseconds()}`;
        const countText = cupon.isAvailable
          ? `<span class='count' style='color:lawngreen;'>${cupon.count}</span>`
          : `<span class='count'>${cupon.count}</span>`;
        setProcessTexts(prevTexts => [
          `[${timeString}] [${cupon.title}] : ${countText}`,
          ...prevTexts
        ]);
      }, 500);

      return () => clearInterval(intervalRef.current);
    }
  }, [isRunning, selectedCuponIndex]);

  const handleToggleChange = () => {
    setIsRunning(!isRunning);
  };

  const handleCuponIndexChange = (event) => {
    setSelectedCuponIndex(event.target.value);
  };

  return (
    <Container>
      서버 부하로 인해 일시적으로 사용 중단
    </Container>
  );
};

export default SurpriseCuponPage;
