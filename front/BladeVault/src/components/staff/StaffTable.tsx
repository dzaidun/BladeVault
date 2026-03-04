import React from "react";
import type { StaffUser } from "../../types/users";
import { Button } from "../ui/button";
import { Badge } from "../ui/badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "../ui/table";
import { Card } from "../ui/card";
import { Eye, Edit2, Trash2, UserX } from "lucide-react";

interface StaffTableProps {
  data: StaffUser[];
  isLoading: boolean;
  totalCount: number;
  page: number;
  pageSize: number;
  onPageChange: (page: number) => void;
  onView: (id: string) => void;
  onEdit: (staff: StaffUser) => void;
  onDeactivate: (id: string) => void;
  onDelete: (id: string) => void;
}

export const StaffTable: React.FC<StaffTableProps> = ({
  data,
  isLoading,
  totalCount,
  page,
  pageSize,
  onPageChange,
  onView,
  onEdit,
  onDeactivate,
  onDelete,
}) => {
  const totalPages = Math.ceil(totalCount / pageSize);

  if (isLoading) {
    return (
      <Card className="p-8">
        <div className="flex justify-center items-center">
          <div className="w-8 h-8 border-4 border-slate-300 border-t-blue-600 rounded-full animate-spin"></div>
          <span className="ml-3 text-slate-600">Завантаження...</span>
        </div>
      </Card>
    );
  }

  if (data.length === 0) {
    return (
      <Card className="p-8">
        <div className="text-center text-slate-500">
          <p className="text-lg font-medium">Працівників не знайдено</p>
          <p className="text-sm mt-1">Спробуйте змінити фільтри або додайте нового працівника</p>
        </div>
      </Card>
    );
  }

  return (
    <div className="space-y-4">
      <Card>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Ім'я</TableHead>
              <TableHead>Email</TableHead>
              <TableHead>Телефон</TableHead>
              <TableHead>Роль</TableHead>
              <TableHead>Статус</TableHead>
              <TableHead>Створено</TableHead>
              <TableHead className="text-right">Дії</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data.map((staff) => (
              <TableRow key={staff.id}>
                <TableCell className="font-medium">
                  {staff.firstName} {staff.lastName}
                </TableCell>
                <TableCell>{staff.email}</TableCell>
                <TableCell>{staff.phoneNumber || "—"}</TableCell>
                <TableCell>
                  <Badge variant="outline">{staff.role}</Badge>
                </TableCell>
                <TableCell>
                  {staff.isActive ? (
                    <Badge variant="default" className="bg-green-600">Активний</Badge>
                  ) : (
                    <Badge variant="destructive">Неактивний</Badge>
                  )}
                </TableCell>
                <TableCell>
                  {new Date(staff.createdAt).toLocaleDateString("uk-UA")}
                </TableCell>
                <TableCell className="text-right">
                  <div className="flex justify-end gap-2">
                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => onView(staff.id)}
                      title="Переглянути"
                    >
                      <Eye className="w-4 h-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => onEdit(staff)}
                      title="Редагувати"
                    >
                      <Edit2 className="w-4 h-4" />
                    </Button>
                    {staff.isActive && (
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => onDeactivate(staff.id)}
                        title="Деактивувати"
                      >
                        <UserX className="w-4 h-4" />
                      </Button>
                    )}
                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => onDelete(staff.id)}
                      title="Видалити"
                    >
                      <Trash2 className="w-4 h-4 text-red-600" />
                    </Button>
                  </div>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Card>

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="flex items-center justify-between">
          <div className="text-sm text-slate-600">
            Показано {(page - 1) * pageSize + 1} -{" "}
            {Math.min(page * pageSize, totalCount)} з {totalCount}
          </div>
          <div className="flex gap-2">
            <Button
              variant="outline"
              onClick={() => onPageChange(page - 1)}
              disabled={page === 1}
            >
              Назад
            </Button>
            <div className="flex items-center gap-2">
              {Array.from({ length: Math.min(5, totalPages) }, (_, i) => {
                let pageNum;
                if (totalPages <= 5) {
                  pageNum = i + 1;
                } else if (page <= 3) {
                  pageNum = i + 1;
                } else if (page >= totalPages - 2) {
                  pageNum = totalPages - 4 + i;
                } else {
                  pageNum = page - 2 + i;
                }
                return (
                  <Button
                    key={pageNum}
                    variant={page === pageNum ? "default" : "outline"}
                    onClick={() => onPageChange(pageNum)}
                  >
                    {pageNum}
                  </Button>
                );
              })}
            </div>
            <Button
              variant="outline"
              onClick={() => onPageChange(page + 1)}
              disabled={page === totalPages}
            >
              Вперед
            </Button>
          </div>
        </div>
      )}
    </div>
  );
};
