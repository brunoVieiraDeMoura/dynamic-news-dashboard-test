'use client';

import { Box, Button } from '@mui/material';
import { Editor } from '@tiptap/core';
import FormatQuoteIcon from '@mui/icons-material/FormatQuote';
interface QuoteButtonPropos {
  editor: Editor | null;
}

export default function QuoteButtomComponent({ editor }: QuoteButtonPropos) {
  if (!editor) return null;

  return (
    <Box>
      <Button
        variant={editor.isActive('blockquote') ? 'contained' : 'outlined'}
        onClick={() => editor.chain().focus().toggleBlockquote().run()}
      >
        <FormatQuoteIcon />
      </Button>
    </Box>
  );
}
