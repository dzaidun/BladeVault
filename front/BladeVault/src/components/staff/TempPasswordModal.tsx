import React, { useState } from "react";
import type { CreateStaffResponse } from "../../types/users";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "../ui/dialog";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Alert, AlertDescription } from "../ui/alert";
import { Copy, CheckCircle2, AlertCircle } from "lucide-react";
import toast from "react-hot-toast";

interface TempPasswordModalProps {
  data: CreateStaffResponse;
  onClose: () => void;
}

export const TempPasswordModal: React.FC<TempPasswordModalProps> = ({
  data,
  onClose,
}) => {
  const [copied, setCopied] = useState(false);

  const handleCopyPassword = () => {
    navigator.clipboard.writeText(data.temporaryPassword);
    setCopied(true);
    toast.success("Пароль скопійовано!");
    setTimeout(() => setCopied(false), 3000);
  };

  const handleCopyAll = () => {
    const text = `Ваші облікові дані для входу в систему BladeVault:\n\nEmail: ${data.login}\nТимчасовий пароль: ${data.temporaryPassword}\n\nПісля входу ви повинні змінити пароль.`;
    navigator.clipboard.writeText(text);
    toast.success("Всі дані скопійовано!");
  };

  return (
    <Dialog open={true} onOpenChange={(open) => !open && onClose()}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <CheckCircle2 className="w-6 h-6 text-green-600" />
            Працівника успішно створено!
          </DialogTitle>
          <DialogDescription>
            Збережіть або скопіюйте ці дані. Тимчасовий пароль не буде показано знову.
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-4">
          <Alert>
            <AlertCircle className="h-4 w-4" />
            <AlertDescription>
              Працівник повинен змінити цей пароль при першому вході в систему.
            </AlertDescription>
          </Alert>

          <div className="space-y-3">
            <div>
              <label className="text-sm font-medium text-slate-700 mb-1 block">
                Email (логін)
              </label>
              <Input value={data.login} readOnly className="bg-slate-50" />
            </div>

            <div>
              <label className="text-sm font-medium text-slate-700 mb-1 block">
                Тимчасовий пароль
              </label>
              <div className="flex gap-2">
                <Input
                  value={data.temporaryPassword}
                  readOnly
                  className="bg-slate-50 font-mono"
                />
                <Button
                  variant="outline"
                  size="icon"
                  onClick={handleCopyPassword}
                  title="Копіювати пароль"
                >
                  {copied ? (
                    <CheckCircle2 className="w-4 h-4 text-green-600" />
                  ) : (
                    <Copy className="w-4 h-4" />
                  )}
                </Button>
              </div>
            </div>
          </div>

          <div className="flex gap-3 pt-4">
            <Button
              variant="outline"
              onClick={handleCopyAll}
              className="flex-1"
            >
              Копіювати все
            </Button>
            <Button onClick={onClose} className="flex-1">
              Готово
            </Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
};
