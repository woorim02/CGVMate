import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Helmet } from 'react-helmet-async';
import { Box, Typography, Container, List, ListItem, ListItemText, CircularProgress, Grid, Divider } from '@mui/material';
import MegaboxMateApi from '../../../../../api/megaboxApi';
import DisplayAds from '../../../../../components/DisplayAds';

const api = new MegaboxMateApi();

const GiveawayDetailPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const eventID = searchParams.get('eventIndex');
  const initialAreaCode = searchParams.get('areaCode') || '10';
  const [currentArea, setCurrentArea] = useState(initialAreaCode);
  const [giftName, setGiftName] = useState(null);
  const [info, setInfo] = useState(null);

  useEffect(() => {
    const fetchEventData = async () => {
      if (!eventID) return;
      try {
        const eventInfo = await api.getGiveawayEventDetailAsync(eventID);
        document.title = eventInfo.title;
        setGiftName(eventInfo.title);
        setInfo(eventInfo);
      } catch (error) {
        console.error('Error fetching event data:', error);
      }
    };

    fetchEventData();
  }, [eventID]);

  const selectAreaTheaterList = (areaCode) => {
    setCurrentArea(areaCode);
    setSearchParams({ eventIndex: eventID, areaCode: areaCode }, { replace: true });
  };

  const getStatusColor = (status) => {
    switch (status) {
      case '보유':
        return '#503396';
      case '소량보유':
        return '#FF5733';
      default:
        return 'inherit';
    }
  };
  const decodeHtml = (html) => {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
  };
  const currentAreaInfo = info?.areas?.find((area) => area.code === currentArea);

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
        <Box sx={{ padding: 1, borderBottom: '1px solid #f1f1f1', display: 'flex', alignItems: 'center' }}>
          <Typography variant="body1">{giftName}</Typography>
        </Box>
        <DisplayAds adSlot='8167919304' />
        <Grid container sx={{ height: '100%', flexWrap: 'nowrap' }}>
          {info ? (
            <>
              <Grid item xs={12} sm={4} md={3} sx={{ backgroundColor: '#f1f1f1', width: '25%', maxWidth: { xs: '120px', sm: 'none' } }}>
                <List>
                  {info.areas?.map((item) => (
                    <ListItem
                      key={item.code}
                      button
                      selected={item.code === currentArea}
                      onClick={() => selectAreaTheaterList(item.code)}
                      sx={{ padding: '17px 0 16px 17px' }}
                    >
                      <ListItemText primary={item.name} />
                    </ListItem>
                  ))}
                </List>
              </Grid>
              <Grid item xs={12} sm={8} md={9}>
                <List>
                  {currentAreaInfo?.infos?.map((item, i) => (
                    <ListItem key={i} sx={{ display: 'flex', flexDirection: 'column', paddingTop: 1, paddingBottom: 1 }}>
                      <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                        <Typography variant="body1" sx={{ display: 'flex', alignItems: 'center' }}>
                          <span dangerouslySetInnerHTML={{ __html: item.title }} />
                        </Typography>
                        <Box sx={{ display: 'flex', alignItems: 'center', minWidth: '60px'}}>
                          <Typography sx={{ color: getStatusColor(item.fAc), fontWeight: '600 !important'  }}>
                            {item.fAc}
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
