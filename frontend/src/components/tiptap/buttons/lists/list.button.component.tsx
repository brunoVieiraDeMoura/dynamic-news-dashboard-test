'use client';
import { Box, ButtonGroup, Button } from '@mui/material';
import FormatListBulletedIcon from '@mui/icons-material/FormatListBulleted';
import { Editor } from '@tiptap/core';

interface ListButtonProps {
  editor: Editor;
}

export default function ListButtonComponent({ editor }: ListButtonProps) {
  if (!editor) return null;
  return (
    <Box>
      <ButtonGroup variant="outlined">
        <Button
          onClick={() => editor.chain().focus().toggleBulletList().run()}
          variant={editor.isActive('bulletList') ? 'contained' : 'outlined'}
        >
          <FormatListBulletedIcon />
        </Button>
      </ButtonGroup>
    </Box>
  );
}
