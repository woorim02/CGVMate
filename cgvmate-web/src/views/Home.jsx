import React, { useEffect } from 'react';
import { Helmet } from 'react-helmet-async';
import { Box, Container, Typography, Card, CardContent, Grid, Button } from '@mui/material';

const Home = () => {
  
  useEffect(() => {
    document.title = 'CGV 도우미 | 홈';
  }, []);

  const handleCardClick = (url) => {
    window.location.href = url;
  };

  return (
    <Box component="article" className="main_article">
      <Helmet>
        <meta name="description" content={`CGV 도우미는 CGV, 롯데시네마 등 다양한 멀티플렉스 영화관의 이벤트와 경품 관련 정보를 손쉽게 확인할 수 있는 안내 사이트입니다. 공식 웹사이트에서 지원하지 않는 다양한 기능들을 제공합니다. :)`} />
        <meta property="og:type" content="website" />
        <meta property="og:title" content={`CGV 도우미는 CGV, 롯데시네마 등 다양한 멀티플렉스 영화관의 이벤트와 경품 관련 정보를 손쉽게 확인할 수 있는 안내 사이트입니다. 공식 웹사이트에서 지원하지 않는 다양한 기능들을 제공합니다. :)`} />
        <meta property="og:description" content={`CGV 도우미는 CGV, 롯데시네마 등 다양한 멀티플렉스 영화관의 이벤트와 경품 관련 정보를 손쉽게 확인할 수 있는 안내 사이트입니다. 공식 웹사이트에서 지원하지 않는 다양한 기능들을 제공합니다. :)`} />
        <meta property="og:url" content={window.location.href} />
      </Helmet>
      <Container maxWidth="lg">
        <Typography variant="h4" component="h1" gutterBottom align="center">
        CGV 도우미
        </Typography>
        <Typography variant="h6" component="h2" gutterBottom align="center">
          CGV 도우미는 다양한 이벤트와 경품 관련 정보를<br /> 손쉽게 확인할 수 있는 안내 사이트입니다.
        </Typography>
        <Typography variant="h6" component="h2" gutterBottom align="center">
            공식 웹사이트에서 지원하지 않는 다양한 기능들을 제공합니다. :)
          </Typography>
        <Grid container spacing={4} justifyContent="center">
          <Grid item xs={12} sm={6} md={4}>
            <Card sx={{ height: '100%', cursor: 'pointer' }} onClick={() => handleCardClick('/promo/list')}>
              <CardContent>
                <Typography variant="h5" component="div">
                  프로모션 쿠폰 목록 조회
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <Card sx={{ height: '100%', cursor: 'pointer' }} onClick={() => handleCardClick('/cgv/event/giveaway')}>
              <CardContent>
                <Typography variant="h5" component="div">
                CGV 경품 이벤트 현황
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <Card sx={{ height: '100%', cursor: 'pointer' }} onClick={() => handleCardClick('/cgv/event/cupon/speed')}>
              <CardContent>
                <Typography variant="h5" component="div">
                CGV 스피드 쿠폰 조회
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <Card sx={{ height: '100%', cursor: 'pointer' }} onClick={() => handleCardClick('/cgv/event/cupon/surprise')}>
              <CardContent>
                <Typography variant="h5" component="div">
                CGV 서프라이즈 쿠폰 조회
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <Card sx={{ height: '100%', cursor: 'pointer' }} onClick={() => handleCardClick('/lotte/event/giveaway')}>
              <CardContent>
                <Typography variant="h5" component="div">
                  롯데시네마 경품 이벤트 현황
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <Card sx={{ height: '100%', cursor: 'pointer' }} onClick={() => handleCardClick('/megabox/event/giveaway')}>
              <CardContent>
                <Typography variant="h5" component="div">
                  메가박스 경품 이벤트 현황
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <Card sx={{ height: '100%', cursor: 'pointer' }} onClick={() => handleCardClick('https://github.com/woorim02/CGVMate')}>
              <CardContent>
                <Typography variant="h5" component="div">
                  Contact
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  궁금한 사항이 있으시면 연락주세요.
                </Typography>
                <Button variant="contained" color="primary" sx={{ mt: 2 }} href="https://github.com/woorim02/CGVMate">
                  Github
                </Button>
                &nbsp;
                &nbsp;
                &nbsp;
                <Button variant="contained" color="primary" sx={{ mt: 2 }} href="https://open.kakao.com/o/sSS6JJsg">
                  Kakao
                </Button>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      </Container>
    </Box>
  );
};

export default Home;
