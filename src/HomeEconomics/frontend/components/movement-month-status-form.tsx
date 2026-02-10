"use client";

import { Box, Stack, TextField, Typography } from "@mui/material";

type MovementMonthStatus = {
  pendingTotalExpenses: number;
  pendingTotalIncomes: number;
  accountAmount: number;
  cashAmount: number;
};

type MovementMonthStatusFormProps = {
  status: MovementMonthStatus;
  loading: boolean;
};

const formatAmount = (amount: number): string =>
  new Intl.NumberFormat("es-ES", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount);

export function MovementMonthStatusForm({ status, loading }: MovementMonthStatusFormProps) {
  const balance =
    status.accountAmount +
    status.cashAmount -
    (status.pendingTotalExpenses - status.pendingTotalIncomes);

  return (
    <Box
      sx={{
        mb: 2,
        p: 3,
        border: 1,
        borderColor: "divider",
        borderRadius: 2,
        bgcolor: "background.paper",
      }}
    >
      <Stack spacing={2}>
        <TextField
          label="Dinero en cuenta"
          value={formatAmount(status.accountAmount)}
          InputProps={{ readOnly: true }}
          disabled={loading}
          fullWidth
        />
        <TextField
          label="Dinero en cash"
          value={formatAmount(status.cashAmount)}
          InputProps={{ readOnly: true }}
          disabled={loading}
          fullWidth
        />
        <Typography variant="body2" color="text.secondary">
          Balance: {formatAmount(balance)}
        </Typography>
      </Stack>
    </Box>
  );
}
