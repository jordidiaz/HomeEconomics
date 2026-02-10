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
import { ConfirmDeleteMonthMovementDialog } from "../components/confirm-delete-month-movement-dialog";
import { ConfirmDeleteMovementDialog } from "../components/confirm-delete-movement-dialog";
import { ConfirmMoveMonthMovementDialog } from "../components/confirm-move-month-movement-dialog";
import { CurrentMonthMovementsList } from "../components/current-month-movements-list";
import { EditMonthMovementAmountDialog } from "../components/edit-month-movement-amount-dialog";
import { AddMonthMovementForm } from "../components/add-month-movement-form";
import { MovementMonthStatusForm } from "../components/movement-month-status-form";
import { MonthMovementMonthSelector } from "../components/month-movement-month-selector";
import { MovementForm } from "../components/movement-form";
import { MovementsList } from "../components/movements-list";
import { useAddMonthMovementForm } from "../hooks/use-add-month-movement-form";
import { useCurrentMonthMovements } from "../hooks/use-current-month-movements";
import { useDeleteMovement } from "../hooks/use-delete-movement";
import { useAddMovementToCurrentMonth } from "../hooks/use-add-movement-to-current-month";
import { useMovementForm } from "../hooks/use-movement-form";
import { useMovements } from "../hooks/use-movements";

