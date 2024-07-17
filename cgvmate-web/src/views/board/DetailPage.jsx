import React, { useEffect, useState } from 'react';
import { Container, Typography, Box, CircularProgress, List, ListItem, ListItemText, Paper, Button, TextField, Divider } from '@mui/material';
import { useLocation, useNavigate } from 'react-router-dom';
import BoardApi from 'api/boardApi';
import { Helmet } from 'react-helmet-async';

const DetailPage = () => {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const boardId = queryParams.get('boardId');
  const postNo = queryParams.get('postNo');
  const [post, setPost] = useState(null);
  const [loading, setLoading] = useState(true);
  const [commentWriter, setCommentWriter] = useState('');
  const [commentPassword, setCommentPassword] = useState('');
  const [commentContent, setCommentContent] = useState('');
  const api = new BoardApi();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPost = async () => {
      try {
        const postData = await api.getPostAsync(boardId, postNo);
        setPost(postData);
      } catch (error) {
        console.error('Failed to fetch post', error);
      } finally {
        setLoading(false);
      }
    };

    fetchPost();
  }, [boardId, postNo]);

  const handleCommentSubmit = async () => {
    if (!commentContent) return;

    const newComment = {
      postId: post.id,
      parentCommentId: -1,
      writerIP: '127.0.0.1',
      writerName: commentWriter,
      writerPassword: commentPassword,
      userId: -1,
      content: commentContent
    };

    try {
      await api.uploadCommentAsync(newComment);
      setCommentContent('');
      const postData = await api.getPostAsync(boardId, postNo);
      setPost(postData);
    } catch (error) {
      console.error('Failed to upload comment', error);
    }
  };

  const formatDateTime = (dateStr) => {
    const date = new Date(dateStr);
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    const hours = date.getHours();
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const formattedHours = hours % 12 || 12;

    return `${year}. ${month}. ${day}. ${formattedHours.toString().padStart(2, '0')}:${minutes}`;
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (!post) {
    return <Typography>게시물을 불러올 수 없습니다.</Typography>;
  }

  return (
    <Container sx={{ width: '100%', height: '100%', padding: '0px !important', margin: 0 }} maxWidth={false}>      {model && (
      <Helmet>
        <title>{post}</title>
        <meta name="description" content={`${post.title}`} />
        <meta property="og:type" content="website" />
        <meta property="og:title" content={`${post.title}`} />
        <meta property="og:description" content={`${post.title}`} />
        <meta property="og:url" content={window.location.href} />
      </Helmet>
    )}
      <Paper elevation={3} sx={{ padding: 2, marginTop: 0, boxShadow: 'none' }}>
        <Typography variant="h4">{post.title}</Typography>
        <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant="body1">{`${post.writerName} (${post.writerIP})`}</Typography>
        </Box>
        <Typography variant="body1" sx={{ marginBottom: 1 }}>{`조회수: ${post.viewCount} | ${formatDateTime(post.dateCreated)}`}</Typography>
        <Divider sx={{ marginBottom: 2 }} />
        <Box>
          {post.content.map((paragraph, index) => (
            <Typography key={index} variant="body1" paragraph sx={{ wordWrap: 'break-word', whiteSpace: 'pre-wrap' }}>
              {paragraph.Body}
            </Typography>
          ))}
        </Box>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', marginTop: 2 }}>
          <Typography variant="body1"></Typography>
          <Box>
            <Button variant="contained" color="primary" onClick={() => navigate('/board/delete/?postId=' + post.id)}>
              삭제하기
            </Button>
          </Box>
        </Box>
      </Paper>
      <Box sx={{ marginTop: 2 }}>
        <Typography variant="h5">댓글</Typography>
        <List>
          {post.comments.map((comment) => (
            <ListItem key={comment.id} alignItems="flex-start" sx={{ width: '100%' }}>
              <Box sx={{ width: '100%' }}>
                <Box>
                  <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between', width: '100%' }}>
                    <Typography color='gray' fontSize='0.8rem'>{`${comment.writerName}(${comment.writerIP})`}</Typography>
                    <Box sx={{ display: 'flex', flexDirection: 'row' }}>
                      <Typography color='grey' fontSize='0.8rem'>{`${formatDateTime(comment.dateCreated)}`}</Typography>
                      <Button sx={{padding: '0px', margin:0 , minWidth: '40px', lineHeight: 'none !important', fontSize: '0.8rem'}}
                       onClick={() => navigate(`/board/delete/?commentId=${comment.id}&boardId=${boardId}&postNo=${postNo}`)}>
                        삭제
                      </Button>
                    </Box>
                  </Box>
                  <Typography>{`${comment.content}`}</Typography>
                </Box>
              </Box>
            </ListItem>
          ))}
        </List>
        <Box sx={{ marginTop: 2 }}>
          <Box sx={{ display: 'flex' }}>
            <TextField
              label="닉네임"
              value={commentWriter}
              onChange={(e) => setCommentWriter(e.target.value)}
              fullWidth
              margin="normal"
              size='small'
              sx={{ marginRight: '5px' }}
            />
            <TextField
              label="비밀번호"
              type="password"
              value={commentPassword}
              onChange={(e) => setCommentPassword(e.target.value)}
              fullWidth
              size='small'
              margin="normal"
            />
          </Box>
          <TextField
            fullWidth
            variant="outlined"
            label="댓글"
            value={commentContent}
            multiline
            onChange={(e) => setCommentContent(e.target.value)}
          />
          <Box sx={{ display: 'flex', flexDirection: 'row-reverse' }}>
            <Button variant="contained" color="primary" onClick={handleCommentSubmit} sx={{ marginTop: 1 }}>
              작성
            </Button>
          </Box>
        </Box>
      </Box>
    </Container>
  );
};

export default DetailPage;
