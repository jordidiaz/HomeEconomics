"use client";

import React from "react";
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
  creatingNextMonth: boolean;
  createNextMonthErrorMessage: string | null;
  creatingCurrentMonth: boolean;
  createCurrentMonthErrorMessage: string | null;
  showCreateMonth: boolean;
  showCreateNextMonth: boolean;
  onSelect: (value: "current" | "next") => void;
  onCreateNextMonth: () => void;
  onCreateMonth: () => void;
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
  creatingNextMonth,
  createNextMonthErrorMessage,
  creatingCurrentMonth,
  createCurrentMonthErrorMessage,
  showCreateMonth,
  showCreateNextMonth,
  onSelect,
  onCreateNextMonth,
  onCreateMonth,
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
          sx={{ width: "fit-content" }}
        >
          <ToggleButton value="current" data-testid="month-selector-current" disabled={disabled}>
            <Stack spacing={0.5} alignItems="center">
              <Typography variant="subtitle2">
                {formatMonthYear(currentMonth.month, currentMonth.year)}
              </Typography>
            </Stack>
          </ToggleButton>
          <ToggleButton
            value="next"
            data-testid="month-selector-next"
            disabled={disabled || !nextMonthAvailable}
          >
            <Stack spacing={0.5} alignItems="center">
              <Typography variant="subtitle2">
                {formatMonthYear(nextMonth.month, nextMonth.year)}
              </Typography>
            </Stack>
          </ToggleButton>
        </ToggleButtonGroup>
        {showCreateMonth ? (
          <Button
            variant="outlined"
            data-testid="create-current-month"
            disabled={disabled || creatingCurrentMonth}
            onClick={onCreateMonth}
            startIcon={creatingCurrentMonth ? <CircularProgress size={18} /> : null}
          >
            {creatingCurrentMonth ? "Creando mes actual" : "Crear mes actual"}
          </Button>
        ) : null}
        {showCreateNextMonth ? (
          <Button
            variant="outlined"
            data-testid="create-next-month"
            disabled={disabled || creatingNextMonth}
            onClick={onCreateNextMonth}
            startIcon={creatingNextMonth ? <CircularProgress size={18} /> : null}
          >
            {creatingNextMonth ? "Creando mes siguiente" : "Crear mes siguiente"}
          </Button>
        ) : null}
        {createCurrentMonthErrorMessage ? (
          <Alert severity="error">{createCurrentMonthErrorMessage}</Alert>
        ) : null}
        {createNextMonthErrorMessage ? (
          <Alert severity="error">{createNextMonthErrorMessage}</Alert>
        ) : null}
      </Stack>
    </Box>
  );
}
