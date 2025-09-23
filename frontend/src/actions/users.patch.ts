'use server';

import { cookies } from 'next/headers';
import { USER_PATCH } from './api';

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

export default async function updateUser(
  id: number,
  data: Partial<LoginResponse>,
) {
  try {
    const token = (await cookies()).get('token')?.value;
    const { url } = USER_PATCH(id);

    const response = await fetch(url, {
      method: 'PUT',
      headers: {
        Authorization: 'Bearer ' + token,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      throw new Error('Erro ao atualizar usuário');
    }

    const result: LoginResponse = await response.json();
    return result;
  } catch (error: unknown) {
    if (error instanceof Error) {
      console.error(error.message);
      return null;
    } else {
      return null;
    }
  }
}
