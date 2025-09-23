export interface User {
  id: number;
  name: string;
  email: string;
  password: number;
  role: 'user' | 'review' | 'writer' | 'admin';
}
