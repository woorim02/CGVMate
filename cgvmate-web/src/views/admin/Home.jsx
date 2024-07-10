import React from 'react';
import { Box, Container, Typography } from '@mui/material';

const MyComponent = () => {
  return (
    <Box component="article" className="main_article" sx={{ display: 'flex', flexDirection: 'column', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
      <Container maxWidth="lg">
        <Typography variant="h2" component="h1" gutterBottom align="center">
          CGV 도우미
        </Typography>
        <Typography variant="h4" component="h2" gutterBottom align="center" color='secondary.800'>
          관리자 페이지
        </Typography>
      </Container>
    </Box>
  );
};

export default MyComponent;
