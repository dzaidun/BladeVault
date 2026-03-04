import React from "react";
import { Input } from "../ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../ui/select";
import { Card } from "../ui/card";
import { Search } from "lucide-react";
import type { StaffFilters as StaffFiltersType } from "../../types/users";
import { USER_ROLES, SORT_OPTIONS } from "../../lib/constants";

interface StaffFiltersProps {
  filters: Partial<StaffFiltersType>;
  onFilterChange: (filters: Partial<StaffFiltersType>) => void;
}

export const StaffFilters: React.FC<StaffFiltersProps> = ({
  filters,
  onFilterChange,
}) => {
  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    onFilterChange({ search: e.target.value });
  };

  return (
    <Card className="p-4">
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        {/* Пошук */}
        <div className="relative">
          <Search className="absolute left-3 top-3 h-4 w-4 text-slate-400" />
          <Input
            placeholder="Пошук за ім'ям, email..."
            value={filters.search || ""}
            onChange={handleSearchChange}
            className="pl-10"
          />
        </div>

        {/* Фільтр по ролі */}
        <Select
          value={filters.role || "all"}
          onValueChange={(value) => 
            onFilterChange({ role: value === "all" ? "" : (value as any) })
          }
        >
          <SelectTrigger>
            <SelectValue placeholder="Всі ролі" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">Всі ролі</SelectItem>
            {USER_ROLES.filter((role) => role.value !== "Customer").map((role) => (
              <SelectItem key={role.value} value={role.value}>
                {role.label}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>

        {/* Фільтр по активності */}
        <Select
          value={filters.isActive === undefined || filters.isActive === "" ? "all" : String(filters.isActive)}
          onValueChange={(value) =>
            onFilterChange({ isActive: value === "all" ? "" : value === "true" })
          }
        >
          <SelectTrigger>
            <SelectValue placeholder="Всі статуси" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">Всі статуси</SelectItem>
            <SelectItem value="true">Активні</SelectItem>
            <SelectItem value="false">Неактивні</SelectItem>
          </SelectContent>
        </Select>

        {/* Сортування */}
        <Select
          value={filters.sortBy || "createdAt"}
          onValueChange={(value) => onFilterChange({ sortBy: value as any })}
        >
          <SelectTrigger>
            <SelectValue placeholder="Сортування" />
          </SelectTrigger>
          <SelectContent>
            {SORT_OPTIONS.map((option) => (
              <SelectItem key={option.value} value={option.value}>
                {option.label}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>
    </Card>
  );
};
