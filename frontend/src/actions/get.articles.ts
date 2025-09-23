'use server';

import { Article } from '@/interfaces/artice';
import { GET_POSTS } from './api';

export default async function getPosts() {
  try {
    const { url } = GET_POSTS();
    const response = await fetch(url, {
      method: 'GET',
    });
    if (!response.ok) throw new Error('ao pegar os usuários');
    const data = (await response.json()) as Article;
    return data;
  } catch (error: unknown) {
    if (error instanceof Error) {
      return error.message;
    } else {
      return null;
    }
  }
}
