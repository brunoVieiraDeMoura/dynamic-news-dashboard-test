// components/register/user.list.tsx
'use client';

import { Box, Typography, Button } from '@mui/material';
import userDelete from '@/actions/user.delete';
import { LoginResponse } from '@/actions/users.get';
import { useTransition } from 'react';

interface Props {
  users: LoginResponse[];
}

export default function UserList({ users }: Props) {
  const [isPending, startTransition] = useTransition();

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        gap: 2,
      }}
    >
      {users.map((data) => (
        <Box
          key={data.id}
          sx={{
            background: '#eee',
            p: 2,
            width: '400px',
            borderRadius: 2,
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
          }}
        >
          <Typography variant="body1" color="primary">
            {data.email}
          </Typography>
          <Button
            variant="outlined"
            color="secondary"
            disabled={isPending}
            onClick={() =>
              startTransition(async () => {
                await userDelete(data.id);
              })
            }
          >
            {isPending ? 'Deletando...' : 'Delete'}
          </Button>
        </Box>
      ))}
    </Box>
  );
}
