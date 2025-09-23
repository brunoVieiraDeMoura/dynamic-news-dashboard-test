'use client';

import { Editor } from '@tiptap/core';
import { Box } from '@mui/material';
import HeadersButtonComponent from '../buttons/headers/headers.button.component';
import React from 'react';
import BoldButtonComponent from '../buttons/bold/bold.button.component';
import QuoteButtomComponent from '../buttons/quote/quote.button.component';
import StrikeButtonComponent from '../buttons/strike/strike.button';
import UnderlineButtonComponent from '../buttons/underline/underline.button.component';
import ItalicButtonComponent from '../buttons/italic/italic.button.component';
import LinkButtonComponent from '../buttons/link/link.button.component';
import ListButtonComponent from '../buttons/lists/list.button.component';
import YoutubeButtonComponent from '../buttons/youtube/youtube.button.component';
import ImageButtonComponent from '../buttons/image/image.button.component';

interface BlockButtonsProps {
  editor: Editor | null;
}

export default function BlockButtonsComponent({ editor }: BlockButtonsProps) {
  const [, setVersion] = React.useState(0);

  React.useEffect(() => {
    if (!editor) return;

    const updateHandler = () => {
      setVersion((v) => v + 1);
    };

    editor.on('selectionUpdate', updateHandler);
    editor.on('transaction', updateHandler);

    return () => {
      editor.off('selectionUpdate', updateHandler);
      editor.off('transaction', updateHandler);
    };
  }, [editor]);

  if (!editor) return null;

  return (
    <Box
      sx={{
        width: '100%',
        p: 2,
        gap: 1,
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
      }}
    >
      <HeadersButtonComponent editor={editor} />
      <BoldButtonComponent editor={editor} />
      <ItalicButtonComponent editor={editor} />
      <UnderlineButtonComponent editor={editor} />
      <QuoteButtomComponent editor={editor} />
      <StrikeButtonComponent editor={editor} />
      <LinkButtonComponent editor={editor} />
      <ListButtonComponent editor={editor} />
      <YoutubeButtonComponent editor={editor} />
      <ImageButtonComponent editor={editor} />
    </Box>
  );
}
