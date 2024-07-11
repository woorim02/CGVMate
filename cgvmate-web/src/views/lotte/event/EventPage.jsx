import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import LotteMateApi from "api/lotteApi";
import { Box, Tabs, Tab, TextField, Button, Container, Grid, Card, CardMedia, CardContent, Typography } from '@mui/material';

const api = new LotteMateApi();

const EventPage = () => {
  const [events, setEvents] = useState([]);
  const [filteredEvents, setFilteredEvents] = useState([]);
  const [tabValue, setTabValue] = useState(0);
  const [isMobile, setIsMobile] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    setIsMobile(isMobileDevice());
  }, []);

  useEffect(() => {
    document.title = '롯데시네마 이벤트 목록';
  }, []);

  useEffect(() => {
    const fetchEvents = async () => {
      const data = await api.getLotteEventListAsync(20); // 초기 이벤트 타입을 영화로 설정
      setEvents(data);
      setFilteredEvents(data);
    };
    fetchEvents();
  }, []);

  const handleTabChange = async (event, newValue) => {
    setTabValue(newValue);
    const typeMap = [20, 40, 10, 30, 50];
    const data = await api.getLotteEventListAsync(typeMap[newValue]);
    setEvents(data);
    setFilteredEvents(data);
  };

  const searchBarOnChange = (event) => {
    const text = event.target.value.toLowerCase().trim();
    if (!text) {
      setFilteredEvents(events);
    } else {
      setFilteredEvents(events.filter(e => e.eventName.toLowerCase().includes(text)));
    }
  };

  const cardOnClick = (eventId) => {
    window.location.href = `https://event.lottecinema.co.kr/${isMobile ? 'NLCMW' : 'NLCHS'}/Event/EventTemplateInfo?eventId=${eventId}`;
  };

  const isMobileDevice = () => {
    const userAgent = navigator.userAgent || navigator.vendor || window.opera;
    return /android|iPhone|iPad|iPod|blackberry|iemobile|opera mini|windows phone/i.test(userAgent);
  };

  return (
    <Box sx={{ padding: 0 }}>
      <Box component="header" sx={{ marginBottom: 2 }}>
        <Tabs value={tabValue} onChange={handleTabChange} variant="scrollable" scrollButtons="auto" aria-label="event tabs">
          <Tab label="영화" />
          <Tab label="시사회/무대인사" />
          <Tab label="HOT" />
          <Tab label="극장" />
          <Tab label="제휴" />
        </Tabs>

        <Box sx={{ display: 'flex', justifyContent: 'center', marginTop: 2, alignItems: 'center' }}>
          <TextField
            variant="outlined"
            onChange={searchBarOnChange}
            placeholder="이벤트 검색"
            sx={{ width: '80%' }}
            inputProps={{ style: { height: 30, padding: '0 14px' } }}
          />
          <Button variant="contained" color="primary" sx={{ marginLeft: 2, height: 30 }}>검색</Button>
        </Box>
      </Box>
      <Container sx={{ maxWidth: '100%', height: 'calc(100% - 4.5rem)', overflowY: 'auto', mt: 2 }}>
        <Grid container spacing={2}>
          {filteredEvents.length > 0 ? (
            filteredEvents.map(item => (
              <Grid item key={item.eventID} xs={12} sm={6} md={4} lg={3}>
                <Card onClick={() => cardOnClick(item.eventID)} sx={{ cursor: 'pointer', maxHeight: '300px' }}>

                  <CardMedia
                    component="img"
                    image={item.imageUrl}
                    alt={item.eventName}
                    sx={{ width: '100%', height: 'auto' }}
                  />
                  <CardContent>
                    <Typography variant="h6" component="div" dangerouslySetInnerHTML={{ __html: item.eventName }} />
                    <Typography variant="body2" color="textSecondary">
                      {`${item.progressStartDate} ~ ${item.progressEndDate}`}
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
            ))
          ) : (
            <Typography>이벤트가 없습니다.</Typography>
          )}
        </Grid>
      </Container>
    </Box>
  );
};

export default EventPage;
