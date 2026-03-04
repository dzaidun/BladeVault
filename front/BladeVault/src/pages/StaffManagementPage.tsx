import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { AppLayout } from "../components/layouts/AppLayout";
import { Button } from "../components/ui/button";
import { StaffTable } from "../components/staff/StaffTable";
import { StaffFilters } from "../components/staff/StaffFilters";
import { AddStaffModal } from "../components/staff/AddStaffModal";
import { TempPasswordModal } from "../components/staff/TempPasswordModal";
import { ViewStaffModal } from "../components/staff/ViewStaffModal";
import { EditStaffModal } from "../components/staff/EditStaffModal";
import { DeleteConfirmDialog } from "../components/staff/DeleteConfirmDialog";
import { api } from "../lib/api";
import type { StaffUser, StaffFilters as StaffFiltersType, CreateStaffResponse } from "../types/users";
import { ArrowLeft, UserPlus } from "lucide-react";
import toast from "react-hot-toast";

export const StaffManagementPage: React.FC = () => {
  const navigate = useNavigate();
  const [staffList, setStaffList] = useState<StaffUser[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(false);

  // Модальні вікна
  const [showAddModal, setShowAddModal] = useState(false);
  const [tempPasswordData, setTempPasswordData] = useState<CreateStaffResponse | null>(null);
  const [selectedStaff, setSelectedStaff] = useState<StaffUser | null>(null);
  const [showViewModal, setShowViewModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [deleteStaffId, setDeleteStaffId] = useState<string | null>(null);

  // Фільтри
  const [filters, setFilters] = useState<Partial<StaffFiltersType>>({
    search: "",
    role: "",
    isActive: "",
    sortBy: "createdAt",
    sortOrder: "desc",
    page: 1,
    pageSize: 20,
  });

  // Завантаження даних
  const fetchStaff = async () => {
    console.log("📥 Fetching staff with filters:", filters);
    setIsLoading(true);
    try {
      const response = await api.staff.listStaffUsers(filters);
      console.log("📊 Got response:", response);
      setStaffList(response.items);
      setTotalCount(response.totalCount);
      console.log("✅ Staff list updated:", response.items);
    } catch (error: any) {
      console.error("❌ Error loading staff:", error);
      toast.error(error?.message || "Помилка завантаження даних");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchStaff();
  }, [filters]);

  const handleFilterChange = (newFilters: Partial<StaffFiltersType>) => {
    setFilters((prev) => ({ ...prev, ...newFilters, page: 1 }));
  };

  const handlePageChange = (page: number) => {
    setFilters((prev) => ({ ...prev, page }));
  };

  const handleShowCreatedPassword = (data: CreateStaffResponse) => {
    setTempPasswordData(data);
    setShowAddModal(false);
  };

  const handleViewStaff = async (staffId: string) => {
    try {
      const staff = await api.staff.getStaffUser(staffId) as StaffUser;
      setSelectedStaff(staff);
      setShowViewModal(true);
    } catch (error: any) {
      toast.error(error?.message || "Помилка завантаження даних");
    }
  };

  const handleEditStaff = async (staff: StaffUser) => {
    setSelectedStaff(staff);
    setShowEditModal(true);
  };

  const handleDeactivate = async (staffId: string) => {
    if (!confirm("Ви впевнені, що хочете деактивувати цього користувача?")) {
      return;
    }

    try {
      await api.staff.deactivateUser(staffId);
      toast.success("Користувача деактивовано!");
      fetchStaff();
    } catch (error: any) {
      toast.error(error?.message || "Помилка при деактивації");
    }
  };

  const handleDelete = async () => {
    if (!deleteStaffId) return;

    try {
      await api.staff.deleteUser(deleteStaffId);
      toast.success("Користувача видалено!");
      setDeleteStaffId(null);
      fetchStaff();
    } catch (error: any) {
      toast.error(error?.message || "Помилка при видаленні");
    }
  };

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
            <h1 className="text-3xl font-bold text-slate-900">Управління працівниками</h1>
          </div>

          <Button onClick={() => setShowAddModal(true)} className="gap-2">
            <UserPlus className="w-4 h-4" />
            Додати працівника
          </Button>
        </div>

        <StaffFilters filters={filters} onFilterChange={handleFilterChange} />

        <StaffTable
          data={staffList}
          isLoading={isLoading}
          totalCount={totalCount}
          page={filters.page || 1}
          pageSize={filters.pageSize || 20}
          onPageChange={handlePageChange}
          onView={handleViewStaff}
          onEdit={handleEditStaff}
          onDeactivate={handleDeactivate}
          onDelete={(id: string) => setDeleteStaffId(id)}
        />
      </div>

      {/* Модалі */}
      {showAddModal && (
        <AddStaffModal
          onClose={() => setShowAddModal(false)}
          onSuccess={(data: CreateStaffResponse) => {
            handleShowCreatedPassword(data);
            fetchStaff();
          }}
        />
      )}

      {tempPasswordData && (
        <TempPasswordModal
          data={tempPasswordData}
          onClose={() => setTempPasswordData(null)}
        />
      )}

      {showViewModal && selectedStaff && (
        <ViewStaffModal
          staff={selectedStaff}
          onClose={() => {
            setShowViewModal(false);
            setSelectedStaff(null);
          }}
          onEdit={() => {
            setShowViewModal(false);
            setShowEditModal(true);
          }}
        />
      )}

      {showEditModal && selectedStaff && (
        <EditStaffModal
          staff={selectedStaff}
          onClose={() => {
            setShowEditModal(false);
            setSelectedStaff(null);
          }}
          onSuccess={() => {
            setShowEditModal(false);
            setSelectedStaff(null);
            fetchStaff();
          }}
        />
      )}

      {deleteStaffId && (
        <DeleteConfirmDialog
          onConfirm={handleDelete}
          onCancel={() => setDeleteStaffId(null)}
        />
      )}
    </AppLayout>
  );
};
