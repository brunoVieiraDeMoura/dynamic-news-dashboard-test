'use client';
import { Box, Button } from '@mui/material';
import { Editor } from '@tiptap/core';
import React from 'react';

interface BoldButtonPropos {
  editor: Editor | null;
}

export default function BoldButtonComponent({ editor }: BoldButtonPropos) {
  if (!editor) return null;

  return (
    <Box sx={{}}>
      <Button
        variant={editor.isActive('bold') ? 'contained' : 'outlined'}
        onClick={() => editor.chain().focus().toggleBold().run()}
        sx={{ fontWeight: 'bold' }}
      >
        B
      </Button>
    </Box>
  );
}
