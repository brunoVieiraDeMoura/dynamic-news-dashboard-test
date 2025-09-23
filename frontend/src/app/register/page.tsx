// app/register/page.tsx (Server Component)
'use server';

import getUsers, { LoginResponse } from '@/actions/users.get';
import RegisterFormComponent from '@/components/register/register.form.component';
import UserList from '@/components/user/userlist';
import { Box } from '@mui/material';

export default async function RegisterAccount() {
  const data = (await getUsers()) as LoginResponse[];
  if (!data) return null;

  return (
    <>
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          gap: 4,
        }}
      >
        <RegisterFormComponent />
        <UserList users={data} />
      </Box>
    </>
  );
}
