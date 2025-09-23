'use client';
import { Box, Button } from '@mui/material';
import { Editor } from '@tiptap/core';
import YouTubeIcon from '@mui/icons-material/YouTube';
interface YoutubeButtonPropos {
  editor: Editor;
}

export default function YoutubeButtonComponent({
  editor,
}: YoutubeButtonPropos) {
  function addYoutubeVideo() {
    const url = prompt('Enter YouTube URL');

    if (url) {
      editor.commands.setYoutubeVideo({
        src: url,
        width: 640,
        height: 480,
      });
    }
  }

  if (!editor) return null;

  return (
    <Box>
      <Button variant="outlined" onClick={addYoutubeVideo}>
        <YouTubeIcon />
      </Button>
    </Box>
  );
}
