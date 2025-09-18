export const API_URL = 'http://localhost:5056';

export function USER_GET() {
  return {
    url: API_URL + '/users',
  };
}

export function USER_POST() {
  return {
    url: API_URL + '/user',
  };
}

export function USER_DELETE(id: string) {
  return {
    url: ` ${API_URL}/user/${id}`,
  };
}
