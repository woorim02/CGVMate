import React, { useEffect, useState } from 'react';
import { Helmet } from 'react-helmet-async';
import { Box, Container, Typography, Card, CardMedia, CardContent, Grid, TextField, Button } from '@mui/material';
import CgvMateApi from 'api/cgvmateApi';
import MegaboxApi from 'api/megaboxApi';

const ListPage = () => {
  const cgvApi = new CgvMateApi();
  const megaboxApi = new MegaboxApi();
  const [eventList, setEventList] = useState([]);
  const [filteredEventList, setFilteredEventList] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    async function fetchCuponEventList() {
      try {
        let eventList = [];
        let cgvEvents = await cgvApi.getCuponEventList();
        cgvEvents.forEach(e => {
          let event = {
            startDateTime: e.startDateTime,
            title: e.eventName,
            imgSrc: e.imageSource,
            src: `https://m.cgv.co.kr/WebApp/EventNotiV4/EventDetailGeneralUnited.aspx?seq=${e.eventId}`
          };
          eventList.push(event);
        });

        let megaboxCuponEvents = await megaboxApi.getCuponEventList();
        megaboxCuponEvents.forEach(e => {
          let event = {
            startDateTime: e.startDateTime,
            title: e.title,
            imgSrc: e.imageUrl,
            src: `https://m.megabox.co.kr/event/detail?eventNo=${e.eventNo}`
          };
          eventList.push(event);
        });

        eventList.sort((a, b) => new Date(b.startDateTime) - new Date(a.startDateTime));
        setEventList(eventList);
        setFilteredEventList(eventList); // Initialize filtered list
      } catch (error) {
        console.error('Failed to fetch cupon event list:', error);
      }
    }
    fetchCuponEventList();
  }, []);

  const handleSearchChange = (event) => {
    const value = event.target.value.toLowerCase();
    setSearchTerm(value);
    filterEvents(value);
  };

  const filterEvents = (searchTerm) => {
    const filteredEvents = eventList.filter(event =>
      event.title.toLowerCase().includes(searchTerm)
    );
    setFilteredEventList(filteredEvents);
  };

  const isToday = (date) => {
    const today = new Date();
    const eventDate = new Date(date);
    return eventDate.getDate() === today.getDate() &&
      eventDate.getMonth() === today.getMonth() &&
      eventDate.getFullYear() === today.getFullYear();
  };

  return (
    <Box component="article" className="main_article">
      <Helmet>
        <title>프로모션 쿠폰 목록</title>
        <meta name="description" content={`진행중인 프로모션 쿠폰 리스트 확인하기`} />
        <meta property="og:type" content="website" />
        <meta property="og:title" content={`진행중인 프로모션 쿠폰 리스트 확인하기`} />
        <meta property="og:description" content={`진행중인 프로모션 쿠폰 리스트 확인하기`} />
        <meta property="og:url" content={window.location.href} />
      </Helmet>
      <Container maxWidth="100%">
        <Typography variant="h4" component="h1" gutterBottom>
          프로모션 쿠폰 목록
          <Typography variant="body2" color="text.secondary">
            (롯데시네마 추가 예정)
          </Typography>
        </Typography>
        <Box component="header" sx={{ marginBottom: 2, width: '100%' }}>
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
        <Grid container spacing={4}>
          {filteredEventList.map((event, index) => (
            <Grid item key={index} xs={12} sm={6} md={3}>
              <Card 
                onClick={() => window.open(event.src, '_blank')}
                sx={{
                  backgroundColor: isToday(event.startDateTime) ? '#fbe9e7' : '#ffffff' // Change background color for today's events
                }}
              >
                <CardMedia
                  component="img"
                  height="140"
                  width="250"
                  image={event.imgSrc}
                  alt={event.title}
                />
                <CardContent>
                  <Typography variant="h6" component="div">
                    {event.title}
                  </Typography>
                  <Typography variant="body2" color="text.primary" fontWeight={'600'}>
                    {new Date(event.startDateTime).toLocaleString()}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Container>
    </Box>
  );
};

export default ListPage;
