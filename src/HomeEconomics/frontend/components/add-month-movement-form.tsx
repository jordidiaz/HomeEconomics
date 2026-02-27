"use client";

import React from "react";
import type { FormEvent } from "react";
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Stack,
  TextField,
} from "@mui/material";
import { MovementType } from "../types/movement-type";

type AddMonthMovementFormProps = {
  name: string;
  amount: string;
  type: MovementType;
  submitting: boolean;
  errorMessage: string | null;
  validationMessage: string | null;
  onNameChange: (value: string) => void;
  onAmountChange: (value: string) => void;
  onTypeChange: (value: MovementType) => void;
  onSubmit: () => void;
  onCancel: () => void;
};

const typeOptions = [
  { value: MovementType.Income, label: "Ingreso" },
  { value: MovementType.Expense, label: "Gasto" },
];

export function AddMonthMovementForm({
  name,
  amount,
  type,
  submitting,
  errorMessage,
  validationMessage,
  onNameChange,
  onAmountChange,
  onTypeChange,
  onSubmit,
  onCancel,
}: AddMonthMovementFormProps) {
  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onSubmit();
  };

  return (
    <Box
      component="form"
      data-testid="add-month-movement-form"
      onSubmit={handleSubmit}
      sx={{
        mb: 3,
        p: { xs: 2.5, md: 3 },
        border: 1,
        borderColor: "divider",
        borderRadius: 3,
        bgcolor: "background.paper",
        boxShadow: "0 16px 28px rgba(15, 23, 42, 0.08)",
      }}
    >
      <Stack spacing={3}>
        {validationMessage ? (
          <Alert severity="warning">{validationMessage}</Alert>
        ) : null}
        {errorMessage ? <Alert severity="error">{errorMessage}</Alert> : null}
        <TextField
          label="Nombre"
          value={name}
          onChange={(event) => onNameChange(event.target.value)}
          disabled={submitting}
          required
          fullWidth
        />
        <TextField
          label="Cantidad"
          type="number"
          value={amount}
          onChange={(event) => onAmountChange(event.target.value)}
          disabled={submitting}
          required
          fullWidth
          inputProps={{ step: "0.01" }}
        />
        <FormControl fullWidth required disabled={submitting}>
          <InputLabel id="month-movement-type-label">Tipo</InputLabel>
          <Select
            labelId="month-movement-type-label"
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
        <Box sx={{ display: "flex", justifyContent: "flex-end", gap: 2 }}>
          <Button variant="outlined" onClick={onCancel} disabled={submitting}>
            Cancelar
          </Button>
          <Button
            type="submit"
            variant="contained"
            disabled={submitting}
            startIcon={submitting ? <CircularProgress size={18} /> : null}
          >
            {submitting ? "Creando" : "Aceptar"}
          </Button>
        </Box>
      </Stack>
    </Box>
  );
}
