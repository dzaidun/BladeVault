import { useState, useCallback } from "react";
import type { ApiError } from "../types/common";
import toast from "react-hot-toast";
import { API_ERROR_MESSAGES } from "../lib/constants";

export const useApi = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<ApiError | null>(null);

  const execute = useCallback(
    async <T,>(
      apiCall: () => Promise<T>,
      options: {
        showError?: boolean;
        showSuccess?: boolean;
        successMessage?: string;
      } = {}
    ): Promise<T | null> => {
      setIsLoading(true);
      setError(null);

      try {
        const result = await apiCall();

        if (options.showSuccess) {
          toast.success(options.successMessage || "Успішно!");
        }

        return result;
      } catch (err: any) {
        const apiError: ApiError = {
          message: err.message || "Невідома помилка",
          statusCode: err.statusCode || 500,
        };

        setError(apiError);

        if (options.showError !== false) {
          const errorMessage =
            API_ERROR_MESSAGES[apiError.statusCode || 500] ||
            apiError.message;
          toast.error(errorMessage);
        }

        return null;
      } finally {
        setIsLoading(false);
      }
    },
    []
  );

  return { execute, isLoading, error };
};
