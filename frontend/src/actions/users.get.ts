'use server';

import { USER_GET } from './api';

export interface LoginResponse {
  id: string;
  email: string;
  name: string;
  role?: string;
  post: null;
}

export default async function getUsers() {
  try {
    const { url } = USER_GET();
    const response = await fetch(url, {
      method: 'GET',
    });
    if (!response.ok) throw new Error('ao pegar os usuários');
    const data = (await response.json()) as LoginResponse;
    console.log('passou');
    return { data: data };
  } catch (error: unknown) {
    if (error instanceof Error) {
      return { error: 'Senha ou usuário não preenchido.' };
    } else {
      return null;
    }
  }
}
