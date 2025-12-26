import { AxiosError } from 'axios';

export interface ApiErrorResponse {
  success: boolean;
  message: string;
  data?: any;
  errors?: Record<string, string[]>;
}

export function getErrorMessage(error: unknown): string {
  if (error instanceof Error) {
    return error.message;
  }

  const axiosError = error as AxiosError<ApiErrorResponse>;

  if (axiosError.response?.data) {
    const data = axiosError.response.data;

    if (data.message) {
      return data.message;
    }

    if (data.errors) {
      const errorMessages = Object.values(data.errors)
        .flat()
        .filter(Boolean);

      if (errorMessages.length > 0) {
        return errorMessages.join('. ');
      }
    }
  }

  if (axiosError.message) {
    return axiosError.message;
  }

  return 'An unexpected error occurred. Please try again.';
}

export function getFieldErrors(error: unknown): Record<string, string> | null {
  const axiosError = error as AxiosError<ApiErrorResponse>;

  if (axiosError.response?.data?.errors) {
    const errors = axiosError.response.data.errors;
    const fieldErrors: Record<string, string> = {};

    Object.entries(errors).forEach(([field, messages]) => {
      if (messages && messages.length > 0) {
        fieldErrors[field.toLowerCase()] = messages[0];
      }
    });

    return fieldErrors;
  }

  return null;
}

export function isNetworkError(error: unknown): boolean {
  const axiosError = error as AxiosError;
  return axiosError.message === 'Network Error' || !axiosError.response;
}

export function isUnauthorizedError(error: unknown): boolean {
  const axiosError = error as AxiosError;
  return axiosError.response?.status === 401;
}

export function isForbiddenError(error: unknown): boolean {
  const axiosError = error as AxiosError;
  return axiosError.response?.status === 403;
}

export function isValidationError(error: unknown): boolean {
  const axiosError = error as AxiosError<ApiErrorResponse>;
  return axiosError.response?.status === 400 && !!axiosError.response?.data?.errors;
}
