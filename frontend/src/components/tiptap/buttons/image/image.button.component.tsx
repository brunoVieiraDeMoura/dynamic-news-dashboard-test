import { Box, Button } from '@mui/material';
import { Editor } from '@tiptap/core';
import AddPhotoAlternateIcon from '@mui/icons-material/AddPhotoAlternate';
import React from 'react';
import { ImageDataProps, postImage } from '@/actions/image';
import { API_URL } from '@/actions/api';

interface ImageButtonProps {
  editor: Editor;
}

export default function ImageButtonComponent({ editor }: ImageButtonProps) {
  const fileInputRef = React.useRef<HTMLInputElement>(null);

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    const formData = new FormData();
    formData.append('file', file);

    const data = (await postImage(formData)) as ImageDataProps;

    const url = `${API_URL}/uploads/${data.id}`;

    if (url) {
      editor
        .chain()
        .focus()
        .setImage({
          src: url,
          alt: 'A boring example image',
          title: 'An example',
        })
        .run();
    }
  };

  const handleClick = () => {
    fileInputRef.current?.click();
  };

  if (!editor) return null;

  return (
    <Box>
      <input
        ref={fileInputRef}
        type="file"
        accept="image/*"
        style={{ display: 'none' }}
        onChange={handleFileChange}
      />
      <Button onClick={handleClick} variant="outlined">
        <AddPhotoAlternateIcon />
      </Button>
    </Box>
  );
}
