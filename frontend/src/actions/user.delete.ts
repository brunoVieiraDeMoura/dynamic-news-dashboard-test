'use server';

import { cookies } from 'next/headers';
import { USER_DELETE } from './api';

export default async function userDelete(id: number) {
  try {
    const token = (await cookies()).get('token')?.value;
    if (!id) throw new Error('Id não encontrado');

    const { url } = USER_DELETE(id);
    console.log(url);
    const response = await fetch(url, {
      method: 'DELETE',
      headers: {
        Authorization: 'Bearer ' + token,
      },
    });
    if (!response.ok) throw new Error('Erro ao deletar a foto.');
  } catch (error: unknown) {
    return error;
  }
}
