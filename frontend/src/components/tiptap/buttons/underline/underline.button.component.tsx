'use client';

import { Box, Button } from '@mui/material';
import { Editor } from '@tiptap/core';
import FormatUnderlinedIcon from '@mui/icons-material/FormatUnderlined';
interface UnderlineButtonProps {
  editor: Editor;
}

export default function UnderlineButtonComponent({
  editor,
}: UnderlineButtonProps) {
  if (!editor) return null;
  return (
    <Box>
      <Button
        variant={editor.isActive('underline') ? 'contained' : 'outlined'}
        onClick={() => editor.chain().focus().toggleUnderline().run()}
      >
        <FormatUnderlinedIcon />
      </Button>
    </Box>
  );
}
