import { USER_DELETE } from './api';

export default async function userDelete(id: string) {
  try {
    if (!id) throw new Error('Id não encontrado');
    const { url } = USER_DELETE(id);
    console.log(url);
    const response = await fetch(url, {
      method: 'DELETE',
    });
    if (!response.ok) throw new Error('Erro ao deletar a foto.');
  } catch (error: unknown) {
    return error;
  }
}
