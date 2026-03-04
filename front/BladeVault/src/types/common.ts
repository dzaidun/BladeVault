// Pagination & API Response Types
export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface QueryParams {
  search?: string;
  role?: string;
  isActive?: boolean;
  sortBy?: string;
  sortOrder?: "asc" | "desc";
  page?: number;
  pageSize?: number;
}

export interface ApiError {
  message: string;
  errors?: Record<string, string[]>;
  statusCode?: number;
}

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  role: UserRole;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
}

export enum UserRole {
  Customer = "Customer",
  Owner = "Owner",
  Admin = "Admin",
  Analyst = "Analyst",
  CatalogManager = "CatalogManager",
  CallCenter = "CallCenter",
  Warehouse = "Warehouse",
}
