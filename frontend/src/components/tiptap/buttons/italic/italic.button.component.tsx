'use client';
import { Box, Button } from '@mui/material';
import { Editor } from '@tiptap/core';
import FormatItalicIcon from '@mui/icons-material/FormatItalic';

interface ItalicButtonPropos {
  editor: Editor;
}

export default function ItalicButtonComponent({ editor }: ItalicButtonPropos) {
  if (!editor) return null;

  return (
    <Box>
      <Button
        onClick={() => editor.chain().focus().toggleItalic().run()}
        variant={editor.isActive('italic') ? 'contained' : 'outlined'}
      >
        <FormatItalicIcon />
      </Button>
    </Box>
  );
}
