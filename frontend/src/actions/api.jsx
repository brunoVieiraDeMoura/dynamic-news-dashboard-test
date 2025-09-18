export const API_URL = 'http://localhost:5056';

export function TOKEN_POST(body) {
  return {
    url: API_URL + '/jwt-auth/v1/token',
    options: {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
    },
    body: JSON.stringify(body),
  };
}

export function TOKEN_VALIDATE_POST(token) {
  return {
    url: API_URL + '/jwt-auth/v1/token',
    options: {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + token,
      },
    },
  };
}

export function USER_GET(token) {
  return {
    url: API_URL + '/user',
  };
}

export function USER_POST() {
  return {
    url: API_URL + '/user',
  };
}
