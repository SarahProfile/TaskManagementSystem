import axiosInstance from './axios';
import { Project, CreateProjectDto, UpdateProjectDto, PaginatedResult, ProjectFilters } from '../types/project.types';
import { ApiResponse } from '../types/api.types';

export const projectsApi = {
  async getAllProjects(filters?: ProjectFilters): Promise<PaginatedResult<Project>> {
    const params = new URLSearchParams();

    if (filters) {
      Object.entries(filters).forEach(([key, value]) => {
        if (value !== undefined && value !== null) {
          params.append(key, String(value));
        }
      });
    }

    const response = await axiosInstance.get<ApiResponse<PaginatedResult<Project>>>('/projects', { params });
    return response.data.data;
  },

  async getProjectById(id: string): Promise<Project> {
    const response = await axiosInstance.get<ApiResponse<Project>>(`/projects/${id}`);
    return response.data.data;
  },

  async createProject(data: CreateProjectDto): Promise<Project> {
    const response = await axiosInstance.post<ApiResponse<Project>>('/projects', data);
    return response.data.data;
  },

  async updateProject(id: string, data: UpdateProjectDto): Promise<Project> {
    const response = await axiosInstance.put<ApiResponse<Project>>(`/projects/${id}`, data);
    return response.data.data;
  },

  async deleteProject(id: string): Promise<void> {
    await axiosInstance.delete(`/projects/${id}`);
  },

  async getProjectsByOwner(ownerId: string, filters?: ProjectFilters): Promise<PaginatedResult<Project>> {
    const params = new URLSearchParams();

    if (filters) {
      Object.entries(filters).forEach(([key, value]) => {
        if (value !== undefined && value !== null) {
          params.append(key, String(value));
        }
      });
    }

    const response = await axiosInstance.get<ApiResponse<PaginatedResult<Project>>>(`/projects/owner/${ownerId}`, { params });
    return response.data.data;
  },
};
