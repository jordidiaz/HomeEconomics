"use client";

import { Alert, Box, Stack, TextField, Typography } from "@mui/material";

type MovementMonthStatusFormProps = {
  accountAmount: string;
  cashAmount: string;
  balance: number;
  loading: boolean;
  errorMessage: string | null;
  successMessage?: string | null;
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
  successMessage,
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
          data-testid="status-account-input"
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
          data-testid="status-cash-input"
          value={cashAmount}
          onChange={(event) => onCashAmountChange(event.target.value)}
          onBlur={onBlur}
          disabled={loading}
          fullWidth
          type="number"
          inputProps={{ step: "0.01" }}
        />
        <Box data-testid="status-balance" sx={{ mt: 0.5 }}>
          <Typography
            variant="subtitle1"
            color="text.secondary"
            sx={{ fontWeight: 700, fontSize: { xs: "1.05rem", md: "1.15rem" } }}
          >
            Balance
          </Typography>
          <Typography
            variant="h5"
            color="text.primary"
            sx={{ fontWeight: 700, fontSize: { xs: "1.55rem", md: "1.9rem" }, lineHeight: 1.15 }}
          >
            {formatAmount(balance)}
          </Typography>
        </Box>
        {successMessage ? (
          <Alert severity="success" data-testid="status-success">
            {successMessage}
          </Alert>
        ) : null}
        {errorMessage ? <Alert severity="error">{errorMessage}</Alert> : null}
      </Stack>
    </Box>
  );
}
