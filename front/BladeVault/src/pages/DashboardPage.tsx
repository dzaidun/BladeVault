import React from "react";
import { useNavigate } from "react-router-dom";
import { AppLayout } from "../components/layouts/AppLayout";
import { Card, CardDescription, CardHeader, CardTitle } from "../components/ui/card";
import { Button } from "../components/ui/button";
import { Users, UserCircle, Settings } from "lucide-react";

export const DashboardPage: React.FC = () => {
  const navigate = useNavigate();

  const dashboardCards = [
    {
      title: "Мій профіль",
      description: "Переглядайте та редагуйте ваші особисті дані",
      icon: UserCircle,
      action: () => navigate("/profile"),
    },
    {
      title: "Управління працівниками",
      description: "Додавайте, редагуйте та видаляйте працівників",
      icon: Users,
      action: () => navigate("/staff"),
    },
    {
      title: "Налаштування",
      description: "Налаштування системи (додатково)",
      icon: Settings,
      action: () => navigate("/settings"),
      disabled: true,
    },
  ];

  return (
    <AppLayout>
      <div className="space-y-8">
        <div>
          <h1 className="text-3xl font-bold text-slate-900">Добро пожалувати до BladeVault</h1>
          <p className="text-slate-600 mt-2">Виберіть розділ для початку роботи</p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {dashboardCards.map((card) => {
            const IconComponent = card.icon;
            return (
              <Card
                key={card.title}
                className={`cursor-pointer transition-all hover:shadow-lg ${
                  card.disabled ? "opacity-50 cursor-not-allowed" : ""
                }`}
              >
                <CardHeader>
                  <div className="flex items-start justify-between">
                    <div className="flex-1">
                      <CardTitle>{card.title}</CardTitle>
                      <CardDescription className="mt-2">
                        {card.description}
                      </CardDescription>
                    </div>
                    <IconComponent className="w-6 h-6 text-slate-400 mt-1" />
                  </div>
                </CardHeader>
                <div className="px-6 pb-6">
                  <Button
                    onClick={card.action}
                    disabled={card.disabled}
                    className="w-full"
                  >
                    Перейти
                  </Button>
                </div>
              </Card>
            );
          })}
        </div>
      </div>
    </AppLayout>
  );
};
