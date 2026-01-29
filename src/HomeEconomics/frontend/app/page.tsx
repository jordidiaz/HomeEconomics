"use client";

import { Alert, Box, CircularProgress, Typography } from "@mui/material";
import { MovementForm } from "../components/movement-form";
import { MovementsList } from "../components/movements-list";
import { useCreateMovement } from "../hooks/use-create-movement";
import { useDeleteMovement } from "../hooks/use-delete-movement";
import { useMovements } from "../hooks/use-movements";

export default function HomePage() {
  const { movements, loading, error, reload } = useMovements();
  const createMovement = useCreateMovement({ onCreated: reload });
  const deleteMovement = useDeleteMovement({ onDeleted: reload });

  return (
    <Box sx={{ px: 4, py: 6 }}>
      <Typography component="h1" variant="h1" sx={{ mb: 4 }}>
        Movimientos
      </Typography>
      <MovementForm
        name={createMovement.name}
        amount={createMovement.amount}
        type={createMovement.type}
        frequencyType={createMovement.frequencyType}
        frequencyMonth={createMovement.frequencyMonth}
        customMonths={createMovement.customMonths}
        submitting={createMovement.submitting}
        errorMessage={createMovement.errorMessage}
        validationMessage={createMovement.validationMessage}
        onNameChange={createMovement.setName}
        onAmountChange={createMovement.setAmount}
        onTypeChange={createMovement.setType}
        onFrequencyTypeChange={createMovement.setFrequencyType}
        onFrequencyMonthChange={createMovement.setFrequencyMonth}
        onCustomMonthsChange={createMovement.setCustomMonths}
        onSubmit={createMovement.submit}
      />
      {loading ? (
        <Box sx={{ display: "flex", justifyContent: "center", py: 6 }}>
          <CircularProgress />
        </Box>
      ) : null}
      {!loading && error ? (
        <Alert severity="error">
          No se pudieron cargar los movimientos. Por favor, inténtalo de nuevo.
        </Alert>
      ) : null}
      {!loading && !error && movements.length === 0 ? (
        <Alert severity="info">No hay movimientos disponibles.</Alert>
      ) : null}
      {deleteMovement.errorMessage ? (
        <Alert severity="error" sx={{ mt: 2 }}>
          {deleteMovement.errorMessage}
        </Alert>
      ) : null}
      {!loading && !error && movements.length > 0 ? (
        <MovementsList
          movements={movements}
          deleting={deleteMovement.deleting}
          deletingId={deleteMovement.deletingId}
          onDelete={deleteMovement.deleteMovement}
        />
      ) : null}
    </Box>
  );
}
