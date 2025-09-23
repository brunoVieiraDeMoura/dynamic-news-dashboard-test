'use client';

import { Box } from '@mui/material';
import Link from 'next/link';

export default function Header() {
  return (
    <Box
      sx={{
        display: 'flex',
        position: 'fixed',
        width: '100%',
        flexWrap: 'wrap',
        gap: 2,
        fontSize: '1.25rem',
        fontWeight: '500',
        padding: 2,
        justifyContent: 'end',

        background: '#1976D2',
        color: '#fff',
      }}
    >
      <Link style={{ textDecoration: 'none', color: 'inherit' }} href={'/'}>
        Home
      </Link>
      <Link
        style={{ textDecoration: 'none', color: 'inherit' }}
        href={'/register'}
      >
        Register
      </Link>
      <Link
        style={{ textDecoration: 'none', color: 'inherit' }}
        href={'/login'}
      >
        Login
      </Link>
      <Link
        style={{ textDecoration: 'none', color: 'inherit' }}
        href={'/editor'}
      >
        Editor
      </Link>
      <Link
        style={{ textDecoration: 'none', color: 'inherit' }}
        href={'/dashboard'}
      >
        Dashboard
      </Link>
    </Box>
  );
}
