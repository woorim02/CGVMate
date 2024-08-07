import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Helmet } from 'react-helmet-async';
import { Box, Container, Typography, Card, CardMedia, CardContent, Grid, TextField, Button, FormControlLabel, Checkbox } from '@mui/material';
import CgvMateApi from 'api/cgvmateApi';
import MegaboxApi from 'api/megaboxApi';
import LotteApi from 'api/lotteApi';
import DisplayAds from 'components/DisplayAds';

const ListPage = () => {
  const cgvApi = new CgvMateApi();
  const megaboxApi = new MegaboxApi();
  const lotteApi = new LotteApi();
  const navigate = useNavigate();
  const [eventList, setEventList] = useState([]);
  const [filteredEventList, setFilteredEventList] = useState([]);
  const [showImage, setShowImage] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    (window.adsbygoogle = window.adsbygoogle || []).push({});
  }, []);

  useEffect(() => {
    async function fetchCuponEventList() {
      try {
        let eventList = [];
        let cgvEvents = await cgvApi.getCuponEventList();
        cgvEvents.forEach(e => {
          let event = {
            startDateTime: e.startDateTime,
            title: e.eventName.replace("선착순 무료 쿠폰", "서프라이즈 쿠폰"),
            imgSrc: e.imageSource,
            src: `https://m.cgv.co.kr/WebApp/EventNotiV4/EventDetailGeneralUnited.aspx?seq=${e.eventId}`,
            isToday: isToday(e.startDateTime),
            isPastEvent: isPastEvent(e.startDateTime),
            remainingTime: getRemainingTime(e.startDateTime)
          };
          eventList.push(event);
        });

        let megaboxCuponEvents = await megaboxApi.getCuponEventList();
        megaboxCuponEvents.forEach(e => {
          let event = {
            startDateTime: e.startDateTime,
            title: e.title,
            imgSrc: e.imageUrl,
            src: `https://m.megabox.co.kr/event/detail?eventNo=${e.eventNo}`,
            isToday: isToday(e.startDateTime),
            isPastEvent: isPastEvent(e.startDateTime),
            remainingTime: getRemainingTime(e.startDateTime)
          };
          eventList.push(event);
        });

        let lotteCuponEvents = await lotteApi.getCuponEventList();
        lotteCuponEvents.forEach(e => {
          let event = {
            startDateTime: e.startDateTime,
            title: e.title + " 무비싸다구",
            imgSrc: e.imageUrl,
            src: e.url,
            isToday: isToday(e.startDateTime),
            isPastEvent: isPastEvent(e.startDateTime),
            remainingTime: getRemainingTime(e.startDateTime)
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

  useEffect(() => {
    const intervalId = setInterval(() => {
      const updatedEventList = eventList.map(event => ({
        ...event,
        remainingTime: getRemainingTime(event.startDateTime)
      }));
      setEventList(updatedEventList);
      setFilteredEventList(updatedEventList.filter(event => event.title.toLowerCase().includes(searchTerm)));
    }, 1000);

    return () => clearInterval(intervalId);
  }, [eventList, searchTerm]);

  const formatDateTime = (dateStr) => {
    const date = new Date(dateStr);
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    const weekday = date.toLocaleString('ko-KR', { weekday: 'short' });
    const hours = date.getHours();
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0');
    const period = hours >= 12 ? '오후' : '오전';
    const formattedHours = hours % 12 || 12; // 12시간제로 변환

    return `${year}. ${month}. ${day}. (${weekday}) ${period} ${formattedHours}:${minutes}:${seconds}`;
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

  const isPastEvent = (date) => {
    const now = new Date();
    const eventDate = new Date(date);
    return eventDate < now;
  };

  const getRemainingTime = (startDateTime) => {
    const eventDate = new Date(startDateTime);
    const now = new Date();
    const difference = eventDate - now;

    if (difference <= 0) return "종료됨";

    const hours = Math.floor(difference / (1000 * 60 * 60));
    const minutes = Math.floor((difference % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((difference % (1000 * 60)) / 1000);

    return `${hours}시간 ${minutes}분 ${seconds}초`;
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
        <Typography variant="h4" component="h1" gutterBottom
          sx={{ display: "flex", alignItems: 'center', justifyContent: 'space-between', margin: 0 }}>
          프로모션 쿠폰 목록
          <FormControlLabel
            control={
              <Checkbox
                checked={showImage}
                onChange={() => setShowImage(!showImage)}
                color="primary"
              />
            }
            label="이미지 보기"
            sx={{ marginLeft: 2 }}
          />
        </Typography>
        <Grid container spacing={4} sx={{ marginTop: '10px' }}>
          <Grid item xs={12} sm={6} md={3}>
                <ins className="adsbygoogle"
                  style={{ display: 'inline-block', width: '300px', height: '100px' }}
                  data-ad-client="ca-pub-2422895337222657"
                  data-ad-slot="4893505812">
                </ins>
          </Grid>
          {filteredEventList.map((event, index) => (
            <Grid item key={index} xs={12} sm={6} md={3}>
              <Card
                onClick={() => window.location.href = event.src}
                sx={{
                  backgroundColor: !event.isPastEvent && event.isToday ? '#fbe9e7' : '#ffffff' // Change background color for today's events
                }}
              >
                <CardMedia
                  component={showImage ? 'img' : "div"}
                  height="140"
                  width="250"
                  image={event.imgSrc}
                  alt={event.title}
                />
                <CardContent>
                  <Typography variant="h6" component="div" sx={{ display: 'flex', flexDirection: 'row' }}>
                    {!event.isPastEvent &&
                      <Typography variant="h6" component="div" sx={{ color: 'red' }}>
                        (진행 예정)&nbsp;
                      </Typography>}
                    {event.title}
                  </Typography>
                  <Typography variant="body2" color="text.primary" fontWeight={'600'}>
                    {formatDateTime(event.startDateTime)}
                  </Typography>
                  <Typography
                    variant="h6"
                    component="div"
                    color={parseInt(event.remainingTime.split('시간')[0]) < 1 && event.remainingTime !== "0시간 0분 0초" ? 'red' : 'text.secondary'}
                  >
                    남은 시간: {event.remainingTime}
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
