import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { AppLayout } from "../components/layouts/AppLayout";
import { Card, CardContent, CardHeader, CardTitle } from "../components/ui/card";
import { Button } from "../components/ui/button";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "../components/ui/tabs";
import { EditProfileModal } from "../components/profile/EditProfileModal";
import { ChangePasswordModal } from "../components/profile/ChangePasswordModal";
import { ArrowLeft, Edit2, Lock } from "lucide-react";

export const ProfilePage: React.FC = () => {
  const navigate = useNavigate();
  const { user, updateUser } = useAuth();
  const [showEditModal, setShowEditModal] = useState(false);
  const [showPasswordModal, setShowPasswordModal] = useState(false);

  if (!user) {
    return null;
  }

  return (
    <AppLayout>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <Button
              variant="ghost"
              size="icon"
              onClick={() => navigate("/dashboard")}
            >
              <ArrowLeft className="w-4 h-4" />
            </Button>
            <h1 className="text-3xl font-bold text-slate-900">Мій профіль</h1>
          </div>
        </div>

        <Tabs defaultValue="info" className="w-full">
          <TabsList>
            <TabsTrigger value="info">Інформація</TabsTrigger>
            <TabsTrigger value="security">Безпека</TabsTrigger>
          </TabsList>

          {/* Info tab */}
          <TabsContent value="info" className="space-y-4">
            <Card>
              <CardHeader>
                <CardTitle>Особиста інформація</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label className="text-sm font-medium text-slate-600">Ім'я</label>
                    <p className="text-lg font-semibold mt-1">{user.firstName}</p>
                  </div>
                  <div>
                    <label className="text-sm font-medium text-slate-600">Прізвище</label>
                    <p className="text-lg font-semibold mt-1">{user.lastName}</p>
                  </div>
                  <div>
                    <label className="text-sm font-medium text-slate-600">Email</label>
                    <p className="text-lg font-semibold mt-1">{user.email}</p>
                  </div>
                  <div>
                    <label className="text-sm font-medium text-slate-600">Телефон</label>
                    <p className="text-lg font-semibold mt-1">{user.phoneNumber || "—"}</p>
                  </div>
                  <div>
                    <label className="text-sm font-medium text-slate-600">Роль</label>
                    <p className="text-lg font-semibold mt-1">{user.role}</p>
                  </div>
                  <div>
                    <label className="text-sm font-medium text-slate-600">Статус</label>
                    <p className="text-lg font-semibold mt-1">
                      {user.isActive ? "✅ Активний" : "❌ Неактивний"}
                    </p>
                  </div>
                </div>

                <Button onClick={() => setShowEditModal(true)} className="gap-2">
                  <Edit2 className="w-4 h-4" />
                  Редагувати
                </Button>
              </CardContent>
            </Card>
          </TabsContent>

          {/* Security tab */}
          <TabsContent value="security" className="space-y-4">
            <Card>
              <CardHeader>
                <CardTitle>Зміна пароля</CardTitle>
              </CardHeader>
              <CardContent>
                <Button
                  onClick={() => setShowPasswordModal(true)}
                  variant="outline"
                  className="gap-2"
                >
                  <Lock className="w-4 h-4" />
                  Змінити пароль
                </Button>
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>
      </div>

      {showEditModal && (
        <EditProfileModal
          user={user}
          onClose={() => setShowEditModal(false)}
          onSave={updateUser}
        />
      )}

      {showPasswordModal && (
        <ChangePasswordModal
          onClose={() => setShowPasswordModal(false)}
        />
      )}
    </AppLayout>
  );
};
