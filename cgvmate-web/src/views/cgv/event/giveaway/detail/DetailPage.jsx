import React, { useState, useEffect, useRef } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { Helmet } from 'react-helmet-async';
import { Box, Typography, Container, List, ListItem, ListItemText, CircularProgress, Grid, Divider } from '@mui/material';
import CgvMateApi from 'api/cgvmateApi';
import DisplayAds from 'components/DisplayAds';
import BannerAds from 'components/BannerAds';

const api = new CgvMateApi();

const GiveawayDetailPage = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const eventIndex = searchParams.get('eventIndex');
  const areaCode = searchParams.get('areaCode') || '13';
  const [model, setModel] = useState(null);
  const [info, setInfo] = useState(null);
  const [currentArea, setCurrentArea] = useState(areaCode || '13');

  const boxRef = useRef(null);

  useEffect(() => {
    const fetchModelData = async () => {
      try {
        const modelResponse = await api.getGiveawayEventModelAsync(eventIndex);
        setModel(modelResponse);
      } catch (error) {
        console.error('Error fetching event data:', error);
      }
    };
    if (eventIndex) {
      fetchModelData();
    }
  }, [eventIndex]);

  useEffect(() => {
    const fetchInfoData = async () => {
      try {
        if (model) {
          const infoResponse = await api.getGiveawayInfoAsync(model.giveawayIndex, searchParams.get('areaCode'));
          setInfo(infoResponse);
        }
      } catch (error) {
        console.error('Error fetching event info:', error);
      }
    };
    if (model) {
      fetchInfoData();
    }
  }, [model]);

  useEffect(() => {
    if (boxRef.current) {
      boxRef.current.scrollIntoView({ behavior: 'smooth' });
    }
  }, [currentArea]);

  const selectAreaTheaterList = async (areaCode) => {
    try {
      const infoResponse = await api.getGiveawayInfoAsync(model.giveawayIndex, areaCode);
      setInfo(infoResponse);
      setCurrentArea(areaCode);
      navigate(`?eventIndex=${eventIndex}&areaCode=${areaCode}`, { replace: true });
    } catch (error) {
      console.error('Error selecting area theater list:', error);
    }
  };

  const countTypeCodeToText = (type) => {
    switch (type) {
      case "type4": return "마감 되었습니다.";
      case "type3": return "재고 소진 임박 입니다.";
      case "type2.5": return "재고 소진 중입니다.";
      case "type2": return "재고 보유 가능성이 높습니다.";
      default: return "unknown";
    }
  };

  const countTypeCodeToColor = (type) => {
    switch (type) {
      case "type4": return "#d3d3d3";
      case "type3": return "#fb4357";
      case "type2.5": return "#ffd966";
      case "type2": return "#25c326";
      default: return "unknown";
    }
  };

  return (
    <Box ref={boxRef} sx={{ width: '100%', height: 'auto', padding: 0 }}>
      {model && (
        <Helmet>
          <title>{model.contents}</title>
          <meta name="description" content={`${model.contents} 수량 확인`} />
          <meta property="og:type" content="website" />
          <meta property="og:title" content={`${model.contents} 수량 확인`} />
          <meta property="og:description" content={`${model.contents} 수량 확인`} />
          <meta property="og:url" content={window.location.href} />
        </Helmet>
      )}
      <Box sx={{ display: 'block' }}>
        <Typography variant="h6" sx={{ padding: 1, borderBottom: '1px solid #f1f1f1' }}>잔여 수량 확인</Typography>
      </Box>

      <Container sx={{ padding: '0px !important' }}>
        <Box sx={{ padding: 1, borderBottom: '1px solid #f1f1f1', display: 'flex', alignItems: 'center' }}>
          <Typography variant="body1">{model && model.title}</Typography>
        </Box>
        <DisplayAds adSlot='8167919304' />
        <BannerAds adSlot='4357139561'/>
        <Grid container sx={{ height: '100%', flexWrap: 'nowrap' }}>
          {info && info.AreaList ? (
            <>
              {info.AreaList.reduce((sum, x) => sum + parseInt(x.TheaterCount), 0) > 10 && (
                <Grid item xs={12} sm={4} md={3} sx={{ backgroundColor: '#f1f1f1', width: '25%', maxWidth: { xs: '120px', sm: 'none' } }}>
                  <List>
                    {info.AreaList.map((item) => (
                      <ListItem
                        key={item.AreaCode}
                        button
                        selected={item.AreaCode === currentArea}
                        onClick={() => selectAreaTheaterList(item.AreaCode)}
                        sx={{ padding: '17px 0 16px 17px' }}
                      >
                        <ListItemText primary={`${item.AreaName} (${item.TheaterCount})`} />
                      </ListItem>
                    ))}
                  </List>
                </Grid>
              )}
              <Grid item xs={12} sm={8} md={9}>
                <List>
                  {info.TheaterList.map((item, i) => (
                    <ListItem key={i} sx={{ display: 'flex', flexDirection: 'column', paddingTop: 0.5, paddingBottom: 0.5, alignItems: 'flex-start' }}>
                      <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                        <Typography variant="body1" sx={{ display: 'flex', alignItems: 'center' }}>{item.TheaterName}</Typography>
                        <Box
                          sx={{
                            display: 'inline-flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            width: '60px',
                            height: '24px',
                            fontSize: '1em',
                            backgroundColor: countTypeCodeToColor(item.CountTypeCode),
                            fontWeight: 500,
                            borderRadius: '30px',
                          }}
                        >
                          {item.GiveawayRemainCount}
                        </Box>
                      </Box>
                      <Typography variant="body2" color="textSecondary" sx={{ marginTop: 0 }}>
                        {countTypeCodeToText(item.CountTypeCode)}
                      </Typography>
                      <Divider sx={{ marginTop: 1 }} />
                    </ListItem>
                  ))}
                </List>
              </Grid>
            </>
          ) : (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
              <CircularProgress />
            </Box>
          )}
        </Grid>
      </Container>
    </Box>
  );
};

export default GiveawayDetailPage;
