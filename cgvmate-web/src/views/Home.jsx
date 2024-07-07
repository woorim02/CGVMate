import React, { useEffect } from 'react';
import { Helmet } from 'react-helmet-async';
import { Box, Container, Typography } from '@mui/material';

const Home = () => {
  
  useEffect(() => {
    document.title = 'CGV 도우미 | 홈';
  }, []);

  return (
    <Box component="article" className="main_article">
      <Helmet>
        <meta name="description" content={`CGV 도우미는 CGV, 롯데시네마 등 다양한 멀티플렉스 영화관의 이벤트와 경품 관련 정보를 손쉽게 확인할 수 있는 안내 사이트입니다. 공식 웹사이트에서 지원하지 않는 다양한 기능들을 제공합니다. :)`} />
        <meta property="og:type" content="website" />
        <meta property="og:title" content={`CGV 도우미는 CGV, 롯데시네마 등 다양한 멀티플렉스 영화관의 이벤트와 경품 관련 정보를 손쉽게 확인할 수 있는 안내 사이트입니다. 공식 웹사이트에서 지원하지 않는 다양한 기능들을 제공합니다. :)`} />
        <meta property="og:description" content={`CGV 도우미는 CGV, 롯데시네마 등 다양한 멀티플렉스 영화관의 이벤트와 경품 관련 정보를 손쉽게 확인할 수 있는 안내 사이트입니다. 공식 웹사이트에서 지원하지 않는 다양한 기능들을 제공합니다. :)`} />
        <meta property="og:url" content={window.location.href} />
      </Helmet>
      <Container sx={{ textAlign: 'center', my: 4 }}>
        <Typography variant="h2" component="h2" fontWeight="bold">
          CGV 도우미
        </Typography>
        <Box sx={{ mx: 'auto', my: 4, maxWidth: 'lg' }}>
          <Typography variant="body1" gutterBottom>
            CGV 도우미는 다양한 이벤트와 경품 관련 정보를<br /> 손쉽게 확인할 수 있는 안내 사이트입니다.
          </Typography>
          <Box sx={{ height: 5 }}></Box>
          <Typography variant="body1" gutterBottom>
            공식 웹사이트에서 지원하지 않는 다양한 기능들을 제공합니다. :)
          </Typography>
        </Box>
      </Container>
    </Box>
  );
};

export default Home;
