'use client';
import loginForm from '@/actions/login';
// import ErrorMessage from '@/helper/error-message';

import { Box, Button, Paper, TextField, Typography } from '@mui/material';
import { useActionState } from 'react';

// type IForm = React.PropsWithChildren & {
//   error: string;
// };

export default function LoginForm() {
  const [data, action, isPending] = useActionState(loginForm, undefined);

  return (
    <Box
      display="flex"
      justifyContent="center"
      alignItems="center"
      minHeight="100vh"
      bgcolor="#f5f5f5"
    >
      <Paper elevation={3} sx={{ p: 4, maxWidth: 400, width: '100%' }}>
        <Typography variant="h5" mb={2} textAlign="center" color="primary">
          Login
        </Typography>
        <form action={action}>
          <TextField
            label="Username"
            name="username"
            type="text"
            variant="outlined"
            fullWidth
            margin="normal"
            required
          />
          <TextField
            label="Senha"
            name="password"
            type="password"
            variant="outlined"
            fullWidth
            margin="normal"
            required
          />
          <Button
            disabled={isPending}
            type="submit"
            variant="contained"
            color="primary"
            fullWidth
            sx={{ mt: 2, textTransform: 'none' }}
          >
            {isPending ? 'Entrando...' : 'Entrar'}
          </Button>
        </form>
        <Typography
          variant="h5"
          mb={2}
          textAlign="center"
          color="primary"
        ></Typography>
        {data?.message ? (
          <Typography variant="h5" mb={2} textAlign="center" color="primary">
            {data.message}
          </Typography>
        ) : null}

        {data?.error ? (
          <Typography variant="h5" mb={2} textAlign="center" color="primary">
            {data.error}
          </Typography>
        ) : null}

        {data?.fieldData ? (
          <Typography variant="h5" mb={2} textAlign="center" color="primary">
            {data.fieldData}
          </Typography>
        ) : null}
      </Paper>
    </Box>
  );
}
