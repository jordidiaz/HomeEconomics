"use client";

import {
  Alert,
  Box,
  Button,
  CircularProgress,
  Stack,
  ToggleButton,
  ToggleButtonGroup,
  Typography,
} from "@mui/material";
import type { MouseEvent } from "react";

type MonthReference = {
  year: number;
  month: number;
};

type MonthMovementMonthSelectorProps = {
  currentMonth: MonthReference;
  nextMonth: MonthReference;
  selectedMonth: "current" | "next";
  nextMonthAvailable: boolean;
  disabled: boolean;
  creating: boolean;
  createErrorMessage: string | null;
  showCreateNextMonth: boolean;
  onSelect: (value: "current" | "next") => void;
  onCreateNextMonth: () => void;
};

const monthLabels = [
  "Enero",
  "Febrero",
  "Marzo",
  "Abril",
  "Mayo",
  "Junio",
  "Julio",
  "Agosto",
  "Septiembre",
  "Octubre",
  "Noviembre",
  "Diciembre",
];

const formatMonthYear = (month: number, year: number): string => {
  const label = monthLabels[month - 1] ?? "";
  return label ? `${label} ${year}` : `${year}`;
};

export function MonthMovementMonthSelector({
  currentMonth,
  nextMonth,
  selectedMonth,
  nextMonthAvailable,
  disabled,
  creating,
  createErrorMessage,
  showCreateNextMonth,
  onSelect,
  onCreateNextMonth,
}: MonthMovementMonthSelectorProps) {
  const handleChange = (
    _event: MouseEvent<HTMLElement>,
    value: "current" | "next" | null,
  ) => {
    if (value) {
      onSelect(value);
    }
  };

  return (
    <Box sx={{ mb: 2 }}>
      <Stack spacing={2}>
        <ToggleButtonGroup
          color="primary"
          value={selectedMonth}
          exclusive
          onChange={handleChange}
          disabled={disabled}
        >
          <ToggleButton value="current" disabled={disabled}>
            <Stack spacing={0.5} alignItems="center">
              <Typography variant="subtitle2">
                {formatMonthYear(currentMonth.month, currentMonth.year)}
              </Typography>
            </Stack>
          </ToggleButton>
          <ToggleButton value="next" disabled={disabled || !nextMonthAvailable}>
            <Stack spacing={0.5} alignItems="center">
              <Typography variant="subtitle2">
                {formatMonthYear(nextMonth.month, nextMonth.year)}
              </Typography>
            </Stack>
          </ToggleButton>
        </ToggleButtonGroup>
        {showCreateNextMonth ? (
          <Button
            variant="outlined"
            disabled={disabled || creating}
            onClick={onCreateNextMonth}
            startIcon={creating ? <CircularProgress size={18} /> : null}
          >
            {creating ? "Creando mes siguiente" : "Crear mes siguiente"}
          </Button>
        ) : null}
        {createErrorMessage ? <Alert severity="error">{createErrorMessage}</Alert> : null}
      </Stack>
    </Box>
  );
}
