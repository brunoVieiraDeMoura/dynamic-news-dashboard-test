'use server';

import { cookies } from 'next/headers';
import { LOGIN_POST, USER_GET } from './api';

export interface LoginData {
  email: string;
  password: string;
}

export interface LoginReturn {
  email: string;
  name: string;
  token: string;
}

export default async function loginForm(
  previousState: unknown,
  formData: FormData,
) {
  const email = formData.get('email') as string | null;
  const password = formData.get('password') as string | null;

  try {
    if (!email || !password) throw new Error('Preencha os dados.');

    const payloadBody = {
      email,
      password,
    };

    const { url } = LOGIN_POST();
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ ...payloadBody }),
    });

    if (!response.ok) throw new Error('Senha ou usuário inválidos.');
    const data = (await response.json()) as LoginReturn;
    (await cookies()).set('token', data.token, {
      httpOnly: true,
      secure: true,
      sameSite: 'lax',
      maxAge: 60 * 60 * 24,
    });

    console.log(data);
    // tirar o retornodepois dos testes
    return { data: data };
  } catch (error: unknown) {
    if (error instanceof Error) {
      return { error: 'Senha ou usuário não preenchido.' };
    } else {
      return null;
    }
  }
}
