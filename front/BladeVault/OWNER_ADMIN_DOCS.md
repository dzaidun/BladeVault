# BladeVault Frontend - Owner/Admin Module

## ✅ Що реалізовано

### Фаза 1: Архітектура
- ✅ TypeScript типи для всіх entity (`types/auth.ts`, `types/users.ts`, `types/common.ts`)
- ✅ API клієнт з автоматичною підстановкою токена (`lib/api-client.ts`)
- ✅ API функції для всіх ендпоїнтів (`lib/api.ts`)
- ✅ Zod валідатори для форм (`lib/validators.ts`)
- ✅ Константи та конфігурація (`lib/constants.ts`)

### Фаза 2: Аутентифікація
- ✅ Auth контекст для глобального стану користувача (`contexts/AuthContext.tsx`)
- ✅ Hook `useAuth` для доступу до аутентифікації
- ✅ Hook `useApi` для стандартизованої обробки API запитів
- ✅ Login форма з валідацією
- ✅ Protected routes - захист маршрутів від неавторизованих користувачів
- ✅ Автоматична перевірка токена при завантаженні додатку

### Фаза 3: Профіль користувача
- ✅ Сторінка профілю з відображенням особистих даних
- ✅ Модал редагування профілю (ім'я, прізвище, email, телефон)
- ✅ Модал зміни пароля з валідацією (мін. 8 символів, великі/малі літери, цифри, спец. символи)
- ✅ Навігація з dropdown menu для доступу до профілю

### Фаза 4: Управління працівниками
- ✅ Таблиця зі списком всіх працівників
- ✅ Пошук по ім'я/email (динамічний)
- ✅ Фільтри: роль, статус активності
- ✅ Сортування по: ім'я, прізвище, email, дата створення, роль
- ✅ Пагінація (навігація по сторінках)
- ✅ Додавання працівника з автогенерацією тимчасового пароля
- ✅ Модал з одноразовим паролем (з кнопкою копіювання)
- ✅ Перегляд детальної інформації про працівника (включно із статусом пароля)
- ✅ Редагування інформації про працівника
- ✅ Деактивація працівника
- ✅ Видалення працівника (з підтвердженням)

### Фаза 5: UI/UX & Error Handling
- ✅ Shadcn UI компоненти для консистентного дизайну
- ✅ Toast notifications (react-hot-toast) для всіх операцій
- ✅ Loading states для всіх асинхронних операцій
- ✅ Error handling з людськими повідомленнями
- ✅ Підтвердження для небезпечних операцій (видалення, деактивація)
- ✅ Responsive design (Tailwind CSS)

---

## 📂 Структура проекту

\`\`\`
src/
├── components/
│   ├── auth/
│   │   └── LoginForm.tsx
│   ├── profile/
│   │   ├── EditProfileModal.tsx
│   │   └── ChangePasswordModal.tsx
│   ├── staff/
│   │   ├── StaffTable.tsx
│   │   ├── StaffFilters.tsx
│   │   ├── AddStaffModal.tsx
│   │   ├── TempPasswordModal.tsx
│   │   ├── ViewStaffModal.tsx
│   │   ├── EditStaffModal.tsx
│   │   └── DeleteConfirmDialog.tsx
│   ├── shared/
│   │   ├── Navigation.tsx
│   │   └── ProtectedRoute.tsx
│   ├── layouts/
│   │   └── AppLayout.tsx
│   └── ui/               (Shadcn компоненти)
├── contexts/
│   └── AuthContext.tsx
├── hooks/
│   ├── useAuth.ts
│   └── useApi.ts
├── lib/
│   ├── api-client.ts
│   ├── api.ts
│   ├── validators.ts
│   └── constants.ts
├── types/
│   ├── auth.ts
│   ├── users.ts
│   └── common.ts
├── pages/
│   ├── LoginPage.tsx
│   ├── DashboardPage.tsx
│   ├── ProfilePage.tsx
│   └── StaffManagementPage.tsx
├── App.tsx
└── main.tsx
\`\`\`

---

## 🚀 Як запустити

### 1. Встановити залежності
\`\`\`bash
npm install
\`\`\`

### 2. Налаштувати .env
Файл \`.env\` вже створений з такими налаштуваннями:
\`\`\`
VITE_API_URL=http://localhost:5000/api
\`\`\`

Змініть **port** якщо ваш бекенд запущений на іншому порті.

### 3. Запустити dev server
\`\`\`bash
npm run dev
\`\`\`

Фронтенд буде доступний на: **http://localhost:5173/**

---

## 🔑 Тестова інформація

### Логін
- **Email:** \`owner@bladevault.com\`
- **Пароль:** ваш пароль з бекенду

---

## 📋 API Ендпоїнти

Всі ендпоїнти задокументовані у файлі:
\`\`\`
back/BladeVault/API_ENDPOINTS.md
\`\`\`

### Основні ендпоїнти:

#### Аутентифікація
- \`POST /api/auth/login\` - Логін
- \`GET /api/users/me\` - Отримати поточного користувача

#### Профіль
- \`PUT /api/users/me\` - Оновити профіль
- \`POST /api/users/change-password\` - Змінити пароль

#### Управління працівниками (Owner/Admin only)
- \`GET /api/users\` - Список користувачів (з фільтрами, пошуком, пагінацією)
- \`GET /api/users/{id}\` - Отримати користувача по ID
- \`POST /api/users/staff\` - Створити працівника
- \`PUT /api/users/staff/{id}\` - Оновити працівника
- \`POST /api/users/staff/{id}/deactivate\` - Деактивувати
- \`DELETE /api/users/staff/{id}\` - Видалити

---

## 🎨 UI Бібліотеки

- **Tailwind CSS** - Utility-first CSS framework
- **Shadcn UI** - Готові React компоненти
- **Lucide Icons** - Іконки
- **React Hot Toast** - Notifications

---

## 🔐 Безпека

1. **JWT Token** зберігається в \`localStorage\`
2. Токен автоматично додається до кожного запиту через Axios interceptor
3. При 401 відповіді - автоматичний редирект на login
4. Protected routes блокують доступ неавторизованим користувачам
5. **Важливо:** Всі права доступу контролюються на бекенді

---

## ✨ Особливості

### Управління працівниками
- 🔍 **Пошук** - в реальному часі по ім'я, прізвищу, email
- 🎯 **Фільтри** - по ролі (Owner, Admin, Warehouse, CallCenter, etc.), по статусу (активний/неактивний)
- 📊 **Сортування** - по будь-якому полю (ASC/DESC)
- 📄 **Пагінація** - підтримка великих списків працівників
- 🔑 **Автогенерація пароля** - система сама генерує безпечний тимчасовий пароль
- 📋 **Копіювання** - легке копіювання логіна та пароля в буфер обміну
- ⚠️ **Статус пароля** - видно чи працівник вже змінив тимчасовий пароль

### User Experience
- ⚡ Loading states для всіх кнопок та таблиць
- 🎉 Toast notifications для всіх операцій (success/error)
- ✅ Валідація форм з детальними повідомленнями
- 🚫 Confirmation dialogs для небезпечних операцій
- 📱 Responsive design - працює на всіх екранах

---

## 🛠 Технології

- **React 19.2**
- **TypeScript 5.9**
- **Vite 7.3**
- **React Router 6**
- **Axios** для HTTP запитів
- **React Hook Form** + **Zod** для валідації форм
- **Tailwind CSS v4**
- **Shadcn UI**

---

## 📝 Наступні кроки (опціонально)

1. **Analyst Dashboard** - дашборд з аналітикою (графіки, звіти)
2. **Product Management** - управління товарами (CatalogManager)
3. **Orders Management** - замовлення (CallCenter, Warehouse)
4. **Warehouse Workflow** - складський процес (Warehouse)
5. **Call Center** - логування дзвінків (CallCenter)

---

## 🐛 Відомі проблеми

- TypeScript може показувати помилки "Cannot find module" в IDE - це помилка кешування. Перезавантажте TypeScript Server (Cmd+Shift+P -> "TypeScript: Restart TS Server")

---

## 💡 Поради

1. **Перезавантажуйте сервер** після зміни \`.env\` файлу
2. **Перевіряйте консоль браузера** для детальних помилок API
3. **Використовуйте React DevTools** для дебагу компонентів
4. **Перевіряйте Network tab** для моніторингу API запитів

---

## ✅ Чекліст готовності

- [x] Аутентифікація та авторизація
- [x] Зміна пароля
- [x] Редагування профілю
- [x] Додавання працівників
- [x] Перегляд таблиці працівників
- [x] Пошук, фільтрація, сортування
- [x] Модал з одноразовим паролем та копіювання
- [x] Редагування працівників
- [x] Деактивація працівників
- [x] Видалення працівників

---

**Все готово! 🎉 Насолоджуйтесь роботою з BladeVault!**
