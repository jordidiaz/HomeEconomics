"use client";

import { Alert, Box, Stack, TextField, Typography } from "@mui/material";

type MovementMonthStatusFormProps = {
  accountAmount: string;
  cashAmount: string;
  balance: number;
  loading: boolean;
  errorMessage: string | null;
  onAccountAmountChange: (value: string) => void;
  onCashAmountChange: (value: string) => void;
  onBlur: () => void;
};

const formatAmount = (amount: number): string =>
  new Intl.NumberFormat("es-ES", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount);

export function MovementMonthStatusForm({
  accountAmount,
  cashAmount,
  balance,
  loading,
  errorMessage,
  onAccountAmountChange,
  onCashAmountChange,
  onBlur,
}: MovementMonthStatusFormProps) {
  return (
    <Box
      sx={{
        mb: 2,
        p: { xs: 2.5, md: 3 },
        border: 1,
        borderColor: "divider",
        borderRadius: 3,
        bgcolor: "background.paper",
        boxShadow: "0 14px 24px rgba(15, 23, 42, 0.08)",
      }}
    >
      <Stack spacing={2}>
        <TextField
          label="Dinero en cuenta"
          value={accountAmount}
          onChange={(event) => onAccountAmountChange(event.target.value)}
          onBlur={onBlur}
          disabled={loading}
          fullWidth
          type="number"
          inputProps={{ step: "0.01" }}
        />
        <TextField
          label="Dinero en cash"
          value={cashAmount}
          onChange={(event) => onCashAmountChange(event.target.value)}
          onBlur={onBlur}
          disabled={loading}
          fullWidth
          type="number"
          inputProps={{ step: "0.01" }}
        />
        <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 600 }}>
          Balance: {formatAmount(balance)}
        </Typography>
        {errorMessage ? <Alert severity="error">{errorMessage}</Alert> : null}
      </Stack>
    </Box>
  );
}
