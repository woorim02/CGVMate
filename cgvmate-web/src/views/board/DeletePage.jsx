import React, { useState } from 'react';
import { Container, Typography, Box, Button, TextField, Alert } from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';
import BoardApi from 'api/boardApi';

const DeletePage = () => {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const postId = queryParams.get('postId');
  const commentId = queryParams.get('commentId');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const api = new BoardApi();
  const navigate = useNavigate();

  const handleDelete = async () => {
    if (postId) {
      try {
        const result = await api.deletePostAsync(postId, password);
        if (result) {
          setErrorMessage(result);
        } else {
          navigate('/board');
        }
      } catch (error) {
        console.error('Failed to delete post', error);
        setErrorMessage('알 수 없는 오류가 발생했습니다.');
      }
      return;
    }
    if(commentId){
      try {
        const result = await api.deleteCommentAsync(commentId, password);
        if (result) {
          setErrorMessage(result);
        } else {
          navigate(`/board/detail/?boardId=${queryParams.get('boardId')}&postNo=${queryParams.get('postNo')}`);
        }
      } catch (error) {
        console.error('Failed to delete post', error);
        setErrorMessage('알 수 없는 오류가 발생했습니다.');
      }
    }
  };

  return (
    <Container maxWidth="sm">
      <Box sx={{ marginTop: 4 }}>
        <Typography variant="h4" gutterBottom>
          {postId ? "게시글 삭제" : "댓글 삭제"}
        </Typography>
        <TextField
          label="비밀번호"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          fullWidth
          margin="normal"
          sx={{ marginBottom: 2 }}
        />
        {errorMessage && (
          <Alert severity="error" sx={{ marginBottom: 2 }}>
            {errorMessage}
          </Alert>
        )}
        <Button variant="contained" color="primary" onClick={handleDelete} fullWidth>
          삭제
        </Button>
        <Button variant="outlined" color="secondary" onClick={() => navigate(-1)} fullWidth sx={{ marginTop: 2 }}>
          취소
        </Button>
      </Box>
    </Container>
  );
};

export default DeletePage;
