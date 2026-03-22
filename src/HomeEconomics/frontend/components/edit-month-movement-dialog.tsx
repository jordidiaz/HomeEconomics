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
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Stack,
  TextField,
} from "@mui/material";
import { MovementType } from "../types/movement-type";

type EditMonthMovementDialogProps = {
  open: boolean;
  name: string;
  amount: string;
  type: MovementType;
  submitting: boolean;
  errorMessage: string | null;
  onNameChange: (value: string) => void;
  onAmountChange: (value: string) => void;
  onTypeChange: (value: MovementType) => void;
  onCancel: () => void;
  onAccept: () => void;
};

const typeOptions = [
  { value: MovementType.Income, label: "Ingreso" },
  { value: MovementType.Expense, label: "Gasto" },
];

export function EditMonthMovementDialog({
  open,
  name,
  amount,
  type,
  submitting,
  errorMessage,
  onNameChange,
  onAmountChange,
  onTypeChange,
  onCancel,
  onAccept,
}: EditMonthMovementDialogProps) {
  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onAccept();
  };

  return (
    <Dialog
      data-testid="edit-movement-dialog"
      open={open}
      onClose={submitting ? undefined : onCancel}
      aria-labelledby="edit-movement-dialog-title"
      disableEscapeKeyDown={submitting}
    >
      <DialogTitle id="edit-movement-dialog-title">Editar movimiento</DialogTitle>
      <Box component="form" onSubmit={handleSubmit}>
        <DialogContent>
          <Stack spacing={2}>
            <TextField
              label="Nombre"
              data-testid="edit-movement-name-input"
              value={name}
              onChange={(event) => onNameChange(event.target.value)}
              disabled={submitting}
              required
              fullWidth
              autoFocus
            />
            <TextField
              label="Cantidad"
              data-testid="edit-movement-amount-input"
              type="number"
              value={amount}
              onChange={(event) => onAmountChange(event.target.value)}
              disabled={submitting}
              required
              fullWidth
              inputProps={{ step: "0.01" }}
            />
            <FormControl fullWidth required disabled={submitting}>
              <InputLabel id="edit-movement-type-label">Tipo</InputLabel>
              <Select
                labelId="edit-movement-type-label"
                data-testid="edit-movement-type-select"
                value={type === MovementType.Undefined ? "" : type}
                label="Tipo"
                onChange={(event) => onTypeChange(Number(event.target.value))}
              >
                {typeOptions.map((option) => (
                  <MenuItem key={option.value} value={option.value}>
                    {option.label}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            {errorMessage ? (
              <Alert severity="error">{errorMessage}</Alert>
            ) : null}
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={onCancel} data-testid="edit-movement-cancel" disabled={submitting}>
            Cancelar
          </Button>
          <Button
            type="submit"
            variant="contained"
            data-testid="edit-movement-save"
            disabled={submitting}
          >
            Aceptar
          </Button>
        </DialogActions>
      </Box>
    </Dialog>
  );
}
