import Constants from '../constants';

class BoardApi {

  async getBoardsAsync() {
    const response = await fetch(`${Constants.API_HOST}/board`);
    const json = await response.json();
    return json;
  }

  async getPostSummarysAsync(boardId, pageNo = 1, pageSize = 10) {
    const response = await fetch(`${Constants.API_HOST}/post/${boardId}?pageNo=${pageNo}&pageSize=${pageSize}`);
    const json = await response.json();
    return json;
  }

  async getPostAsync(boardId, postNo) {
    const response = await fetch(`${Constants.API_HOST}/post?boardId=${boardId}&postNo=${postNo}`);
    const json = await response.json();
    json.content = JSON.parse(json.content);
    return json;
  }

  async deletePostAsync(postId, password) {
    const response = await fetch(`${Constants.API_HOST}/post/${postId}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json'
      },
      body: `"${password}"`
    });
    if(response.ok){
      return null;
    }
    const json = await response.json();
    if (json.status !== 500){
      return "알 수 없는 오류가 발생했습니다."
    }
    return json.detail; //오류 메세지
  }

  async uploadPostAsync(post) {
    const response = await fetch(`${Constants.API_HOST}/post`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(post)
    });
    const json = await response.json();
    return json;
  }

  async uploadCommentAsync(comment) {
    const response = await fetch(`${Constants.API_HOST}/comment`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(comment)
    });
    if(response.ok){
      return null;
    }
    const json = await response.json();
    return json;
  }

  async deleteCommentAsync(commentId, password)  {
    const response = await fetch(`${Constants.API_HOST}/comment/${commentId}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json'
      },
      body: `"${password}"`
    });
    if(response.ok){
      return null;
    }
    const json = await response.json();
    if (json.status !== 500){
      return "알 수 없는 오류가 발생했습니다."
    }
    return json.detail; //오류 메세지
  }
}

export default BoardApi;