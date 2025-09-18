'use service';
import getUsers, { LoginResponse } from '@/actions/users.get';
import RegisterFormComponent from '@/components/register/register.form.component';
import { Box, Typography, Button } from '@mui/material';

export default async function RegisterAccount() {
  const data = (await getUsers()) as LoginResponse[];
  console.log(data.map((e) => e.name));

  if (!data) return null;
  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        gap: 4,
      }}
    >
      <RegisterFormComponent />
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          alignContent: 'center',
          flexDirection: 'column',
          gap: 2,
        }}
      >
        {data.map((data) => (
          <Box
            sx={{
              background: '#eee',
              p: 2,
              width: '400px',
              borderRadius: 2,
              display: 'flex',
              alignContent: 'center',
              alignItems: 'center',
              justifyContent: 'space-between',
            }}
            key={data.id}
          >
            <Typography variant="body1" color="primary">
              {data.email}
            </Typography>
            <Button variant="outlined" color="secondary">
              Delete
            </Button>
          </Box>
        ))}
      </Box>
    </Box>
  );
}
