import axiosInstance from './axios';
import { LoginRequest, RegisterRequest, AuthResponse, User } from '../types/auth.types';

export const authApi = {
  async login(credentials: LoginRequest): Promise<AuthResponse> {
    const response = await axiosInstance.post<AuthResponse>('/auth/login', credentials);
    return response.data;
  },

  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await axiosInstance.post<AuthResponse>('/auth/register', data);
    return response.data;
  },

  async getCurrentUser(): Promise<User> {
    const response = await axiosInstance.get<User>('/auth/me');
    return response.data;
  },

  async refreshToken(): Promise<AuthResponse> {
    const response = await axiosInstance.post<AuthResponse>('/auth/refresh');
    return response.data;
  },
};
