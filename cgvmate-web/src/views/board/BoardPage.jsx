import React, { useEffect, useRef, useState } from 'react';
import { Container, Typography, Box, CircularProgress, List, ListItem, ListItemText, Tabs, Tab, Button, Pagination } from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import { IconPencilPlus } from '@tabler/icons-react';
import BoardApi from 'api/boardApi';
import { Helmet } from 'react-helmet-async';

const BoardPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const queryParams = new URLSearchParams(location.search);
  const id = queryParams.get('id');
  if (!id) {
    navigate(`/board/?id=free&pageNo=1`);
  }
  const [loading, setLoading] = useState(true);
  const [boards, setBoards] = useState([]);
  const [posts, setPosts] = useState([]);
  const [board, setBoard] = useState(null);
  const [pageNo, setPageNo] = useState(parseInt(queryParams.get('pageNo')) || 1);
  const [totalPages, setTotalPages] = useState(1); // 전체 페이지 수 상태 추가
  const api = useRef(new BoardApi()).current;

  useEffect(() => {
    const fetchBoards = async () => {
      const boardsData = await api.getBoardsAsync();
      setBoards(boardsData);

      const foundBoard = boardsData.find(x => x.id === id);
      if (foundBoard) {
        setBoard(foundBoard);
      }
    };

    fetchBoards();
  }, [id, api]);

  useEffect(() => {
    const fetchPosts = async () => {
      if (board && board.id) {
        const postsData = await api.getPostSummarysAsync(board.id, pageNo);
        setPosts(postsData);
        setTotalPages(pageNo + 1); // 전체 페이지 수 설정
      }
    };

    fetchPosts();
    setLoading(false);
  }, [board, pageNo]);

  const toggleNavItem = (event, newBoardId) => {
    const newBoard = boards.find(b => b.id === newBoardId);
    setBoard(newBoard);
    navigate(`/board/list?id=${newBoardId}`);
  };

  const handlePageChange = (event, value) => {
    setPageNo(value);
    navigate(`/board/?id=${id}&pageNo=${value}`);
  };

  const formatDateTime = (dateStr) => {
    const date = new Date(dateStr);
    const now = new Date();

    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    const weekday = date.toLocaleString('ko-KR', { weekday: 'short' });
    const hours = date.getHours();
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0');
    const period = hours >= 12 ? '오후' : '오전';
    const formattedHours = hours % 12 || 12;

    if (
      date.getFullYear() === now.getFullYear() &&
      date.getMonth() === now.getMonth() &&
      date.getDate() === now.getDate()
    ) {
      return `${hours.toString().padStart(2, '0')}:${minutes}`;
    }

    return `${year}. ${month}. ${day}`;
  };

  return (
    <Container sx={{ width: '100%', height: '100%', padding: '0px !important', margin: 0 }} maxWidth={false}>
      <Helmet>
        <title>커뮤니티</title>
      </Helmet>
      <Box component="header" sx={{ marginTop: 0, display: 'flex', flexDirection: 'row', justifyContent: 'space-between' }}>
        <Tabs value={board ? board.id : false} onChange={(event, newBoardId) => toggleNavItem(event, newBoardId)} variant='scrollable' sx={{ height: '30px !important', minHeight: '30px' }}>
          {boards.map((board) => (
            <Tab key={board.id} label={board.title} value={board.id} sx={{ padding: '2px', height: '30px !important', minHeight: '30px !important' }} />
          ))}
        </Tabs>
        <Button sx={{ padding: "0 !important", minWidth: 'auto' }} onClick={() => navigate(`/board/write/?id=${id}`)}>
          <IconPencilPlus />&nbsp;새 글 쓰기
        </Button>
      </Box>
      <Box component="main">
        <>
          <List sx={{ padding: 0, margin: 0 }}>
            {posts.length > 0 ? (
              posts.map((item) => (
                <React.Fragment key={item.id}>
                  <ListItem sx={{ padding: '10px' }} component={Link} to={`/board/detail/?boardId=${id}&postNo=${item.no}`}>
                    <ListItemText
                      primary={
                        <Box sx={{ display: 'flex', flexDirection: 'row', alignContent: 'center' }}>
                          <Typography variant="h6" component="span" sx={{ fontSize: '0.95rem' }}>
                            {`${item.title}`}
                          </Typography>
                          {item.commentCount > 0 &&
                            <Typography variant="h6" color="#ef642f" component="span" sx={{ fontSize: '0.9rem' }}>
                              &nbsp;{`(${item.commentCount})`}
                            </Typography>
                          }
                        </Box>
                      }
                      secondary={
                        <Box component="span" sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                          <Box component="span" sx={{ display: 'flex', alignItems: 'center' }}>
                            <Typography variant="body2" component="span" color="textSecondary" sx={{ marginRight: 1 }}>
                              {`${item.writerName}(${item.writerIP})`}
                            </Typography>
                            <VisibilityIcon fontSize="small" />
                            <Typography variant="body2" component="span" color="textSecondary" sx={{ marginLeft: 0.5 }}>
                              {item.viewCount}
                            </Typography>
                          </Box>
                          <Typography variant="body2" component="span" color="textSecondary" sx={{ marginRight: 1 }}>
                            {formatDateTime(item.dateCreated)}
                          </Typography>
                        </Box>
                      }
                    />
                  </ListItem>
                </React.Fragment>
              ))
            ) : (
              <Typography>글이 없습니다.</Typography>
            )}
          </List>
          <Box sx={{ display: 'flex', justifyContent: 'center', mt: 2 }}>
            <Pagination
              count={totalPages}
              page={pageNo}
              onChange={handlePageChange}
              color="primary"
            />
          </Box>
        </>
      </Box>
    </Container>
  );
};

export default BoardPage;
