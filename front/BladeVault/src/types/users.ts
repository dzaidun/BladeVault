import type { User } from "./common";
import { UserRole } from "./common";

export interface StaffUser extends User {
  createdByUserId?: string;
  createdByUserName?: string;
  mustChangePassword?: boolean;
  temporaryPasswordIssuedAt?: string | null;
}

export interface CreateStaffRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  role: UserRole;
}

export interface CreateStaffResponse {
  userId: string;
  login: string;
  temporaryPassword: string;
  role: string;
}

export interface UpdateStaffRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  role: UserRole;
}

export interface StaffListResponse {
  items: StaffUser[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface StaffFilters {
  search?: string;
  role?: UserRole | "";
  isActive?: boolean | "";
  sortBy?: keyof StaffUser;
  sortOrder?: "asc" | "desc";
  page?: number;
  pageSize?: number;
}
