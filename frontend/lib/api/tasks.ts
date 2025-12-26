import axiosInstance from './axios';
import { Task, CreateTaskDto, UpdateTaskDto, PaginatedResult, TaskFilters } from '../types/task.types';
import { ApiResponse } from '../types/api.types';

export const tasksApi = {
  async getAllTasks(filters?: TaskFilters): Promise<PaginatedResult<Task>> {
    const params = new URLSearchParams();

    if (filters) {
      Object.entries(filters).forEach(([key, value]) => {
        if (value !== undefined && value !== null) {
          params.append(key, String(value));
        }
      });
    }

    const response = await axiosInstance.get<ApiResponse<PaginatedResult<Task>>>('/tasks', { params });
    return response.data.data;
  },

  async getTaskById(id: string): Promise<Task> {
    const response = await axiosInstance.get<ApiResponse<Task>>(`/tasks/${id}`);
    return response.data.data;
  },

  async createTask(data: CreateTaskDto): Promise<Task> {
    const response = await axiosInstance.post<ApiResponse<Task>>('/tasks', data);
    return response.data.data;
  },

  async updateTask(id: string, data: UpdateTaskDto): Promise<Task> {
    const response = await axiosInstance.put<ApiResponse<Task>>(`/tasks/${id}`, data);
    return response.data.data;
  },

  async deleteTask(id: string): Promise<void> {
    await axiosInstance.delete(`/tasks/${id}`);
  },

  async getTasksByProject(projectId: string, filters?: TaskFilters): Promise<PaginatedResult<Task>> {
    const params = new URLSearchParams();

    if (filters) {
      Object.entries(filters).forEach(([key, value]) => {
        if (value !== undefined && value !== null) {
          params.append(key, String(value));
        }
      });
    }

    const response = await axiosInstance.get<ApiResponse<PaginatedResult<Task>>>(`/tasks/project/${projectId}`, { params });
    return response.data.data;
  },

  async getTasksByUser(userId: string, filters?: TaskFilters): Promise<PaginatedResult<Task>> {
    const params = new URLSearchParams();

    if (filters) {
      Object.entries(filters).forEach(([key, value]) => {
        if (value !== undefined && value !== null) {
          params.append(key, String(value));
        }
      });
    }

    const response = await axiosInstance.get<ApiResponse<PaginatedResult<Task>>>(`/tasks/user/${userId}`, { params });
    return response.data.data;
  },
};
