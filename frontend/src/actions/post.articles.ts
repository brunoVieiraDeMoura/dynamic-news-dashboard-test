'use server';

import { cookies } from 'next/headers';
import { POST_POST } from './api';

export async function postArticles(
  state: { success: boolean },
  formData: FormData,
) {
  const token = (await cookies()).get('token')?.value;
  const content = formData.get('content') as string;
  const title = formData.get('title') as string;
  const id = formData.get('id') as string;

  const { url } = POST_POST();

  const payload = {
    userId: Number(id),
    title: title,
    text: content,
  };

  const response = await fetch(url, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + token,
    },
    body: JSON.stringify({ ...payload }),
  });

  if (!response.ok) throw new Error('Erro ao envio');

  return { success: true };
}
