# 📋 Task Management System

A modern, full-stack task management application built with .NET 8 and Next.js 14.

![Tech Stack](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Next.js](https://img.shields.io/badge/Next.js-14-black?logo=next.js)
![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178C6?logo=typescript)
![TailwindCSS](https://img.shields.io/badge/Tailwind-3.0-38B2AC?logo=tailwind-css)

## ✨ Features

- 🔐 **User Authentication** - JWT-based secure authentication
- 📊 **Dashboard** - Real-time statistics and insights
- ✅ **Task Management** - Create, update, delete, and track tasks
- 📁 **Project Organization** - Group tasks into projects
- 🎨 **Modern UI** - Responsive design with dark mode support
- 📱 **Mobile Friendly** - Works on all devices
- 🚀 **Fast & Scalable** - Clean architecture with performance in mind

## 🏗️ Architecture

### Backend (.NET 8)
- **Clean Architecture** - Domain, Application, Infrastructure, API layers
- **Entity Framework Core** - Database access with SQLite
- **JWT Authentication** - Secure token-based auth
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **Serilog** - Structured logging

### Frontend (Next.js 14)
- **App Router** - Modern Next.js routing
- **TypeScript** - Type-safe development
- **Tailwind CSS** - Utility-first styling
- **Axios** - HTTP client with interceptors
- **React Context** - State management

## 📁 Project Structure

```
TaskManagementSystem/
├── backend/
│   └── src/
│       ├── TaskManagement.API/          # API controllers & configuration
│       ├── TaskManagement.Application/  # Business logic & DTOs
│       ├── TaskManagement.Domain/       # Core entities & enums
│       └── TaskManagement.Infrastructure/ # Data access & repositories
└── frontend/
    ├── app/                    # Next.js pages & layouts
    ├── components/             # React components
    └── lib/                    # API clients, hooks, types
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [npm](https://www.npmjs.com/) or [yarn](https://yarnpkg.com/)

### Backend Setup

1. Navigate to the backend directory:
```bash
cd backend/src/TaskManagement.API
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Update database configuration in `appsettings.json` if needed

4. Run the application:
```bash
dotnet run
```

The API will be available at `http://localhost:5162`

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Create `.env.local` file:
```env
NEXT_PUBLIC_API_URL=http://localhost:5162/api
```

4. Run the development server:
```bash
npm run dev
```

The application will be available at `http://localhost:3000`

## 🔑 Default Credentials

**Admin Account:**
- Email: `admin@taskmanagement.com`
- Password: `Admin@123`

**Regular Users:**
- Email: `john.doe@taskmanagement.com` / Password: `User@123`
- Email: `jane.smith@taskmanagement.com` / Password: `User@123`

## 🌐 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login

### Tasks
- `GET /api/tasks` - Get all tasks
- `GET /api/tasks/{id}` - Get task by ID
- `POST /api/tasks` - Create task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

### Projects
- `GET /api/projects` - Get all projects
- `GET /api/projects/{id}` - Get project by ID
- `POST /api/projects` - Create project
- `PUT /api/projects/{id}` - Update project
- `DELETE /api/projects/{id}` - Delete project

## 🎨 Screenshots

### Dashboard
Beautiful gradient cards showing task statistics with real-time updates.

### Tasks Page
Responsive grid layout with color-coded status badges and priority indicators.

### Mobile View
Slide-in sidebar navigation with touch-friendly interface.

## 🛠️ Built With

**Backend:**
- [ASP.NET Core 8](https://dotnet.microsoft.com/apps/aspnet)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [SQLite](https://www.sqlite.org/)
- [AutoMapper](https://automapper.org/)
- [FluentValidation](https://fluentvalidation.net/)
- [Serilog](https://serilog.net/)

**Frontend:**
- [Next.js 14](https://nextjs.org/)
- [React 18](https://react.dev/)
- [TypeScript](https://www.typescriptlang.org/)
- [Tailwind CSS](https://tailwindcss.com/)
- [Axios](https://axios-http.com/)

## 📝 License

This project is licensed under the MIT License.

## 👤 Author

**Sarah**

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!

## ⭐ Show your support

Give a ⭐️ if you like this project!
