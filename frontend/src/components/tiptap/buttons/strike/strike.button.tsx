'use client';
import { Box, Button } from '@mui/material';
import { Editor } from '@tiptap/core';
import StrikethroughSIcon from '@mui/icons-material/StrikethroughS';
interface StrikeButtonPropos {
  editor: Editor;
}

export default function StrikeButtonComponent({ editor }: StrikeButtonPropos) {
  if (!editor) return null;

  return (
    <Box>
      <Button
        variant={editor.isActive('strike') ? 'contained' : 'outlined'}
        onClick={() => editor.chain().focus().toggleStrike().run()}
      >
        <StrikethroughSIcon />
      </Button>
    </Box>
  );
}
