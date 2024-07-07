import React, { useState, useEffect, useRef } from 'react';
import { Box, Tabs, Tab, TextField, Button, Card, CardContent, CardMedia, Typography, Container, Grid } from '@mui/material';
import CgvMateApi from '../../../api/cgvmateApi';

const EventPage = () => {
  const [events, setEvents] = useState([]);
  const [filteredEvents, setFilteredEvents] = useState([]);
  const [eventType, setEventType] = useState('Special');
  const api = useRef(new CgvMateApi()).current;

  useEffect(() => {
    document.title = 'CGV 이벤트 목록';
  }, []);

  useEffect(() => {
    const fetchEvents = async () => {
      const eventTypeKey = CgvEventType[eventType];
      const eventData = await api.getEventListAsync(eventTypeKey);
      const processedData = eventData.map(item => {
        if (item.eventName.includes("선착순 무료 쿠폰")) {
          item.eventName = item.eventName.replace("선착순 무료 쿠폰", "서프라이즈 쿠폰");
        }
        return item;
      });
      setEvents(processedData);
      setFilteredEvents(processedData);
    };
    fetchEvents();
  }, [eventType]);

  const toggleNavItem = (type) => {
    setEventType(type);
  };

  const handleSearchChange = (event) => {
    const searchText = event.target.value.toLowerCase().trim();
    if (!searchText) {
      setFilteredEvents(events);
    } else {
      const filtered = events.filter(event => event.eventName.toLowerCase().trim().includes(searchText));
      setFilteredEvents(filtered);
    }
  };

  const handleCardClick = (event) => {
    window.location.href = `https://m.cgv.co.kr/WebApp/EventNotiV4/EventDetailGeneralUnited.aspx?seq=${event.eventId}`;
  };

  return (
    <Box sx={{ padding: 2 }}>
      <Box component="header" sx={{ marginBottom: 2 }}>
        <Tabs value={eventType} onChange={(e, newValue) => toggleNavItem(newValue)} aria-label="event types" variant='scrollable'>
          {Object.keys(CgvEventType).map((type) => (
            <Tab key={type} label={type} value={type} />
          ))}
        </Tabs>
        <Box sx={{ display: 'flex', justifyContent: 'center', marginTop: 2, alignItems: 'center' }}>
          <TextField
            variant="outlined"
            onChange={handleSearchChange}
            placeholder="이벤트 검색"
            sx={{ width: '80%' }}
            inputProps={{ style: { height: 30, padding: '0 14px' } }}
          />
          <Button variant="contained" color="primary" sx={{ marginLeft: 2, height: 30 }}>검색</Button>
        </Box>
      </Box>
      <Container>
        <Grid container spacing={2}>
          {filteredEvents.length > 0 ? filteredEvents.map(event => (
            <Grid item xs={12} sm={6} md={3} key={event.eventId}>
              <Card onClick={() => handleCardClick(event)} sx={{ cursor: 'pointer', display: 'flex', flexDirection: 'column', height: '100%' }}>
                <CardMedia
                  component="img"
                  image={event.imageSource}
                  alt={event.eventName}
                  sx={{ width: '100%', height: 'auto' }}
                />
                <CardContent sx={{ flexGrow: 1 }}>
                  <Typography variant="h6" component="div" dangerouslySetInnerHTML={{ __html: event.eventName }} />
                  {event.period !== "상시진행" ? (
                    <Typography variant="body2" color="textSecondary">
                      {`${new Date(event.startDate).toLocaleDateString()} ~ ${new Date(event.endDate).toLocaleDateString()}`}
                    </Typography>
                  ) : (
                    <Typography variant="body2" color="textSecondary">상시진행</Typography>
                  )}
                </CardContent>
              </Card>
            </Grid>
          )) : (
            <Typography variant="body1" sx={{ marginTop: 2 }}>이벤트가 없습니다.</Typography>
          )}
        </Grid>
      </Container>
    </Box>
  );
};

const CgvEventType = {
  Special: '001',
  영화: '004',
  극장: '005',
  제휴: '006',
  멤버십_클럽: '008',
  지난이벤트: '100'
};

export default EventPage;
