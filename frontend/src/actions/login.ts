'use server';

import { USER_GET } from './api';

export interface LoginData {
  email: string;
  password: string;
}

export interface UserResponse {
  id: string;
  email: string;
  name: string;
  role?: string;
  post: null;
}

export default async function loginForm(
  previousState: unknown,
  formData: FormData,
) {
  const name = formData.get('username') as string | null;
  const password = formData.get('password') as string | null;

  try {
    if (!name || !password) throw new Error('Senha ou usuário incorretos.');

    const { url } = USER_GET();
    const response = await fetch(url, {
      method: 'GET',
    });

    if (!response.ok) throw new Error('Falha ao fazer o login.');
    const data = (await response.json()) as UserResponse;
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
