import { useCallback, useState } from "react";
import { MovementsService } from "../services/movements-service";

type UseDeleteMovementOptions = {
  onDeleted?: () => Promise<void> | void;
};

type UseDeleteMovementResult = {
  deletingId: number | null;
  deleting: boolean;
  errorMessage: string | null;
  deleteMovement: (id: number) => Promise<boolean>;
  clearError: () => void;
};

export function useDeleteMovement(
  options: UseDeleteMovementOptions = {},
): UseDeleteMovementResult {
  const [deletingId, setDeletingId] = useState<number | null>(null);
  const [deleting, setDeleting] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const clearError = useCallback(() => {
    setErrorMessage(null);
  }, []);

  const deleteMovement = useCallback(
    async (id: number) => {
      setErrorMessage(null);
      setDeleting(true);
      setDeletingId(id);
      try {
        await MovementsService.delete(id);
        if (options.onDeleted) {
          await options.onDeleted();
        }
        return true;
      } catch (error) {
        setErrorMessage(
          "No se pudo eliminar el movimiento. Por favor, inténtalo de nuevo.",
        );
        return false;
      } finally {
        setDeleting(false);
        setDeletingId(null);
      }
    },
    [options],
  );

  return { deletingId, deleting, errorMessage, deleteMovement, clearError };
}
