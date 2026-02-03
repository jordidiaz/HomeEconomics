"use client";

import { Chip, List, ListItem, ListItemText, Stack, Typography } from "@mui/material";
import { MovementType } from "../types/movement-type";

type MonthMovementListItem = {
  id: number;
  name: string;
  amount: string;
  type: MovementType;
  typeLabel: string;
  paid: boolean;
  paidLabel: string;
};

type CurrentMonthMovementsListProps = {
  movements: MonthMovementListItem[];
  showPaid: boolean;
};

const getTypeColor = (type: MovementType) =>
  type === MovementType.Income ? "success" : "error";

const getPaidColor = (paid: boolean) => (paid ? "success" : "warning");

export function CurrentMonthMovementsList({
  movements,
  showPaid,
}: CurrentMonthMovementsListProps) {
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
              <Typography
                variant="h6"
                sx={{
                  ml: "auto",
                  color:
                    movement.type === MovementType.Income
                      ? "success.main"
                      : "error.main",
                }}
              >
                {movement.amount}
              </Typography>
            </Stack>
          </Stack>
        </ListItem>
      ))}
    </List>
  );
}
