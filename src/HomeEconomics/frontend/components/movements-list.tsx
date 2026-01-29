"use client";

import {
  Box,
  Button,
  Chip,
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
  deleting: boolean;
  onDeleteRequest: (id: number, name: string) => void;
  onEditRequest: (id: number) => void;
};

const getTypeColor = (type: MovementType) =>
  type === MovementType.Income ? "success" : "error";

export function MovementsList({
  movements,
  deleting,
  onDeleteRequest,
  onEditRequest,
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
                <Stack direction="row" spacing={1}>
                  <Button
                    variant="outlined"
                    size="small"
                    disabled={deleting}
                    onClick={() => onEditRequest(movement.id)}
                  >
                    Editar
                  </Button>
                  <Button
                    variant="outlined"
                    color="error"
                    size="small"
                    disabled={deleting}
                    onClick={() => onDeleteRequest(movement.id, movement.name)}
                  >
                    Eliminar
                  </Button>
                </Stack>
              </Stack>
            </Stack>
          </Stack>
        </ListItem>
      ))}
    </List>
  );
}
