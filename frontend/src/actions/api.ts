export const API_URL = 'http://localhost:5056';

export function USER_GET() {
  return {
    url: API_URL + '/users',
  };
}

export function USER_POST() {
  return {
    url: API_URL + '/users',
  };
}

export function USER_PATCH(id: number) {
  return {
    url: ` ${API_URL}/users/${id}`,
  };
}

export function USER_DELETE(id: number) {
  return {
    url: ` ${API_URL}/users/${id}`,
  };
}

export function POST_POST() {
  return {
    url: `${API_URL}/posts`,
  };
}

export function GET_POSTS() {
  const id = 5;
  return {
    url: `${API_URL}/posts/${id}`,
  };
}

export function LOGIN_POST() {
  return {
    url: `${API_URL}/login`,
  };
}

export function PHOTO_POST() {
  return {
    url: `${API_URL}/upload`,
  };
}

export function PHOTO_DELETE(id: number) {
  return {
    url: `${API_URL}/upload/${id}`,
  };
}
