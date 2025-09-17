import { Box, Typography } from '@mui/material';

export default function Home() {
  return (
    <Box
      sx={{
        widith: '100%',
        height: '100dvh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
      }}
    >
      <Typography textAlign="center" variant="h1" color="primary">
        TESTE
      </Typography>
    </Box>
  );
}
