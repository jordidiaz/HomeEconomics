"use client";

import { Alert, Box, CircularProgress, Typography } from "@mui/material";
import { useState } from "react";
import { ConfirmDeleteMovementDialog } from "../components/confirm-delete-movement-dialog";
import { MovementForm } from "../components/movement-form";
import { MovementsList } from "../components/movements-list";
import { useDeleteMovement } from "../hooks/use-delete-movement";
import { useMovementForm } from "../hooks/use-movement-form";
import { useMovements } from "../hooks/use-movements";

export default function HomePage() {
  const { movements, movementMap, loading, error, reload } = useMovements();
  const movementForm = useMovementForm({ onSaved: reload });
  const deleteMovement = useDeleteMovement({ onDeleted: reload });
  const [movementToDelete, setMovementToDelete] = useState<{
    id: number;
    name: string;
  } | null>(null);

  const handleDeleteRequest = (id: number, name: string) => {
    deleteMovement.clearError();
    setMovementToDelete({ id, name });
  };

  const handleDeleteCancel = () => {
    deleteMovement.clearError();
    setMovementToDelete(null);
  };

  const handleDeleteConfirm = async () => {
    if (!movementToDelete) {
      return;
    }
    const wasDeleted = await deleteMovement.deleteMovement(movementToDelete.id);
    if (wasDeleted) {
      setMovementToDelete(null);
    }
  };

  const handleEditRequest = (id: number) => {
    const movement = movementMap[id];
    if (!movement) {
      return;
    }
    movementForm.startEdit(movement);
  };

  return (
    <Box sx={{ px: 4, py: 6 }}>
      <Box sx={{ display: "flex", gap: 4 }}>
        <Box sx={{ flex: 1 }} />
        <Box sx={{ flex: 1, minWidth: 0 }}>
          <Typography component="h1" variant="h1" sx={{ mb: 4 }}>
            Movimientos
          </Typography>
          <MovementForm
            name={movementForm.name}
            amount={movementForm.amount}
            type={movementForm.type}
            frequencyType={movementForm.frequencyType}
            frequencyMonth={movementForm.frequencyMonth}
            customMonths={movementForm.customMonths}
            isEditing={movementForm.isEditing}
            submitting={movementForm.submitting}
            errorMessage={movementForm.errorMessage}
            validationMessage={movementForm.validationMessage}
            onNameChange={movementForm.setName}
            onAmountChange={movementForm.setAmount}
            onTypeChange={movementForm.setType}
            onFrequencyTypeChange={movementForm.setFrequencyType}
            onFrequencyMonthChange={movementForm.setFrequencyMonth}
            onCustomMonthsChange={movementForm.setCustomMonths}
            onSubmit={movementForm.submit}
            onCancel={movementForm.cancel}
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
          {!loading && !error && movements.length > 0 ? (
            <MovementsList
              movements={movements}
              deleting={deleteMovement.deleting}
              onDeleteRequest={handleDeleteRequest}
              onEditRequest={handleEditRequest}
            />
          ) : null}
        </Box>
      </Box>
      <ConfirmDeleteMovementDialog
        open={movementToDelete !== null}
        deleting={deleteMovement.deleting}
        errorMessage={deleteMovement.errorMessage}
        movementName={movementToDelete?.name ?? ""}
        onCancel={handleDeleteCancel}
        onConfirm={handleDeleteConfirm}
      />
    </Box>
  );
}
