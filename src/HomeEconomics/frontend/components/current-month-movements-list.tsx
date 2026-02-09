"use client";

import {
  Chip,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Stack,
  Tooltip,
  Typography,
} from "@mui/material";
import AttachMoneyOutlinedIcon from "@mui/icons-material/AttachMoneyOutlined";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutlined";
import EditOutlinedIcon from "@mui/icons-material/EditOutlined";
import RedoOutlinedIcon from "@mui/icons-material/RedoOutlined";
import MoneyOffOutlinedIcon from "@mui/icons-material/MoneyOffOutlined";
import { MovementType } from "../types/movement-type";

type MonthMovementListItem = {
  id: number;
  name: string;
  amount: string;
  amountValue: number;
  type: MovementType;
  typeLabel: string;
  paid: boolean;
  paidLabel: string;
};

type MonthMovementActionState = {
  loading: boolean;
  errorMessage: string | null;
};

type MonthMovementAmountUpdateState = {
  loading: boolean;
  monthMovementId: number | null;
};

type MonthMovementDeleteState = {
  loading: boolean;
  monthMovementId: number | null;
};

type MonthMovementMoveState = {
  loading: boolean;
  monthMovementId: number | null;
};

type MonthMovementEditTarget = {
  id: number;
  name: string;
  amountValue: number;
};

type CurrentMonthMovementsListProps = {
  movements: MonthMovementListItem[];
  showPaid: boolean;
  actionStates: Record<number, MonthMovementActionState>;
  amountUpdateState: MonthMovementAmountUpdateState;
  deleteState: MonthMovementDeleteState;
  moveState: MonthMovementMoveState;
  nextMovementMonthExists: boolean;
  onPay: (monthMovementId: number) => Promise<void>;
  onUnpay: (monthMovementId: number) => Promise<void>;
  onEditAmount: (movement: MonthMovementEditTarget) => void;
  onDelete: (movementId: number) => void;
  onMoveToNextMonth: (movementId: number) => void;
};

const getTypeColor = (type: MovementType) =>
  type === MovementType.Income ? "success" : "error";

const getPaidColor = (paid: boolean) => (paid ? "success" : "warning");

export function CurrentMonthMovementsList({
  movements,
  showPaid,
  actionStates,
  amountUpdateState,
  deleteState,
  moveState,
  nextMovementMonthExists,
  onPay,
  onUnpay,
  onEditAmount,
  onDelete,
  onMoveToNextMonth,
}: CurrentMonthMovementsListProps) {
  return (
    <List sx={{ bgcolor: "background.paper" }}>
      {movements.map((movement) => {
        const actionState = actionStates[movement.id] ?? {
          loading: false,
          errorMessage: null,
        };
        const isAmountUpdateLoading =
          amountUpdateState.loading && amountUpdateState.monthMovementId === movement.id;
        const isDeleteLoading =
          deleteState.loading && deleteState.monthMovementId === movement.id;
        const isMoveLoading = moveState.loading && moveState.monthMovementId === movement.id;
        const disableActions =
          actionState.loading || isAmountUpdateLoading || isDeleteLoading || isMoveLoading;

        return (
          <ListItem
            key={movement.id}
            sx={{
              border: 1,
              borderColor: "divider",
              borderRadius: 2,
              mb: 2,
              alignItems: "flex-start",
            }}
          >
            <Stack spacing={1} sx={{ width: "100%" }}>
              <Stack direction="row" spacing={2} alignItems="center">
                <ListItemText
                  primary={
                    <Typography variant="h6" component="h2">
                      {movement.name}
                    </Typography>
                  }
                  secondary={
                    <Stack direction="row" spacing={1} sx={{ mt: 1 }}>
                      <Chip
                        label={movement.typeLabel}
                        color={getTypeColor(movement.type)}
                        size="small"
                      />
                      {showPaid ? (
                        <Chip
                          label={movement.paidLabel}
                          color={getPaidColor(movement.paid)}
                          size="small"
                          variant="outlined"
                        />
                      ) : null}
                    </Stack>
                  }
                />
                <Stack spacing={1} sx={{ ml: "auto", alignItems: "flex-end" }}>
                  <Typography
                    variant="h6"
                    sx={{
                      color:
                        movement.type === MovementType.Income
                          ? "success.main"
                          : "error.main",
                    }}
                  >
                    {movement.amount}
                  </Typography>
                  <Stack spacing={0.5} sx={{ alignItems: "flex-end" }}>
                    <Stack direction="row" spacing={0.5}>
                      {nextMovementMonthExists ? (
                        <Tooltip title="Mover al mes siguiente">
                          <span>
                            <IconButton
                              size="small"
                              aria-label="Mover al mes siguiente"
                              disabled={disableActions}
                              onClick={() => onMoveToNextMonth(movement.id)}
                            >
                              <RedoOutlinedIcon fontSize="small" />
                            </IconButton>
                          </span>
                        </Tooltip>
                      ) : null}
                      <Tooltip title="Editar importe">
                        <span>
                          <IconButton
                            size="small"
                            aria-label="Editar importe"
                            disabled={disableActions}
                            onClick={() =>
                              onEditAmount({
                                id: movement.id,
                                name: movement.name,
                                amountValue: movement.amountValue,
                              })
                            }
                          >
                            <EditOutlinedIcon fontSize="small" />
                          </IconButton>
                        </span>
                      </Tooltip>
                      {!movement.paid ? (
                        <Tooltip title="Eliminar">
                          <span>
                            <IconButton
                              size="small"
                              aria-label="Eliminar"
                              disabled={disableActions}
                              onClick={() => onDelete(movement.id)}
                            >
                              <DeleteOutlinedIcon fontSize="small" />
                            </IconButton>
                          </span>
                        </Tooltip>
                      ) : null}
                      <Tooltip
                        title={
                          movement.paid ? "Marcar como no pagado" : "Marcar como pagado"
                        }
                      >
                        <span>
                          <IconButton
                            size="small"
                            aria-label={
                              movement.paid
                                ? "Marcar como no pagado"
                                : "Marcar como pagado"
                            }
                            disabled={disableActions}
                            onClick={() =>
                              movement.paid
                                ? void onUnpay(movement.id)
                                : void onPay(movement.id)
                            }
                          >
                            {movement.paid ? (
                              <MoneyOffOutlinedIcon fontSize="small" />
                            ) : (
                              <AttachMoneyOutlinedIcon fontSize="small" />
                            )}
                          </IconButton>
                        </span>
                      </Tooltip>
                    </Stack>
                    {actionState.errorMessage ? (
                      <Typography variant="body2" color="error">
                        {actionState.errorMessage}
                      </Typography>
                    ) : null}
                  </Stack>
                </Stack>
              </Stack>
            </Stack>
          </ListItem>
        );
      })}
    </List>
  );
}
