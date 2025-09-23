'use client';

import {
  Box,
  Typography,
  Button,
  Select,
  MenuItem,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from '@mui/material';
import React from 'react';
import { LoginResponse } from '@/actions/users.get';
import updateUser from '@/actions/users.patch';
import userDelete from '@/actions/user.delete';

interface Props {
  users: LoginResponse[];
}

export default function UserList({ users }: Props) {
  const [isPending, startTransition] = React.useTransition();
  const [values, setValues] = React.useState<Record<number, string>>(() =>
    users.reduce((acc, user) => ({ ...acc, [user.id]: user.role }), {}),
  );

  // Estados para o diálogo
  const [openDialog, setOpenDialog] = React.useState(false);
  const [selectedUserId, setSelectedUserId] = React.useState<number | null>(
    null,
  );
  const [newRole, setNewRole] = React.useState<
    'user' | 'admin' | 'review' | 'writer' | undefined
  >(undefined);

  const handleChange = (
    userId: number,
    role: 'user' | 'admin' | 'review' | 'writer',
  ) => {
    // Abrir modal de confirmação
    setSelectedUserId(userId);
    setNewRole(role);
    setOpenDialog(true);
  };

  const handleConfirm = () => {
    if (!newRole || selectedUserId === null) return;
    console.log(newRole, selectedUserId);
    if (selectedUserId !== null) {
      startTransition(async () => {
        const updated = await updateUser(selectedUserId, { role: newRole });
        if (updated) {
          setValues((prev) => ({ ...prev, [selectedUserId]: newRole }));
        }
        setOpenDialog(false);
      });
    }
  };

  const handleCancel = () => {
    setOpenDialog(false);
  };

  return (
    <Box
      sx={{ display: 'flex', flexDirection: 'column', gap: 2, width: '800px' }}
    >
      {users.map((user) => (
        <Box
          key={user.id}
          sx={{
            background: '#eee',
            p: 2,
            gap: 2,
            borderRadius: 2,
            display: 'flex',
            flexDirection: 'column',
          }}
        >
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              width: '100%',
              gap: 2,
            }}
          >
            <Typography variant="body1" color="primary">
              {user.email}
            </Typography>
            <Button
              variant="outlined"
              color="secondary"
              disabled={isPending}
              onClick={() =>
                startTransition(async () => {
                  await userDelete(user.id);
                })
              }
            >
              {isPending ? 'Deletando...' : 'Delete'}{' '}
            </Button>
          </Box>
          <Box sx={{ width: 160 }}>
            <Select
              value={values[user.id] || user.role}
              onChange={(e) =>
                handleChange(
                  user.id,
                  e.target.value as 'user' | 'admin' | 'review' | 'writer',
                )
              }
              displayEmpty
              sx={{ minWidth: '160px', mt: 1 }}
            >
              <MenuItem value="user">Usuário</MenuItem>
              <MenuItem value="writer">Escritor</MenuItem>
              <MenuItem value="review">Revisor</MenuItem>
              <MenuItem value="admin">Administrador</MenuItem>
            </Select>
          </Box>
        </Box>
      ))}

      {/* Modal de confirmação */}
      <Dialog open={openDialog} onClose={handleCancel}>
        <DialogTitle>Confirmação</DialogTitle>
        <DialogContent>
          <Typography>
            Tem certeza que deseja alterar a role para{' '}
            <strong>{newRole}</strong>?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCancel} color="secondary">
            Cancelar
          </Button>
          <Button onClick={handleConfirm} color="primary" autoFocus>
            Confirmar
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
