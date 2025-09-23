'use client';

import { Box, Button, ButtonGroup } from '@mui/material';
import { Editor } from '@tiptap/core';
import React from 'react';

interface HeaderButtonPropos {
  editor: Editor | null;
}

export default function HeadersButtonComponent({ editor }: HeaderButtonPropos) {
  if (!editor) return null;

  return (
    <Box sx={{ width: '100%' }}>
      <ButtonGroup variant="outlined">
        <Button
          sx={{ fontWeight: 'bold' }}
          variant={
            editor.isActive('heading', { level: 1 }) ? 'contained' : 'outlined'
          }
          onClick={() =>
            editor.chain().focus().toggleHeading({ level: 1 }).run()
          }
        >
          H1
        </Button>
        <Button
          sx={{ fontWeight: 'bold' }}
          variant={
            editor.isActive('heading', { level: 2 }) ? 'contained' : 'outlined'
          }
          onClick={() =>
            editor.chain().focus().toggleHeading({ level: 2 }).run()
          }
        >
          H2
        </Button>
        <Button
          sx={{ fontWeight: 'bold' }}
          variant={
            editor.isActive('heading', { level: 3 }) ? 'contained' : 'outlined'
          }
          onClick={() =>
            editor.chain().focus().toggleHeading({ level: 3 }).run()
          }
        >
          H3
        </Button>
      </ButtonGroup>
    </Box>
  );
}
