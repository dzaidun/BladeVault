import { apiClient } from "./api-client";
import type {
  LoginRequest,
  LoginResponse,
  UpdateProfileRequest,
  ChangePasswordRequest,
  User,
} from "../types/auth";
import type {
  CreateStaffRequest,
  CreateStaffResponse,
  UpdateStaffRequest,
  StaffListResponse,
  StaffFilters,
  StaffUser,
} from "../types/users";

// ══════════════════════════════════════════════════════
// AUTH ENDPOINTS
// ══════════════════════════════════════════════════════

export const authApi = {
  login: async (data: LoginRequest) => {
    const response = await apiClient.post<LoginResponse>("/auth/login", data);
    // Backend повертає accessToken, потрібно отримати повну інформацію про користувача
    apiClient.setToken(response.data.accessToken);
    const profile = await apiClient.get<User>("/users/me");
    
    return {
      token: response.data.accessToken,
      user: profile.data,
    };
  },

  getMyProfile: async () => {
    const response = await apiClient.get<User>("/users/me");
    return response.data;
  },

  updateMyProfile: async (data: UpdateProfileRequest) => {
    await apiClient.put("/users/me", data);
  },

  changePassword: async (data: ChangePasswordRequest) => {
    await apiClient.post("/users/change-password", data);
  },
};

// ══════════════════════════════════════════════════════
// STAFF MANAGEMENT ENDPOINTS
// ══════════════════════════════════════════════════════

export const staffApi = {
  listStaffUsers: async (filters: Partial<StaffFilters> = {}) => {
    const params = new URLSearchParams();

    if (filters.search) params.append("search", filters.search);
    if (filters.role) params.append("role", filters.role);
    if (filters.isActive !== undefined && filters.isActive !== "")
      params.append("isActive", String(filters.isActive));
    if (filters.sortBy) params.append("sortBy", filters.sortBy);
    if (filters.sortOrder) params.append("sortOrder", filters.sortOrder);
    if (filters.page) params.append("page", String(filters.page));
    if (filters.pageSize) params.append("pageSize", String(filters.pageSize));

    const url = `/users?${params.toString()}`;
    console.log("🔍 Fetching staff users from:", url);
    
    try {
      const response = await apiClient.get<StaffListResponse>(url);
      console.log("✅ Staff response:", response.data);
      return response.data;
    } catch (error: any) {
      console.error("❌ Error fetching staff:", error);
      throw error;
    }
  },

  getStaffUser: async (userId: string) => {
    const response = await apiClient.get<StaffUser>(`/users/${userId}`);
    return response.data;
  },

  createStaffUser: async (data: CreateStaffRequest) => {
    console.log("📤 Creating staff user with data:", data);
    try {
      const response = await apiClient.post<CreateStaffResponse>(
        "/users/staff",
        data
      );
      console.log("✅ Create staff response:", response.data);
      return response.data;
    } catch (error: any) {
      console.error("❌ Error creating staff:", error.response?.data || error);
      throw error;
    }
  },

  updateStaffUser: async (userId: string, data: UpdateStaffRequest) => {
    console.log(`📤 Updating staff user ${userId} with data:`, data);
    try {
      await apiClient.put(`/users/staff/${userId}`, data);
      console.log("✅ Update staff success");
    } catch (error: any) {
      console.error("❌ Error updating staff:", error);
      throw error;
    }
  },

  deactivateUser: async (userId: string) => {
    await apiClient.post(`/users/staff/${userId}/deactivate`);
  },

  deleteUser: async (userId: string) => {
    await apiClient.delete(`/users/staff/${userId}`);
  },
};

export const api = {
  auth: authApi,
  staff: staffApi,
};
