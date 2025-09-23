'use client';
import { Box, Button, ButtonGroup } from '@mui/material';
import { Editor } from '@tiptap/core';
import AddLinkIcon from '@mui/icons-material/AddLink';
import LinkOffIcon from '@mui/icons-material/LinkOff';
import { useEditorState } from '@tiptap/react';
import React from 'react';
interface LinkButtonPropos {
  editor: Editor;
}

export default function LinkButtonComponent({ editor }: LinkButtonPropos) {
  const setLink = React.useCallback(() => {
    const previousUrl = editor.getAttributes('link').href;
    const url = window.prompt('URL', previousUrl);

    // cancelled
    if (url === null) {
      return;
    }

    // empty
    if (url === '') {
      editor.chain().focus().extendMarkRange('link').unsetLink().run();
      return;
    }

    // update link
    try {
      editor
        .chain()
        .focus()
        .extendMarkRange('link')
        .setLink({ href: url })
        .run();
    } catch (error) {
      return error;
    }
  }, [editor]);

  const editorState = useEditorState({
    editor,
    selector: (ctx) => ({
      isLink: ctx.editor.isActive('link'),
    }),
  });

  if (!editor) return null;
  return (
    <Box>
      <ButtonGroup variant="outlined">
        <Button
          onClick={setLink}
          variant={editorState.isLink ? 'contained' : 'outlined'}
        >
          <AddLinkIcon />
        </Button>
        <Button
          onClick={() => editor.chain().focus().unsetLink().run()}
          disabled={!editorState.isLink}
        >
          <LinkOffIcon />
        </Button>
      </ButtonGroup>
    </Box>
  );
}
