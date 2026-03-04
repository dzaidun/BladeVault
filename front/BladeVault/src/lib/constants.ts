import { UserRole } from "../types/common";

export const USER_ROLES = [
  { value: UserRole.Owner, label: "Owner" },
  { value: UserRole.Admin, label: "Admin" },
  { value: UserRole.Analyst, label: "Analyst" },
  { value: UserRole.CatalogManager, label: "Менеджер Каталогу" },
  { value: UserRole.CallCenter, label: "Call Center" },
  { value: UserRole.Warehouse, label: "Складський" },
];

export const SORT_OPTIONS = [
  { value: "firstName", label: "Ім'я" },
  { value: "lastName", label: "Прізвище" },
  { value: "email", label: "Email" },
  { value: "createdAt", label: "Дата створення" },
  { value: "role", label: "Роль" },
];

export const PAGE_SIZE_OPTIONS = [10, 20, 50, 100];

export const TOAST_DURATION = 4000;

export const API_ERROR_MESSAGES: Record<number, string> = {
  400: "Невірні дані",
  401: "Не авторизовано",
  403: "Доступ заборонено",
  404: "Не знайдено",
  409: "Конфлікт (можливо, електронна пошта вже існує)",
  500: "Помилка сервера",
};
