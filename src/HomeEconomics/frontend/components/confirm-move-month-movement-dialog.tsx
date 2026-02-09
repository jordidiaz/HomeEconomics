"use client";

import {
  Alert,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";

type ConfirmMoveMonthMovementDialogProps = {
  open: boolean;
  moving: boolean;
  errorMessage: string | null;
  movementName: string;
  onCancel: () => void;
  onConfirm: () => void;
};

export function ConfirmMoveMonthMovementDialog({
  open,
  moving,
  errorMessage,
  movementName,
  onCancel,
  onConfirm,
}: ConfirmMoveMonthMovementDialogProps) {
  return (
    <Dialog
      open={open}
      onClose={moving ? undefined : onCancel}
      aria-labelledby="move-month-movement-dialog-title"
      disableEscapeKeyDown={moving}
    >
      <DialogTitle id="move-month-movement-dialog-title">Confirmar acción</DialogTitle>
      <DialogContent>
        <DialogContentText>
          ¿Quieres mover el movimiento &quot;{movementName}&quot; al mes siguiente? Esta
          acción es irreversible y se moverá permanentemente.
        </DialogContentText>
        {errorMessage ? (
          <Alert severity="error" sx={{ mt: 2 }}>
            {errorMessage}
          </Alert>
        ) : null}
      </DialogContent>
      <DialogActions>
        <Button onClick={onCancel} disabled={moving}>
          Cancelar
        </Button>
        <Button onClick={onConfirm} variant="contained" disabled={moving}>
          Aceptar
        </Button>
      </DialogActions>
    </Dialog>
  );
}
