'use service';

import getPosts from '@/actions/get.articles';
import Tiptap from '@/components/tiptap/tiptap.module';
import { Article } from '@/interfaces/artice';
import { Box, Typography } from '@mui/material';

export default async function EditorTipTap() {
  const article = (await getPosts()) as Article;
  if (!article) return null;
  return (
    <Box
      sx={{
        width: '100%',
        height: '100vh',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',

        alignContent: 'center',
      }}
    >
      <Tiptap content={article.title} />
      <Box>
        <Typography variant="h1" color="primary">
          {article?.id}
        </Typography>
      </Box>
    </Box>
  );
}
