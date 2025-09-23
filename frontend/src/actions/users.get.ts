'use server';

import { cookies } from 'next/headers';
import { USER_GET } from './api';

export interface LoginResponse {
  id: number;
  email: string;
  name: string;
  role: 'user' | 'admin' | 'review' | 'writer';
  post: PostType[];
}

export interface PostType {
  id: number;
  userId: number;
  title: string;
  text: string;
  userName: string;
}

export default async function getUsers() {
  try {
    const token = (await cookies()).get('token')?.value;
    const { url } = USER_GET();
    const response = await fetch(url, {
      method: 'GET',
      headers: {
        Authorization: 'Bearer ' + token,
      },
      next: {
        revalidate: 60,
      },
    });
    if (!response.ok) throw new Error('ao pegar os usuários');
    const data = (await response.json()) as LoginResponse[];
    return data;
  } catch (error: unknown) {
    if (error instanceof Error) {
      return error.message;
    } else {
      return null;
    }
  }
}
