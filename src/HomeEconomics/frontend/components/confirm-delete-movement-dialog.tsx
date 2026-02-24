"use client";

import React from "react";
import {
  Alert,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";

type ConfirmDeleteMovementDialogProps = {
  open: boolean;
  deleting: boolean;
  errorMessage: string | null;
  movementName: string;
  onCancel: () => void;
  onConfirm: () => void;
};

export function ConfirmDeleteMovementDialog({
  open,
  deleting,
  errorMessage,
  movementName,
  onCancel,
  onConfirm,
}: ConfirmDeleteMovementDialogProps) {
  return (
    <Dialog
      data-testid="confirm-delete-movement-dialog"
      open={open}
      onClose={deleting ? undefined : onCancel}
      aria-labelledby="delete-dialog-title"
      disableEscapeKeyDown={deleting}
    >
      <DialogTitle id="delete-dialog-title">Confirmar borrado</DialogTitle>
      <DialogContent>
        <DialogContentText>
          ¿Quieres eliminar el movimiento &quot;{movementName}&quot;? Esta acción
          es irreversible y se eliminará permanentemente.
        </DialogContentText>
        {errorMessage ? (
          <Alert severity="error" data-testid="delete-movement-error" sx={{ mt: 2 }}>
            {errorMessage}
          </Alert>
        ) : null}
      </DialogContent>
      <DialogActions>
        <Button onClick={onCancel} data-testid="delete-movement-cancel" disabled={deleting}>
          Cancelar
        </Button>
        <Button
          onClick={onConfirm}
          variant="contained"
          color="error"
          data-testid="delete-movement-confirm"
          disabled={deleting}
        >
          Eliminar
        </Button>
      </DialogActions>
    </Dialog>
  );
}
