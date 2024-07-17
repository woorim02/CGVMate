import React, { useState } from 'react';
import { Container, TextField, Button, Box, Typography, Paper } from '@mui/material';
import BoardApi from 'api/boardApi';
import { useLocation, useNavigate } from 'react-router-dom';
import { Helmet } from 'react-helmet-async';

const WritePage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const queryParams = new URLSearchParams(location.search);
  const id = queryParams.get('id') || 'free';
  const [title, setTitle] = useState('');
  const [writerName, setWriterName] = useState('ㅇㅇ');
  const [writerPassword, setWriterPassword] = useState('');
  const [contents, setContents] = useState([{ contentType: 1, body: '' }]);
  const api = new BoardApi();

  const handleContentChange = (index, event) => {
    const newContents = [...contents];
    newContents[0].body = event.target.value;
    setContents(newContents);
  };

  const handleSubmit = async () => {
    const post = {
      boardId: id,
      writerIP: '127.0.0.1',
      writerName,
      writerPassword,
      title,
      contents,
    };

    try {
      const response = await api.uploadPostAsync(post);
      console.log('Post uploaded successfully:', response);
      navigate(`/board/?id=${id}`);
    } catch (error) {
      alert('Error uploading post:', error);
    }
  };

  return (
    <Container>
      <Helmet>
        <title>게시글 작성</title>
      </Helmet>
      <Box component={Paper} sx={{ p: 3, mt: 1 }}>
        <Box sx={{ display: 'flex' }}>
          <TextField
            label="닉네임"
            value={writerName}
            onChange={(e) => setWriterName(e.target.value)}
            fullWidth
            margin="normal"
            sx={{ marginRight: '5px' }}
          />
          <TextField
            label="비밀번호"
            type="password"
            value={writerPassword}
            onChange={(e) => setWriterPassword(e.target.value)}
            fullWidth
            margin="normal"
          />
        </Box>
        <TextField
          label="제목"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          fullWidth
          margin="normal"
        />

        <TextField
          key={index}
          label={`본문`}
          value={contents[0].body}
          onChange={(e) => handleContentChange(index, e)}
          fullWidth
          multiline
          margin="normal"
        />
        <Box sx={{ display: 'flex', flexDirection: 'row-reverse' }}>
          <Button variant="contained" onClick={handleSubmit} sx={{ mt: 2, ml: 2 }}>
            등록
          </Button>
        </Box>
      </Box>
    </Container>
  );
};

export default WritePage;
