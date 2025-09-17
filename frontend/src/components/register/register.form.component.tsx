'use client';

import RegisterForm from '@/actions/register';
import { Box, Button, Paper, TextField, Typography } from '@mui/material';
import { useActionState } from 'react';

export default function RegisterFormComponent() {
  const [data, action, isPending] = useActionState(RegisterForm, undefined);

  return (
    <Box
      display="flex"
      justifyContent="center"
      alignItems="center"
      bgcolor="#f5f5f5"
      sx={{
        height: '100dvh',
      }}
    >
      <Paper elevation={3} sx={{ p: 4, maxWidth: 400, width: '100%' }}>
        <Typography variant="h5" mb={2} textAlign="center" color="primary">
          Registro
        </Typography>
        <form action={action}>
          <TextField
            label="Username"
            name="name"
            type="text"
            variant="outlined"
            fullWidth
            margin="normal"
            required
          />
          <TextField
            label="Email"
            name="email"
            type="email"
            variant="outlined"
            fullWidth
            margin="normal"
          />
          <TextField
            label="Senha"
            name="password"
            type="password"
            variant="outlined"
            fullWidth
            margin="normal"
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

        {data ? data.fieldData : null}
      </Paper>
    </Box>
  );
}
