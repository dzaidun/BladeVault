import axios, { type AxiosInstance, type AxiosError } from "axios";
import type { ApiError } from "../types/common";

const API_BASE_URL = import.meta.env.VITE_API_URL || "http://localhost:5000/api";

class ApiClient {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        "Content-Type": "application/json",
      },
    });

    // Interceptor для додавання токена в заголовок
    this.client.interceptors.request.use((config) => {
      const token = localStorage.getItem("authToken");
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      if (config.method === "post" || config.method === "put") {
        console.log(`📤 ${config.method?.toUpperCase()} ${config.url}:`, config.data);
      }
      return config;
    });

    // Interceptor для обробки помилок
    this.client.interceptors.response.use(
      (response) => response,
      (error: AxiosError) => {
        if (error.response?.status === 401) {
          // Очистити токен та перенаправити на login
          localStorage.removeItem("authToken");
          window.location.href = "/login";
        }
        return Promise.reject(this.handleError(error));
      }
    );
  }

  private handleError(error: AxiosError): ApiError {
    if (error.response) {
      const errorData = error.response.data as any;
      console.error(`❌ Response error (${error.response.status}):`, errorData);
      if (errorData.errors) {
        console.error("📋 Validation errors:", errorData.errors);
      }
      return {
        message:
          errorData?.message || "Помилка сервера",
        statusCode: error.response.status,
        errors: errorData?.errors,
      };
    }
    console.error("❌ Network error:", error.message);
    return {
      message: error.message || "Помилка мережі",
    };
  }

  public get<T>(url: string, config?: any) {
    return this.client.get<T>(url, config);
  }

  public post<T>(url: string, data?: any, config?: any) {
    return this.client.post<T>(url, data, config);
  }

  public put<T>(url: string, data?: any, config?: any) {
    return this.client.put<T>(url, data, config);
  }

  public delete<T>(url: string, config?: any) {
    return this.client.delete<T>(url, config);
  }

  public setToken(token: string) {
    localStorage.setItem("authToken", token);
  }

  public getToken(): string | null {
    return localStorage.getItem("authToken");
  }

  public clearToken() {
    localStorage.removeItem("authToken");
  }
}

export const apiClient = new ApiClient();
