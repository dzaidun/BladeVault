import React from "react";
import type { StaffUser } from "../../types/users";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "../ui/dialog";
import { Button } from "../ui/button";
import { Badge } from "../ui/badge";
import { Edit2, X } from "lucide-react";

interface ViewStaffModalProps {
  staff: StaffUser;
  onClose: () => void;
  onEdit: () => void;
}

export const ViewStaffModal: React.FC<ViewStaffModalProps> = ({
  staff,
  onClose,
  onEdit,
}) => {
  const InfoRow: React.FC<{ label: string; value: React.ReactNode }> = ({
    label,
    value,
  }) => (
    <div className="flex flex-col space-y-1">
      <span className="text-sm font-medium text-slate-600">{label}</span>
      <span className="text-base text-slate-900">{value}</span>
    </div>
  );

  return (
    <Dialog open={true} onOpenChange={(open) => !open && onClose()}>
      <DialogContent className="sm:max-w-[550px]">
        <DialogHeader>
          <DialogTitle>Інформація про працівника</DialogTitle>
        </DialogHeader>

        <div className="space-y-6">
          {/* Основна інформація */}
          <div className="grid grid-cols-2 gap-4">
            <InfoRow label="Ім'я" value={staff.firstName} />
            <InfoRow label="Прізвище" value={staff.lastName} />
            <InfoRow label="Email" value={staff.email} />
            <InfoRow label="Телефон" value={staff.phoneNumber || "—"} />
          </div>

          {/* Роль і статус */}
          <div className="grid grid-cols-2 gap-4">
            <InfoRow
              label="Роль"
              value={<Badge variant="outline">{staff.role}</Badge>}
            />
            <InfoRow
              label="Статус"
              value={
                staff.isActive ? (
                  <Badge className="bg-green-600">Активний</Badge>
                ) : (
                  <Badge variant="destructive">Неактивний</Badge>
                )
              }
            />
          </div>

          {/* Інформація про створення */}
          <div className="pt-4 border-t">
            <h3 className="text-sm font-semibold text-slate-700 mb-3">
              Додаткова інформація
            </h3>
            <div className="space-y-3">
              <InfoRow
                label="Дата створення"
                value={new Date(staff.createdAt).toLocaleString("uk-UA")}
              />
              {staff.createdByUserName && (
                <InfoRow
                  label="Створено користувачем"
                  value={staff.createdByUserName}
                />
              )}
            </div>
          </div>

          {/* Інформація про тимчасовий пароль */}
          {staff.temporaryPasswordIssuedAt && (
            <div className="pt-4 border-t">
              <h3 className="text-sm font-semibold text-slate-700 mb-3">
                Тимчасовий пароль
              </h3>
              <div className="space-y-3">
                <InfoRow
                  label="Дата видачі"
                  value={new Date(staff.temporaryPasswordIssuedAt).toLocaleString("uk-UA")}
                />
                <InfoRow
                  label="Статус пароля"
                  value={
                    staff.mustChangePassword ? (
                      <Badge variant="outline" className="bg-yellow-50 text-yellow-700 border-yellow-300">
                        ⚠️ Потрібна зміна пароля
                      </Badge>
                    ) : (
                      <Badge variant="outline" className="bg-green-50 text-green-700 border-green-300">
                        ✅ Пароль змінено
                      </Badge>
                    )
                  }
                />
              </div>
            </div>
          )}

          {/* Дії */}
          <div className="flex gap-3 pt-4">
            <Button
              variant="outline"
              onClick={onClose}
              className="flex-1"
            >
              <X className="w-4 h-4 mr-2" />
              Закрити
            </Button>
            <Button onClick={onEdit} className="flex-1">
              <Edit2 className="w-4 h-4 mr-2" />
              Редагувати
            </Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
};
