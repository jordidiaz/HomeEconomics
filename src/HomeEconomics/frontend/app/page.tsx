"use client";

import {
  Alert,
  Box,
  CircularProgress,
  FormControlLabel,
  Switch,
  Typography,
} from "@mui/material";
import { useState } from "react";
import { ConfirmDeleteMovementDialog } from "../components/confirm-delete-movement-dialog";
import { CurrentMonthMovementsList } from "../components/current-month-movements-list";
import { MonthMovementMonthSelector } from "../components/month-movement-month-selector";
import { MovementForm } from "../components/movement-form";
import { MovementsList } from "../components/movements-list";
import { useCurrentMonthMovements } from "../hooks/use-current-month-movements";
import { useDeleteMovement } from "../hooks/use-delete-movement";
import { useMovementForm } from "../hooks/use-movement-form";
import { useMovements } from "../hooks/use-movements";

export default function HomePage() {
  const currentMonthMovements = useCurrentMonthMovements();
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
        <Box sx={{ flex: 1, minWidth: 0 }}>
          <MonthMovementMonthSelector
            currentMonth={currentMonthMovements.currentMonth}
            nextMonth={currentMonthMovements.nextMonth}
            selectedMonth={currentMonthMovements.selectedMonth}
            nextMonthAvailable={currentMonthMovements.nextMonthAvailable}
            disabled={currentMonthMovements.loading}
            creating={currentMonthMovements.creatingNextMonth}
            createErrorMessage={currentMonthMovements.createNextMonthErrorMessage}
            showCreateNextMonth={
              currentMonthMovements.movementMonthLoaded &&
              !currentMonthMovements.nextMonthAvailable
            }
            onSelect={currentMonthMovements.selectMonth}
            onCreateNextMonth={currentMonthMovements.createNextMonth}
          />
          {currentMonthMovements.loading ? (
            <Box sx={{ display: "flex", justifyContent: "center", py: 6 }}>
              <CircularProgress />
            </Box>
          ) : null}
          {!currentMonthMovements.loading && currentMonthMovements.error ? (
            <Alert severity="error">
              No se pudieron cargar los movimientos del mes. Por favor, inténtalo de nuevo.
            </Alert>
          ) : null}
          {!currentMonthMovements.loading &&
          !currentMonthMovements.error &&
          currentMonthMovements.totalMonthMovements > 0 ? (
            <FormControlLabel
              sx={{ mb: 2 }}
              control={
                <Switch
                  checked={currentMonthMovements.showPaid}
                  onChange={(event) =>
                    currentMonthMovements.setShowPaid(event.target.checked)
                  }
                />
              }
              label="Mostrar pagados"
            />
          ) : null}
          {!currentMonthMovements.loading &&
          !currentMonthMovements.error &&
          currentMonthMovements.totalMonthMovements === 0 ? (
            <Alert severity="info">
              No hay movimientos registrados para el mes actual.
            </Alert>
          ) : null}
          {!currentMonthMovements.loading &&
          !currentMonthMovements.error &&
          currentMonthMovements.totalMonthMovements > 0 &&
          currentMonthMovements.monthMovements.length === 0 ? (
            <Alert severity="info">
              {currentMonthMovements.showPaid
                ? "No hay movimientos pagados para el mes actual."
                : "No hay movimientos pendientes para el mes actual."}
            </Alert>
          ) : null}
          {!currentMonthMovements.loading &&
          !currentMonthMovements.error &&
          currentMonthMovements.monthMovements.length > 0 ? (
            <CurrentMonthMovementsList
              movements={currentMonthMovements.monthMovements}
              showPaid={currentMonthMovements.showPaid}
              actionStates={currentMonthMovements.actionStates}
              onPay={currentMonthMovements.payMonthMovement}
              onUnpay={currentMonthMovements.unpayMonthMovement}
            />
          ) : null}
        </Box>
        <Box sx={{ flex: 1, minWidth: 0 }}>
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