export default function HomePage() {
  const currentMonthMovements = useCurrentMonthMovements();
  const { movements, movementMap, loading, error, reload } = useMovements();
  const movementForm = useMovementForm({ onSaved: reload });
  const deleteMovement = useDeleteMovement({ onDeleted: reload });
  const addMovementToCurrentMonth = useAddMovementToCurrentMonth({
    movementMonthId: currentMonthMovements.currentMovementMonthId,
    onAdded: currentMonthMovements.reloadCurrentMonthMovements,
  });
  const addMonthMovementForm = useAddMonthMovementForm({
    movementMonthId: currentMonthMovements.currentMovementMonthId,
    onAdded: currentMonthMovements.reloadCurrentMonthMovements,
  });
  const [movementToDelete, setMovementToDelete] = useState<{
    id: number;
    name: string;
  } | null>(null);
  const [monthMovementToDelete, setMonthMovementToDelete] = useState<{
    id: number;
    name: string;
  } | null>(null);
  const [monthMovementToMove, setMonthMovementToMove] = useState<{
    id: number;
    name: string;
  } | null>(null);
  const [movementToEditAmount, setMovementToEditAmount] = useState<{
    id: number;
    name: string;
    amountValue: number;
  } | null>(null);
  const [editAmountValue, setEditAmountValue] = useState("");

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

  const handleMonthMovementDeleteRequest = (movementId: number) => {
    const movement = currentMonthMovements.monthMovements.find(
      (monthMovement) => monthMovement.id === movementId,
    );
    if (!movement) {
      return;
    }
    currentMonthMovements.setDeleteTarget(movementId);
    setMonthMovementToDelete({ id: movementId, name: movement.name });
  };

  const handleMonthMovementDeleteCancel = () => {
    currentMonthMovements.setDeleteTarget(null);
    setMonthMovementToDelete(null);
  };

  const handleMonthMovementDeleteConfirm = async () => {
    if (!monthMovementToDelete) {
      return;
    }
    const wasDeleted = await currentMonthMovements.deleteMonthMovement(
      monthMovementToDelete.id,
    );
    if (wasDeleted) {
      handleMonthMovementDeleteCancel();
    }
  };

  const handleMonthMovementMoveRequest = (movementId: number) => {
    const movement = currentMonthMovements.monthMovements.find(
      (monthMovement) => monthMovement.id === movementId,
    );
    if (!movement) {
      return;
    }
    currentMonthMovements.setMoveTarget(movementId);
    setMonthMovementToMove({ id: movementId, name: movement.name });
  };

  const handleMonthMovementMoveCancel = () => {
    currentMonthMovements.setMoveTarget(null);
    setMonthMovementToMove(null);
  };

  const handleMonthMovementMoveConfirm = async () => {
    if (!monthMovementToMove) {
      return;
    }
    const wasMoved = await currentMonthMovements.moveMonthMovementToNextMonth(
      monthMovementToMove.id,
    );
    if (wasMoved) {
      handleMonthMovementMoveCancel();
    }
  };

  const handleEditRequest = (id: number) => {
    const movement = movementMap[id];
    if (!movement) {
      return;
    }
    movementForm.startEdit(movement);
  };

  const handleAddToCurrentMonth = (id: number) => {
    const movement = movementMap[id];
    if (!movement) {
      return;
    }
    void addMovementToCurrentMonth.addToCurrentMonth(movement);
  };

  const handleEditAmountRequest = (movement: {
    id: number;
    name: string;
    amountValue: number;
  }) => {
    currentMonthMovements.setAmountUpdateTarget(movement.id);
    setMovementToEditAmount(movement);
    setEditAmountValue(movement.amountValue.toFixed(2));
  };

  const handleEditAmountCancel = () => {
    currentMonthMovements.setAmountUpdateTarget(null);
    setMovementToEditAmount(null);
    setEditAmountValue("");
  };

  const handleEditAmountAccept = async () => {
    if (!movementToEditAmount) {
      return;
    }
    const wasUpdated = await currentMonthMovements.updateMonthMovementAmount(
      movementToEditAmount.id,
      editAmountValue,
    );
    if (wasUpdated) {
      handleEditAmountCancel();
    }
  };

  return (
    <Box sx={{ px: 4, py: 6 }}>
      <Box sx={{ display: "flex", gap: 4 }}>
        <Box sx={{ flex: 1, minWidth: 0 }}>
          {currentMonthMovements.status ? (
            <MovementMonthStatusForm
              status={currentMonthMovements.status}
              loading={currentMonthMovements.loading}
            />
          ) : null}
          <MonthMovementMonthSelector
            currentMonth={currentMonthMovements.currentMonth}
            nextMonth={currentMonthMovements.nextMonth}
            selectedMonth={currentMonthMovements.selectedMonth}
            nextMonthAvailable={currentMonthMovements.nextMonthAvailable}
            disabled={currentMonthMovements.loading}
            creatingNextMonth={currentMonthMovements.creatingNextMonth}
            createNextMonthErrorMessage={currentMonthMovements.createNextMonthErrorMessage}
            creatingCurrentMonth={currentMonthMovements.creatingCurrentMonth}
            createCurrentMonthErrorMessage={
              currentMonthMovements.createCurrentMonthErrorMessage
            }
            showCreateMonth={!currentMonthMovements.currentMonthAvailable}
            showCreateNextMonth={
              currentMonthMovements.movementMonthLoaded &&
              !currentMonthMovements.nextMonthAvailable
            }
            onSelect={currentMonthMovements.selectMonth}
            onCreateNextMonth={currentMonthMovements.createNextMonth}
            onCreateMonth={currentMonthMovements.createCurrentMonth}
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
          currentMonthMovements.selectedMonth === "current" &&
          currentMonthMovements.currentMovementMonthId !== null ? (
            <AddMonthMovementForm
              name={addMonthMovementForm.name}
              amount={addMonthMovementForm.amount}
              type={addMonthMovementForm.type}
              submitting={addMonthMovementForm.submitting}
              errorMessage={addMonthMovementForm.errorMessage}
              validationMessage={addMonthMovementForm.validationMessage}
              onNameChange={addMonthMovementForm.setName}
              onAmountChange={addMonthMovementForm.setAmount}
              onTypeChange={addMonthMovementForm.setType}
              onSubmit={addMonthMovementForm.submit}
              onCancel={addMonthMovementForm.cancel}
            />
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
              amountUpdateState={currentMonthMovements.amountUpdateState}
              deleteState={currentMonthMovements.deleteState}
              moveState={currentMonthMovements.moveState}
              nextMovementMonthExists={currentMonthMovements.nextMovementMonthExists}
              onPay={currentMonthMovements.payMonthMovement}
              onUnpay={currentMonthMovements.unpayMonthMovement}
              onEditAmount={handleEditAmountRequest}
              onDelete={handleMonthMovementDeleteRequest}
              onMoveToNextMonth={handleMonthMovementMoveRequest}
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
              addDisabled={
                !currentMonthMovements.currentMonthAvailable ||
                currentMonthMovements.currentMovementMonthId === null
              }
              addActionStates={addMovementToCurrentMonth.actionStates}
              onAddToCurrentMonth={handleAddToCurrentMonth}
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
      <EditMonthMovementAmountDialog
        open={movementToEditAmount !== null}
        amount={editAmountValue}
        submitting={currentMonthMovements.amountUpdateState.loading}
        errorMessage={currentMonthMovements.amountUpdateState.errorMessage}
        onAmountChange={setEditAmountValue}
        onCancel={handleEditAmountCancel}
        onAccept={handleEditAmountAccept}
      />
      <ConfirmDeleteMonthMovementDialog
        open={monthMovementToDelete !== null}
        deleting={currentMonthMovements.deleteState.loading}
        errorMessage={currentMonthMovements.deleteState.errorMessage}
        movementName={monthMovementToDelete?.name ?? ""}
        onCancel={handleMonthMovementDeleteCancel}
        onConfirm={handleMonthMovementDeleteConfirm}
      />
      <ConfirmMoveMonthMovementDialog
        open={monthMovementToMove !== null}
        moving={currentMonthMovements.moveState.loading}
        errorMessage={currentMonthMovements.moveState.errorMessage}
        movementName={monthMovementToMove?.name ?? ""}
        onCancel={handleMonthMovementMoveCancel}
        onConfirm={handleMonthMovementMoveConfirm}
      />
    </Box>
  );
}
