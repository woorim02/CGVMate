import React, { useState, useEffect, useRef } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Helmet } from 'react-helmet-async';
import { Box, Typography, Container, List, ListItem, ListItemText, CircularProgress, Grid, Divider } from '@mui/material';
import LotteMateApi from '../../../../../api/lotteApi';
import DisplayAds from '../../../../../components/DisplayAds';
import BannerAds from 'components/BannerAds';

const api = new LotteMateApi();

const GiveawayDetailPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const eventID = searchParams.get('eventIndex');
  const initialAreaCode = searchParams.get('areaCode') || '0001';
  const [currentArea, setCurrentArea] = useState(initialAreaCode);
  const [giftId, setGiftId] = useState(null);
  const [giftName, setGiftName] = useState(null);
  const [info, setInfo] = useState(null);

  const boxRef = useRef(null);

  useEffect(() => {
    const fetchEventData = async () => {
      if (!eventID) return;
      try {
        const eventModel = await api.getLotteGiveawayEventModelAsync(eventID);
        setGiftId(eventModel.frGiftID);
        setGiftName(eventModel.frGiftNm);
        const eventInfo = await api.getLotteGiveawayInfoAsync(eventID, eventModel.frGiftID);
        setInfo(eventInfo);
      } catch (error) {
        console.error('Error fetching event data:', error);
      }
    };

    fetchEventData();
  }, [eventID]);

  useEffect(() => {
    const fetchAreaData = async () => {
      if (!eventID || !giftId) return;
      try {
        const eventInfo = await api.getLotteGiveawayInfoAsync(eventID, giftId);
        setInfo(eventInfo);
      } catch (error) {
        console.error('Error fetching area data:', error);
      }
    };

    fetchAreaData();
  }, [eventID, giftId]);

  useEffect(() => {
    if (boxRef.current) {
      boxRef.current.scrollIntoView({ behavior: 'smooth' });
    }
  }, [currentArea]);

  const selectAreaTheaterList = (areaCode) => {
    setCurrentArea(areaCode);
    setSearchParams({ eventIndex: eventID, areaCode: areaCode }, { replace: true });
  };

  return (
    <Box sx={{ width: '100%', height: 'auto', padding: 0 }}>
      {giftName && (
        <Helmet>
          <title>{giftName}</title>
          <meta name="description" content={`${giftName} 수량 확인`} />
          <meta property="og:type" content="website" />
          <meta property="og:title" content={`${giftName} 수량 확인`} />
          <meta property="og:description" content={`${giftName} 수량 확인`} />
          <meta property="og:url" content={window.location.href} />
        </Helmet>
      )}
      <Box sx={{ display: 'block' }}>
        <Typography variant="h6" sx={{ padding: 1, borderBottom: '1px solid #f1f1f1' }}>잔여 수량 확인</Typography>
      </Box>

      <Container sx={{ padding: '0px !important' }}>
        <Box ref={boxRef} sx={{ padding: 1, borderBottom: '1px solid #f1f1f1', display: 'flex', alignItems: 'center' }}>
          <Typography variant="body1">{giftName}</Typography>
        </Box>
        <DisplayAds adSlot='8167919304' />
        <BannerAds adSlot='4357139561'/>
        <Grid container sx={{ height: '100%', flexWrap: 'nowrap' }}>
          {info ? (
            <>
              <Grid item xs={12} sm={4} md={3} sx={{ backgroundColor: '#f1f1f1', width: '25%', maxWidth: { xs: '120px', sm: 'none' } }}>
                <List>
                  {info.cinemaDivisions.map((item) => (
                    <ListItem
                      key={item.detailDivisionCode}
                      button
                      selected={item.detailDivisionCode === currentArea}
                      onClick={() => selectAreaTheaterList(item.detailDivisionCode)}
                      sx={{ padding: '17px 0 16px 17px' }}
                    >
                      <ListItemText primary={`${item.groupNameKR} (${item.cinemaCount})`} />
                    </ListItem>
                  ))}
                </List>
              </Grid>
              <Grid item xs={12} sm={8} md={9}>
                <List>
                  {info.cinemaDivisionGoods
                    .filter((item) => item.detailDivisionCode === currentArea)
                    .map((item, i) => (
                      <ListItem key={i} sx={{ display: 'flex', flexDirection: 'column', paddingTop: 1, paddingBottom: 1 }}>
                        <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                          <Typography variant="body1" sx={{ display: 'flex', alignContent: 'center' }}>{item.cinemaNameKR}</Typography>
                          <Box sx={{ display: 'flex', alignContent: 'center' }}>
                            <Typography color=' #ED4C6B'>
                              {item.cnt}
                            </Typography>

                            <Typography>
                              개 이상
                            </Typography>
                          </Box>
                        </Box>
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
