"use client";

import { Alert, Box, CircularProgress, Typography } from "@mui/material";
import { MovementsList } from "../components/movements-list";
import { useMovements } from "../hooks/use-movements";

export default function HomePage() {
  const { movements, loading, error } = useMovements();

  return (
    <Box sx={{ px: 4, py: 6 }}>
      <Typography component="h1" variant="h1" sx={{ mb: 4 }}>
        Movements
      </Typography>
      {loading ? (
        <Box sx={{ display: "flex", justifyContent: "center", py: 6 }}>
          <CircularProgress />
        </Box>
      ) : null}
      {!loading && error ? (
        <Alert severity="error">
          Failed to load movements. Please try again.
        </Alert>
      ) : null}
      {!loading && !error && movements.length === 0 ? (
        <Alert severity="info">No movements available.</Alert>
      ) : null}
      {!loading && !error && movements.length > 0 ? (
        <MovementsList movements={movements} />
      ) : null}
    </Box>
  );
}
