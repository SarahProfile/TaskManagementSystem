import axiosInstance from './axios';
import { LoginRequest, RegisterRequest, AuthResponse, User } from '../types/auth.types';
import { ApiResponse } from '../types/api.types';

export const authApi = {
  async login(credentials: LoginRequest): Promise<AuthResponse> {
    const response = await axiosInstance.post<ApiResponse<AuthResponse>>('/auth/login', credentials);
    return response.data.data;
  },

  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await axiosInstance.post<ApiResponse<AuthResponse>>('/auth/register', data);
    return response.data.data;
  },

  async getCurrentUser(): Promise<User> {
    const response = await axiosInstance.get<ApiResponse<User>>('/auth/me');
    return response.data.data;
  },

  async refreshToken(): Promise<AuthResponse> {
    const response = await axiosInstance.post<ApiResponse<AuthResponse>>('/auth/refresh');
    return response.data.data;
  },
};
