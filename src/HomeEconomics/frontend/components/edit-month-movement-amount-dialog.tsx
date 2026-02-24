"use client";

import React from "react";
import type { FormEvent } from "react";
import {
  Alert,
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
} from "@mui/material";

type EditMonthMovementAmountDialogProps = {
  open: boolean;
  amount: string;
  submitting: boolean;
  errorMessage: string | null;
  onAmountChange: (value: string) => void;
  onCancel: () => void;
  onAccept: () => void;
};

export function EditMonthMovementAmountDialog({
  open,
  amount,
  submitting,
  errorMessage,
  onAmountChange,
  onCancel,
  onAccept,
}: EditMonthMovementAmountDialogProps) {
  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onAccept();
  };

  return (
    <Dialog
      data-testid="edit-amount-dialog"
      open={open}
      onClose={submitting ? undefined : onCancel}
      aria-labelledby="edit-amount-dialog-title"
      disableEscapeKeyDown={submitting}
    >
      <DialogTitle id="edit-amount-dialog-title">Editar importe</DialogTitle>
      <Box component="form" onSubmit={handleSubmit}>
        <DialogContent>
          <TextField
            label="Cantidad"
            data-testid="edit-amount-input"
            type="number"
            value={amount}
            onChange={(event) => onAmountChange(event.target.value)}
            disabled={submitting}
            required
            fullWidth
            inputProps={{ step: "0.01" }}
            autoFocus
          />
          {errorMessage ? (
            <Alert severity="error" sx={{ mt: 2 }}>
              {errorMessage}
            </Alert>
          ) : null}
        </DialogContent>
        <DialogActions>
          <Button onClick={onCancel} data-testid="edit-amount-cancel" disabled={submitting}>
            Cancelar
          </Button>
          <Button
            type="submit"
            variant="contained"
            data-testid="edit-amount-save"
            disabled={submitting}
          >
            Aceptar
          </Button>
        </DialogActions>
      </Box>
    </Dialog>
  );
}
