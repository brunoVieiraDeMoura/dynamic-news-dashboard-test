import LoginForm from '@/components/login/login.form';
import { Box } from '@mui/material';

export default function Home() {
  return (
    <Box
      sx={{
        widith: '100%',
        height: '100dvh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        gap: 2,
        flexDirection: 'column',
      }}
    ></Box>
  );
}
