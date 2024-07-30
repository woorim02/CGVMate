import React, { useState, useEffect, useRef } from 'react';
import { Helmet } from 'react-helmet-async';
import { Box, Typography, TextField, Button, Container, List, ListItem, ListItemText, CircularProgress } from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { Link, useLocation } from 'react-router-dom';
import MegaboxApi from 'api/megaboxApi';
import DisplayAds from 'components/DisplayAds';

const GiveawayPage = () => {
  const [eventList, setEventList] = useState([]);
  const [filteredEventList, setFilteredEventList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchText, setSearchText] = useState('');
  const api = useRef(new MegaboxApi()).current;
  const location = useLocation();

  useEffect(() => {
    document.title = '메가박스 경품 이벤트 현황';
  }, []);

  useEffect(() => {
    const fetchEvents = async () => {
      let events = await api.getGiveawayEventListAsync();
      events = events.reverse();
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

  const decodeHtml = (html) => {
    var txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
  };

  return (
    <Box sx={{ padding: 0 }}>
      <Helmet>
        <meta name="description" content={`메가박스 경품 특전 수량 확인하기, 공식 사이트에서 공개되지 않는 특전 수량 확인이 가능합니다.`} />
        <meta property="og:type" content="website" />
        <meta property="og:title" content={`메가박스 경품 특전 수량 확인하기, 공식 사이트에서 공개되지 않는 특전 수량 확인이 가능합니다.`} />
        <meta property="og:description" content={`메가박스 경품 특전 수량 확인하기, 공식 사이트에서 공개되지 않는 특전 수량 확인이 가능합니다.`} />
        <meta property="og:url" content={window.location.href} />
      </Helmet>
      <Box sx={{ marginBottom: 2 }}>
        <Typography variant="h6" sx={{ fontSize: '0.95rem' }}>
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
                <React.Fragment key={item.id}>
                  <ListItem button component='a' href={`/megabox/event/giveaway/detail?eventIndex=${item.id}`}>
                    <ListItemText
                      primary={
                        <Box sx={{ display: 'flex', alignItems: 'center', marginRight:'5px' }}>
                          <Typography variant="h6" sx={{ fontSize: '0.95rem' }}>
                             <span dangerouslySetInnerHTML={{ __html: decodeHtml(item.title) }} />
                          </Typography>
                        </Box>
                      }
                      secondary={
                        <Box sx={{ display: 'flex', alignItems: 'center', marginRight:'5px' }}>
                          <VisibilityIcon fontSize="small" />
                          <Typography variant="body2" color="textSecondary" sx={{ marginLeft: 0.5 }}>
                            {item.viewCount}
                          </Typography>
                        </Box>
                      }
                      sx={{display: 'flex'}}
                    />
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
