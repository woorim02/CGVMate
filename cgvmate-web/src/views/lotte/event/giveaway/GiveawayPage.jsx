import React, { useState, useEffect, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { Box, Typography, TextField, Button, Container, List, ListItem, ListItemText, ListItemSecondaryAction, CircularProgress } from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import LotteApi from 'api/lotteApi';
import DisplayAds from 'components/DisplayAds';

const GiveawayPage = () => {
  const [eventList, setEventList] = useState([]);
  const [filteredEventList, setFilteredEventList] = useState([]);
  const [loading, setLoading] = useState(true);
  const api = useRef(new LotteApi()).current;

  useEffect(() => {
    document.title = '롯데시네마 경품 이벤트 현황';
  }, []);

  useEffect(() => {
    const fetchEvents = async () => {
      const events = await api.getLotteGiveawayEventListAsync();
      setEventList(events);
      setFilteredEventList(events);
      setLoading(false);
    };
    fetchEvents();
  }, [api]);

  const handleSearchChange = (event) => {
    const searchText = event.target.value.toLowerCase().trim();
    if (!searchText) {
      setFilteredEventList(eventList);
    } else {
      const filtered = eventList.filter(event => event.title.toLowerCase().includes(searchText));
      setFilteredEventList(filtered);
    }
  };
  const convertToDDay = (dateString) => {
    const targetDate = new Date(dateString);
    const currentDate = new Date().setHours(0, 0, 0, 0);
    const difference = targetDate - currentDate;
    const daysUntil = Math.floor(difference / (1000 * 60 * 60 * 24));

    if (daysUntil > 0) {
      return `D-${daysUntil}`;
    } else if (daysUntil < 0) {
      return `D+${-daysUntil}`;
    } else {
      return 'D-Day';
    }
  };
  return (
    <Box sx={{ padding: 0 }}>
      <Helmet>
        <meta name="description" content={`롯데시네마 경품 특전 수량 확인하기, 공식 사이트에서 공개되지 않는 특전 수량 확인이 가능합니다.`} />
        <meta property="og:type" content="website" />
        <meta property="og:title" content={`롯데시네마 경품 특전 수량 확인하기, 공식 사이트에서 공개되지 않는 특전 수량 확인이 가능합니다.`} />
        <meta property="og:description" content={`롯데시네마 경품 특전 수량 확인하기, 공식 사이트에서 공개되지 않는 특전 수량 확인이 가능합니다.`} />
        <meta property="og:url" content={window.location.href} />
      </Helmet>
      <Box sx={{ marginBottom: 2 }}>
        <Typography variant="h6" sx={{fontSize: '0.95rem'}}>
          진행중인 경품 이벤트&nbsp;
          <strong>{filteredEventList.length}</strong>개
        </Typography>
        <Box sx={{ display: 'flex', marginTop: 2 }}>
          <TextField
            variant="outlined"
            onChange={handleSearchChange}
            placeholder="이벤트 검색"
            sx={{ flexGrow: 1 }}
            inputProps={{ style: { height: '30px', padding: '0 14px' } }}
          />
          <Button variant="contained" color="primary" sx={{ marginLeft: 2, height: '30px' }}>
            검색
          </Button>
        </Box>
      </Box>
      <Container maxWidth={false} sx={{ padding: 0 }}>
        {loading ? (
          <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
            <CircularProgress />
          </Box>
        ) : (
          <List sx={{ padding: 0, margin: 0 }}>
            <DisplayAds adSlot='3730871491' />
            {filteredEventList.length > 0 ? (
              filteredEventList.map((item, index) => (
                <React.Fragment key={item.eventID}>
                  {index === Math.floor(filteredEventList.length / 2) && <DisplayAds adSlot='1843074752' />}
                  <ListItem button component="a" href={`/lotte/event/giveaway/detail?eventIndex=${item.eventID}`}>
                    <ListItemText
                      primary={
                        <Typography variant="h6" sx={{fontSize: '0.95rem'}}>
                          {item.eventName}
                        </Typography>
                      }
                      secondary={
                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                          <Typography variant="body2" color="textSecondary" sx={{ marginRight: 1 }}>
                          {item.progressStartDate} ~ {item.progressEndDate}
                          </Typography>
                          <VisibilityIcon fontSize="small" />
                          <Typography variant="body2" color="textSecondary" sx={{ marginLeft: 0.5 }}>
                            {item.views}
                          </Typography>
                        </Box>
                      }
                    />
                    <ListItemSecondaryAction>
                      <Box sx={{
                        display: 'inline-flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        minWidth: '60px',
                        minHeight: '24px',
                        fontSize: '0.929em',
                        color: '#fb4357',
                        border: '1px solid #fb4357',
                        borderRadius: '5px'
                      }}>
                        {convertToDDay(item.progressEndDate)}
                      </Box>
                    </ListItemSecondaryAction>
                  </ListItem>
                </React.Fragment>
              ))
            ) : (
              <Typography>이벤트가 없습니다.</Typography>
            )}
          </List>
        )}
      </Container>
    </Box>
  );
};

export default GiveawayPage;
