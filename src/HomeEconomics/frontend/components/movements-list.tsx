"use client";

import {
  Box,
  Button,
  Chip,
  CircularProgress,
  List,
  ListItem,
  ListItemText,
  Stack,
  Typography,
} from "@mui/material";
import { MovementType } from "../types/movement-type";

type MovementListItem = {
  id: number;
  name: string;
  amount: string;
  type: MovementType;
  typeLabel: string;
  frequencyLabel: string;
};

type MovementsListProps = {
  movements: MovementListItem[];
  deletingId: number | null;
  deleting: boolean;
  onDelete: (id: number) => Promise<void>;
};

const getTypeColor = (type: MovementType) =>
  type === MovementType.Income ? "success" : "error";

export function MovementsList({
  movements,
  deletingId,
  deleting,
  onDelete,
}: MovementsListProps) {
  return (
    <List sx={{ bgcolor: "background.paper" }}>
      {movements.map((movement) => (
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
                <Button
                  variant="outlined"
                  color="error"
                  size="small"
                  disabled={deleting}
                  onClick={() => onDelete(movement.id)}
                  startIcon={
                    deletingId === movement.id ? (
                      <CircularProgress size={16} />
                    ) : null
                  }
                >
                  {deletingId === movement.id ? "Eliminando" : "Eliminar"}
                </Button>
              </Stack>
            </Stack>
          </Stack>
        </ListItem>
      ))}
    </List>
  );
}
