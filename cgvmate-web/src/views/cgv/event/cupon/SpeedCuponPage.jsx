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
      <Helmet>
        <meta name="description" content="스피드쿠폰 자동 확인, 스피드쿠폰 매크로" />
        <meta property="og:type" content="website" />
        <meta property="og:title" content="스피드쿠폰 자동 확인" />
        <meta property="og:description" content="스피드쿠폰 자동 확인, 스피드쿠폰 매크로" />
        <meta property="og:url" content={window.location.href} />
      </Helmet>
      <Box sx={{ mb: 2 }}>
        <Typography variant="h5">스피드쿠폰 자동 확인</Typography>
      </Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
        <FormControlLabel
          control={
            <Switch
              checked={isRunning}
              onChange={handleToggleChange}
              color="primary"
            />
          }
          label="스피드쿠폰 자동 확인"
        />
        <FormControl variant="outlined" sx={{ minWidth: 150 }}>
          <InputLabel id="movie-select-label">영화 선택</InputLabel>
          <Select
            labelId="movie-select-label"
            value={selectedMovieIndex}
            onChange={handleMovieIndexChange}
            label="영화 선택"
          >
            <MenuItem value="nullmoviewqwegv">All</MenuItem>
            {cupons.map((cupon, index) => (
              <MenuItem key={index} value={cupon.movieIndex}>{cupon.movieTitle}</MenuItem>
            ))}
          </Select>
        </FormControl>
      </Box>
      <DisplayAds adSlot='2485625472' />
      <List>
        {processTexts.map((text, index) => (
          <ListItem key={index}>
            <ListItemText primary={<span dangerouslySetInnerHTML={{ __html: text }} />} />
          </ListItem>
        ))}
      </List>
    </Container>
  );
};

export default SpeedCuponPage;

