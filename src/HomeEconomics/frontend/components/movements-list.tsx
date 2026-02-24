"use client";

import React from "react";
import {
  Box,
  Chip,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Stack,
  Tooltip,
  Typography,
} from "@mui/material";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import EditOutlinedIcon from "@mui/icons-material/EditOutlined";
import { MovementType } from "../types/movement-type";

type MovementListItem = {
  id: number;
  name: string;
  amount: string;
  type: MovementType;
  typeLabel: string;
  frequencyLabel: string;
};

type MovementAddActionState = {
  loading: boolean;
  errorMessage: string | null;
};

type MovementsListProps = {
  movements: MovementListItem[];
  deleting: boolean;
  addDisabled: boolean;
  addActionStates: Record<number, MovementAddActionState>;
  onAddToCurrentMonth: (id: number) => void;
  onDeleteRequest: (id: number, name: string) => void;
  onEditRequest: (id: number) => void;
};

const getTypeColor = (type: MovementType) =>
  type === MovementType.Income ? "success" : "error";

export function MovementsList({
  movements,
  deleting,
  addDisabled,
  addActionStates,
  onAddToCurrentMonth,
  onDeleteRequest,
  onEditRequest,
}: MovementsListProps) {
  return (
    <List
      data-testid="movements-list"
      sx={{
        p: 0,
        display: "flex",
        flexDirection: "column",
        gap: 2,
        bgcolor: "transparent",
      }}
    >
      {movements.map((movement) => {
        const actionState = addActionStates[movement.id] ?? {
          loading: false,
          errorMessage: null,
        };

        return (
          <ListItem
            key={movement.id}
            data-testid={`movement-item-${movement.name}`}
            sx={{
              p: { xs: 2, md: 2.5 },
              border: 1,
              borderColor: "divider",
              borderRadius: 3,
              alignItems: "flex-start",
              bgcolor: "background.paper",
              boxShadow: "0 12px 24px rgba(15, 23, 42, 0.08)",
              transition: "transform 0.2s ease, box-shadow 0.2s ease",
              "&:hover": {
                boxShadow: "0 18px 32px rgba(15, 23, 42, 0.12)",
                transform: "translateY(-1px)",
              },
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
                    <Typography variant="body2" color="text.secondary">
                      {movement.frequencyLabel}
                    </Typography>
                  }
                />
                <Stack
                  spacing={1}
                  alignItems="flex-end"
                  sx={{ ml: "auto", textAlign: "right" }}
                >
                  <Typography
                    variant="h6"
                    sx={{
                      fontWeight: 700,
                      color:
                        movement.type === MovementType.Income
                          ? "success.main"
                          : "error.main",
                    }}
                  >
                    {movement.amount}
                  </Typography>
                  <Chip
                    label={movement.typeLabel}
                    color={getTypeColor(movement.type)}
                    size="small"
                  />
                  <Stack spacing={0.5} alignItems="flex-end">
                    <Stack direction="row" spacing={1}>
                      <Tooltip title="Agregar al mes actual">
                        <span>
                          <IconButton
                            size="small"
                            aria-label="Agregar al mes actual"
                            data-testid={`movement-add-to-month-${movement.name}`}
                            disabled={addDisabled || actionState.loading}
                            onClick={() => onAddToCurrentMonth(movement.id)}
                          >
                            <AddCircleOutlineIcon fontSize="small" />
                          </IconButton>
                        </span>
                      </Tooltip>
                      <Tooltip title="Editar">
                        <span>
                          <IconButton
                            size="small"
                            aria-label="Editar"
                            data-testid={`movement-edit-${movement.name}`}
                            disabled={deleting}
                            onClick={() => onEditRequest(movement.id)}
                          >
                            <EditOutlinedIcon fontSize="small" />
                          </IconButton>
                        </span>
                      </Tooltip>
                      <Tooltip title="Eliminar">
                        <span>
                          <IconButton
                            size="small"
                            aria-label="Eliminar"
                            data-testid={`movement-delete-${movement.name}`}
                            disabled={deleting}
                            onClick={() => onDeleteRequest(movement.id, movement.name)}
                          >
                            <DeleteOutlineIcon fontSize="small" />
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
