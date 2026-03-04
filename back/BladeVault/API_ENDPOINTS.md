# API ENDPOINTS FOR FRONTEND

## Frontend: Owner + Admin User Management & Profile

This document lists all required API endpoints for the single-page frontend targeting Owner, Admin, Analyst, CatalogManager, CallCenter, and Warehouse roles.

### Initial Phase: Owner + Admin Full Access

---

## 1. Authentication & Profile

### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "owner@bladevault.com",
  "password": "SecurePassword123!"
}

Response: 200 OK
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "user": {
    "id": "uuid",
    "firstName": "Ivan",
    "lastName": "Kovalenko",
    "email": "owner@bladevault.com",
    "role": "Owner",
    "isActive": true
  }
}
```

### Register (Customer only)
```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phoneNumber": "+380501234567",
  "password": "SecurePassword123!"
}

Response: 201 Created
{
  "id": "uuid"
}
```

---

## 2. User Profile Management (Self)

### Get My Profile
```http
GET /api/users/me
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "uuid",
  "firstName": "Ivan",
  "lastName": "Kovalenko",
  "email": "owner@bladevault.com",
  "phoneNumber": "+380501234567",
  "role": "Owner",
  "isActive": true,
  "createdAt": "2026-03-01T10:00:00Z",
  "updatedAt": "2026-03-04T15:30:00Z"
}
```

### Update My Profile
```http
PUT /api/users/me
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "Ivan",
  "lastName": "Kovalenko",
  "email": "ivan.kovalenko@bladevault.com",
  "phoneNumber": "+380501234567"
}

Response: 204 No Content
```

### Change My Password
```http
POST /api/users/change-password
Authorization: Bearer {token}
Content-Type: application/json

{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewPassword123!",
  "confirmPassword": "NewPassword123!"
}

Response: 204 No Content
```

### Change Temporary Password (First Login)
```http
POST /api/users/change-temporary-password
Authorization: Bearer {token}
Content-Type: application/json

{
  "temporaryPassword": "TEMP_AUTO_GENERATED",
  "newPassword": "MyNewPassword123!",
  "confirmPassword": "MyNewPassword123!"
}

Response: 204 No Content
```

---

## 3. Staff Management (Owner/Admin only)

### List All Users with Filtering, Search & Sorting
```http
GET /api/users?search=john&role=Warehouse&isActive=true&sortBy=firstName&sortOrder=asc&page=1&pageSize=20
Authorization: Bearer {token}

Query Parameters:
- search: string (optional) - search by firstName, lastName, email
- role: string (optional) - filter by role (Owner, Admin, Warehouse, CallCenter, CatalogManager, Analyst)
- isActive: boolean (optional) - filter by active/inactive status
- sortBy: string (optional) - field to sort by: firstName, lastName, email, createdAt, role (default: createdAt)
- sortOrder: string (optional) - asc or desc (default: desc)
- page: int (optional) - page number (default: 1)
- pageSize: int (optional) - items per page (default: 20, max: 200)

Response: 200 OK
{
  "items": [
    {
      "id": "uuid",
      "firstName": "John",
      "lastName": "Doe",
      "email": "john.doe@bladevault.com",
      "phoneNumber": "+380501234567",
      "role": "Warehouse",
      "isActive": true,
      "createdAt": "2026-02-15T10:00:00Z",
      "createdByUserId": "uuid",
      "createdByUserName": "Ivan Kovalenko",
      "mustChangePassword": false,
      "temporaryPasswordIssuedAt": null
    }
  ],
  "totalCount": 15,
  "page": 1,
  "pageSize": 20
}
```

### Get User by Id
```http
GET /api/users/{userId}
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "uuid",
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@bladevault.com",
  "phoneNumber": "+380501234567",
  "role": "Warehouse",
  "isActive": true,
  "createdAt": "2026-02-15T10:00:00Z",
  "createdByUserId": "uuid",
  "createdByUserName": "Ivan Kovalenko",
  "mustChangePassword": false,
  "temporaryPasswordIssuedAt": "2026-02-15T10:05:00Z"
}
```

### Create Staff User (with auto-generated temporary password)
```http
POST /api/users/staff
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@bladevault.com",
  "phoneNumber": "+380501234567",
  "role": "Warehouse"  // Owner, Admin, Warehouse, CallCenter, CatalogManager, Analyst
}

Response: 201 Created
{
  "userId": "uuid",
  "temporaryPassword": "TEMP_ABC123DEF456GHI789",
  "temporaryPasswordIssuedAt": "2026-03-04T16:00:00Z",
  "mustChangePassword": true
}
```

### Update Staff User
```http
PUT /api/users/staff/{staffUserId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@bladevault.com",
  "phoneNumber": "+380501234567",
  "role": "CallCenter"
}

Response: 204 No Content
```

### Deactivate User
```http
POST /api/users/staff/{userId}/deactivate
Authorization: Bearer {token}

Response: 204 No Content
```

### Delete User
```http
DELETE /api/users/staff/{userId}
Authorization: Bearer {token}

Response: 204 No Content
```

---

## Future Phases

- Analytics & Reports (Analyst)
- Product Management (CatalogManager)
- Stock Management (CatalogManager, Warehouse)
- Warehouse Workflow (Warehouse)
- Call Center Operations (CallCenter)
