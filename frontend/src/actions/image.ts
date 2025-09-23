'use server';
// src/app/api/upload/route.ts
import { NextResponse } from 'next/server';
import { PHOTO_POST } from './api';
import { cookies } from 'next/headers';

export interface ImageDataProps {
  id: number;
}

export async function postImage(formData: FormData) {
  const file = formData.get('file') as File | null;

  if (!file) {
    return NextResponse.json({ error: 'No file uploaded' }, { status: 400 });
  }
  try {
    const { url } = PHOTO_POST();
    const token = (await cookies()).get('token')?.value;
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + token,
      },
      body: JSON.stringify(file),
    });
    if (!response.ok) throw new Error('Erro ao tentar postar imagem');
    const data = (await response.json()) as ImageDataProps;
    return data;
  } catch (error) {
    return error;
  }

  // const bytes = await file.arrayBuffer();
  // const buffer = Buffer.from(bytes);
  // salvar em /public/uploads
  // const filePath = path.join(process.cwd(), 'public/uploads', file.name);
  // await writeFile(filePath, buffer);

  // salvar no BD o caminho (supondo tabela images)
  // ex: const image = await db.image.create({ data: { path: `/uploads/${file.name}` } });
}
