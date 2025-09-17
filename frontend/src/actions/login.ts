'use server';

export interface LoginData {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: {
    id: string;
    email: string;
    name: string;
  };
}

export default async function loginForm(
  previousState: unknown,
  formData: FormData,
) {
  const username = formData.get('username') as string | null;
  const password = formData.get('password') as string | null;

  try {
    if (username && password) {
      return { message: 'Usuário Logado', fieldData: username };
    } else {
      throw new Error('Senha ou usuário não preenchido.');
    }
  } catch (error: unknown) {
    if (error instanceof Error) {
      return { error: 'Senha ou usuário não preenchido.' };
    } else {
      return null;
    }
  }
}
