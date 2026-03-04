import { z } from "zod";

// ══════════════════════════════════════════════════════
// AUTH VALIDATORS
// ══════════════════════════════════════════════════════

export const loginSchema = z.object({
  email: z.string().email("Некоректна електронна пошта"),
  password: z
    .string()
    .min(1, "Пароль обов'язковий")
    .min(6, "Пароль повинен бути мінімум 6 символів"),
});

export const updateProfileSchema = z.object({
  firstName: z
    .string()
    .min(1, "Ім'я обов'язкове")
    .min(2, "Ім'я повинно мати мінімум 2 символи"),
  lastName: z
    .string()
    .min(1, "Прізвище обов'язкове")
    .min(2, "Прізвище повинно мати мінімум 2 символи"),
  email: z.string().email("Некоректна електронна пошта"),
  phoneNumber: z.string().optional().or(z.literal("")),
});

export const changePasswordSchema = z
  .object({
    currentPassword: z.string().min(1, "Поточний пароль обов'язковий"),
    newPassword: z
      .string()
      .min(8, "Новий пароль повинен бути мінімум 8 символів")
      .regex(/[A-Z]/, "Пароль повинен мати принаймні одну велику літеру")
      .regex(/[a-z]/, "Пароль повинен мати принаймні одну малу літеру")
      .regex(/[0-9]/, "Пароль повинен мати принаймні одну цифру")
      .regex(/[!@#$%^&*]/, "Пароль повинен мати спеціальний символ (!@#$%^&*)"),
    confirmPassword: z.string(),
  })
  .refine((data) => data.newPassword === data.confirmPassword, {
    message: "Паролі не збігаються",
    path: ["confirmPassword"],
  });

// ══════════════════════════════════════════════════════
// STAFF VALIDATORS
// ══════════════════════════════════════════════════════

export const createStaffSchema = z.object({
  firstName: z
    .string()
    .min(1, "Ім'я обов'язкове")
    .min(2, "Ім'я повинно мати мінімум 2 символи"),
  lastName: z
    .string()
    .min(1, "Прізвище обов'язкове")
    .min(2, "Прізвище повинно мати мінімум 2 символи"),
  email: z.string().email("Некоректна електронна пошта"),
  phoneNumber: z.string().optional().or(z.literal("")),
  role: z.string().min(1, "Роль обов'язкова"),
});

export const updateStaffSchema = z.object({
  firstName: z
    .string()
    .min(1, "Ім'я обов'язкове")
    .min(2, "Ім'я повинно мати мінімум 2 символи"),
  lastName: z
    .string()
    .min(1, "Прізвище обов'язкове")
    .min(2, "Прізвище повинно мати мінімум 2 символи"),
  email: z.string().email("Некоректна електронна пошта"),
  phoneNumber: z.string().optional().or(z.literal("")),
  role: z.string().min(1, "Роль обов'язкова"),
});

export type LoginSchema = z.infer<typeof loginSchema>;
export type UpdateProfileSchema = z.infer<typeof updateProfileSchema>;
export type ChangePasswordSchema = z.infer<typeof changePasswordSchema>;
export type CreateStaffSchema = z.infer<typeof createStaffSchema>;
export type UpdateStaffSchema = z.infer<typeof updateStaffSchema>;
