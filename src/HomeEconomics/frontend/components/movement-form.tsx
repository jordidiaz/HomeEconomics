"use client";

import type { FormEvent } from "react";
import {
  Alert,
  Box,
  Button,
  Checkbox,
  CircularProgress,
  FormControl,
  InputLabel,
  ListItemText,
  MenuItem,
  Select,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { FrequencyType } from "../types/frequency-type";
import { MovementType } from "../types/movement-type";

type MovementFormProps = {
  name: string;
  amount: string;
  type: MovementType;
  frequencyType: FrequencyType;
  frequencyMonth: number;
  customMonths: number[];
  isEditing: boolean;
  submitting: boolean;
  errorMessage: string | null;
  validationMessage: string | null;
  onNameChange: (value: string) => void;
  onAmountChange: (value: string) => void;
  onTypeChange: (value: MovementType) => void;
  onFrequencyTypeChange: (value: FrequencyType) => void;
  onFrequencyMonthChange: (value: number) => void;
  onCustomMonthsChange: (value: number[]) => void;
  onSubmit: () => void;
};

const monthOptions = [
  { value: 1, label: "Enero" },
  { value: 2, label: "Febrero" },
  { value: 3, label: "Marzo" },
  { value: 4, label: "Abril" },
  { value: 5, label: "Mayo" },
  { value: 6, label: "Junio" },
  { value: 7, label: "Julio" },
  { value: 8, label: "Agosto" },
  { value: 9, label: "Septiembre" },
  { value: 10, label: "Octubre" },
  { value: 11, label: "Noviembre" },
  { value: 12, label: "Diciembre" },
];

const frequencyOptions = [
  { value: FrequencyType.None, label: "Ninguna" },
  { value: FrequencyType.Monthly, label: "Mensual" },
  { value: FrequencyType.Yearly, label: "Anual" },
  { value: FrequencyType.Custom, label: "Personalizada" },
];

const typeOptions = [
  { value: MovementType.Income, label: "Ingreso" },
  { value: MovementType.Expense, label: "Gasto" },
];

const renderCustomMonthsValue = (selected: number[]) =>
  selected
    .map((month) => monthOptions.find((option) => option.value === month)?.label)
    .filter((label): label is string => Boolean(label))
    .join(", ");

export function MovementForm({
  name,
  amount,
  type,
  frequencyType,
  frequencyMonth,
  customMonths,
  isEditing,
  submitting,
  errorMessage,
  validationMessage,
  onNameChange,
  onAmountChange,
  onTypeChange,
  onFrequencyTypeChange,
  onFrequencyMonthChange,
  onCustomMonthsChange,
  onSubmit,
}: MovementFormProps) {
  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onSubmit();
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{
        mb: 4,
        p: 3,
        border: 1,
        borderColor: "divider",
        borderRadius: 2,
        bgcolor: "background.paper",
      }}
    >
      <Stack spacing={3}>
        <Typography component="h2" variant="h4">
          {isEditing ? "Editar movimiento" : "Crear movimiento"}
        </Typography>
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
          <InputLabel id="movement-type-label">Tipo</InputLabel>
          <Select
            labelId="movement-type-label"
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
        <FormControl fullWidth required disabled={submitting}>
          <InputLabel id="movement-frequency-label">Frecuencia</InputLabel>
          <Select
            labelId="movement-frequency-label"
            value={frequencyType === FrequencyType.Undefined ? "" : frequencyType}
            label="Frecuencia"
            onChange={(event) =>
              onFrequencyTypeChange(Number(event.target.value))
            }
          >
            {frequencyOptions.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
        {frequencyType === FrequencyType.Yearly ? (
          <FormControl fullWidth required disabled={submitting}>
            <InputLabel id="movement-month-label">Mes</InputLabel>
            <Select
              labelId="movement-month-label"
              value={frequencyMonth}
              label="Mes"
              onChange={(event) =>
                onFrequencyMonthChange(Number(event.target.value))
              }
            >
              {monthOptions.map((option) => (
                <MenuItem key={option.value} value={option.value}>
                  {option.label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        ) : null}
        {frequencyType === FrequencyType.Custom ? (
          <FormControl fullWidth required disabled={submitting}>
            <InputLabel id="movement-custom-months-label">Meses</InputLabel>
            <Select
              labelId="movement-custom-months-label"
              multiple
              value={customMonths}
              label="Meses"
              onChange={(event) =>
                onCustomMonthsChange(event.target.value as number[])
              }
              renderValue={renderCustomMonthsValue}
            >
              {monthOptions.map((option) => (
                <MenuItem key={option.value} value={option.value}>
                  <Checkbox checked={customMonths.includes(option.value)} />
                  <ListItemText primary={option.label} />
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        ) : null}
        <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
          <Button
            type="submit"
            variant="contained"
            disabled={submitting}
            startIcon={submitting ? <CircularProgress size={18} /> : null}
          >
            {submitting
              ? isEditing
                ? "Guardando"
                : "Creando"
              : isEditing
                ? "Guardar cambios"
                : "Crear movimiento"}
          </Button>
        </Box>
      </Stack>
    </Box>
  );
}
