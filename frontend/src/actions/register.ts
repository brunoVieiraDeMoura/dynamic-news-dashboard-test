'use server';

import { USER_POST } from './api';

export interface LoginData {
  name: string;
  email: string;
  password: string;
}

export default async function RegisterForm(
  previousState: unknown,
  formData: FormData,
) {
  const name = formData.get('name') as string | null;
  const email = formData.get('email') as string | null;
  const password = formData.get('password') as string | null;
  const role = formData.get('role') as string | null;

  try {
    if (!name || !password || !email || !role)
      throw new Error('Preencha os dados');

    const { url } = USER_POST();
    const payload = {
      name,
      email,
      password,
      role,
    };
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });
    console.log(response);
    if (!response.ok) throw new Error('Email ou usuário cadastrados');

    const data = (await response.json()) as LoginData;
    return data;
  } catch (error: unknown) {
    if (error instanceof Error) {
      return { data: null };
    } else {
      return null;
    }
  }
}
