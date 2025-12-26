export enum ProjectStatus {
  PLANNING = 'PLANNING',
  ACTIVE = 'ACTIVE',
  ON_HOLD = 'ON_HOLD',
  COMPLETED = 'COMPLETED',
  ARCHIVED = 'ARCHIVED',
}

export enum Priority {
  LOW = 'LOW',
  MEDIUM = 'MEDIUM',
  HIGH = 'HIGH',
  URGENT = 'URGENT',
}

export interface Project {
  id: string;
  name: string;
  description?: string;
  status: ProjectStatus;
  priority: Priority;
  startDate?: string;
  endDate?: string;
  ownerId: string;
  createdAt: string;
  updatedAt: string;
  owner?: {
    id: string;
    username: string;
    email: string;
  };
  tasks?: Array<{
    id: string;
    title: string;
    status: string;
  }>;
}

export interface CreateProjectDto {
  name: string;
  description?: string;
  status?: ProjectStatus;
  priority?: Priority;
  startDate?: string;
  endDate?: string;
}

export interface UpdateProjectDto {
  name?: string;
  description?: string;
  status?: ProjectStatus;
  priority?: Priority;
  startDate?: string;
  endDate?: string;
}

export interface ProjectFilters {
  status?: ProjectStatus;
  priority?: Priority;
  search?: string;
  page?: number;
  limit?: number;
}

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
