"use client";

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
    <List sx={{ bgcolor: "background.paper" }}>
      {movements.map((movement) => {
        const actionState = addActionStates[movement.id] ?? {
          loading: false,
          errorMessage: null,
        };

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
