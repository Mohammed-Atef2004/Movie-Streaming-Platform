# 🎬 Streamify - Online Movie & Series Streaming Platform

<div align="center">

*Your Ultimate Destination for Movies and Series Streaming*

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)]()
[![Version](https://img.shields.io/badge/version-1.0.0-orange.svg)]()

</div>

---

## 📋 Table of Contents

- [🎯 Overview](#-overview)
- [✨ Features](#-features)
- [🏗️ Architecture](#️-architecture)
- [🚀 Getting Started](#-getting-started)
- [📦 Installation](#-installation)
- [⚙️ Configuration](#️-configuration)
- [📱 Screenshots](#-screenshots)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)
- [👥 Authors](#-authors)

---

## 🎯 Overview

**Streamify** is a modern, full-featured online movie and series streaming platform built with **.NET 8** and **Entity Framework Core**. The platform follows **Clean Architecture** principles with a clear separation between Core, Application, Infrastructure, and Presentation layers.

### 🎥 What Makes Streamify Special?

- **Clean Architecture**: Domain-Driven Design with proper layer separation and dependency inversion
- **Soft Delete Pattern**: All entities support soft delete with full audit trail (CreatedAt, UpdatedAt, DeletedAt, CreatedBy, UpdatedBy, DeletedBy)
- **Role-Based Access**: Admin and User roles with automatic seeding on startup
- **Free / Premium Content Model**: Each movie and series can be marked as free or premium (`IsFree` flag)
- **Areas-Based Structure**: Separate Admin and Customer areas for clean routing

---

## ✨ Features

### 🎬 Core Content
- **Movies**: Browse, view details, stream movies with trailer and main video support
- **Series & Episodes**: Full series management with episode-level video URLs and view/download tracking
- **Categories (Genres)**: Content organized by categories with many-to-many mapping to movies

### 🔐 Authentication & Authorization
- ASP.NET Core Identity with role-based authorization (`Admin`, `User`)
- Cookie-based authentication
- Custom Login / Register / Access Denied pages
- Auto-seeded admin account on first run (`admin@gmail.com` / `Admin@123`)

### 🛠️ Admin Panel
- Manage Movies (Create, Edit, View All, Soft Delete)
- Manage Series (Create, Edit, View All, Soft Delete)
- Manage Categories (Create, Edit, Delete)
- User Management with role assignment

### 💳 Subscriptions
- Subscription plans page (UI ready, business logic in roadmap)

### 📊 Content Tracking
- View count and download count per movie and episode
- Rating field per movie (default 5.0)
- Release year tracking

---

## 🏗️ Architecture

Streamify follows **Clean Architecture** with four distinct layers:

```
├── 🧠 Core/                        
│   ├── Models/
│   │   ├── CommonData.cs               ← Abstract base (audit + soft delete)
│   │   ├── ApplicationUser.cs          ← IdentityUser extension
│   │   ├── Movie.cs                    ← Movie entity
│   │   ├── Series.cs                   ← Series entity
│   │   ├── Episode.cs                  ← Episode entity
│   │   └── Category.cs                 ← Category/Genre entity
│   └── Repositories/
│       ├── IGenericRepository.cs       ← Base CRUD contract
│       ├── IMovieRepository.cs
│       ├── ISeriesRepository.cs
│       ├── IEpisodeRepository.cs
│       └── ICategoryRepository.cs
│
├── 📦 Application/                     
│   ├── Services/
│   │   ├── Abstraction/
│   │   │   ├── IMovieService.cs
│   │   │   ├── ISeriesService.cs
│   │   │   ├── IEpisodeService.cs
│   │   │   ├── ICategoryService.cs
│   │   │   ├── IAccountService.cs
│   │   │   └── IUserMangerService.cs
│   │   └── Implementation/
│   │       ├── MovieService.cs
│   │       ├── SeriesService.cs
│   │       ├── EpisodeService.cs
│   │       ├── CategoryService.cs
│   │       ├── AccountService.cs
│   │       └── UserMangerService.cs
│   ├── ViewModels/
│   │   ├── MovieVM.cs
│   │   ├── SeriesVM.cs
│   │   ├── EpisodeVM.cs
│   │   ├── CategoryVM.cs
│   │   ├── LoginUserVM.cs
│   │   ├── RegisterUserVM.cs
│   │   └── UserVM.cs
│   ├── Mapping/
│   │   └── DomainProfile.cs            ← AutoMapper configuration
│   └── Helpers/
│       └── Load.cs
│
├── 🗄️ Infrastructure/                 
│   ├── Presistance/Database/
│   │   └── ApplicationDbContext.cs     ← EF Core DbContext
│   ├── Repositories/Implementation/
│   │   ├── GenericRepository.cs
│   │   ├── MovieRepository.cs
│   │   ├── SeriesRepository.cs
│   │   ├── EpisodeRepository.cs
│   │   └── CategoryRepository.cs
│   └── Migrations/
│
└── 🌐 Movie Streamer Platform/         
    ├── Areas/
    │   ├── Admin/
    │   │   ├── Controllers/
    │   │   │   ├── HomeController.cs
    │   │   │   ├── MovieController.cs
    │   │   │   ├── SeriesController.cs
    │   │   │   ├── CategoryController.cs
    │   │   │   └── UserController.cs
    │   │   └── Views/
    │   └── Customer/
    │       ├── Controllers/
    │       │   └── HomeController.cs
    │       └── Views/
    ├── Controllers/
    │   ├── AccountController.cs
    │   ├── HomeController.cs
    │   ├── MoviesController.cs
    │   ├── SeriesController.cs
    │   ├── SubscriptionController.cs
    │   └── ErrorController.cs
    ├── Views/
    │   ├── Account/       (Login, Register, AccessDenied)
    │   ├── Movies/        (Index, Details, View)
    │   ├── Series/        (Index, Details, View)
    │   ├── Home/          (Index, Movies, Privacy)
    │   ├── Subscriptions/ (Index)
    │   ├── Error/         (NotFound)
    │   └── Shared/        (_Layout, Error)
    └── Program.cs
```

### 🧱 Domain Model Highlights

**CommonData** — abstract base class inherited by all entities:

| Field | Type | Notes |
|---|---|---|
| CreatedAt | DateTime? | Auto-set on creation |
| UpdatedAt | DateTime? | Auto-set on update |
| IsDeleted | bool? | Soft delete flag |
| DeletedAt | DateTime? | Timestamp of deletion |
| CreatedBy | string? | Username who created |
| UpdatedBy | string? | Username who updated |
| DeletedBy | string? | Username who deleted |

**Movie** fields: `Title`, `Description`, `ImageUrl`, `TrailerUrl`, `MovieUrl`, `IsFree`, `Rating`, `Year`, `Views`, `Downloads`, `CategoryId`

**Series** fields: `Title`, `Description`, `ImageUrl`, `IsFree`, `ViewCount`, `DownloadCount`, `CategoryId`

**Episode** fields: `Title`, `Description`, `VideoUrl`, `ViewCount`, `DownloadCount`, `SeriesId`

**Category** fields: `Name`, `Description` → has a collection of `Movies`

---

## 🚀 Getting Started

### 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or full instance)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### 🎯 Tech Stack

| Layer | Technology |
|---|---|
| Backend | .NET 8, ASP.NET Core MVC |
| Database | SQL Server + Entity Framework Core 8 |
| Auth | ASP.NET Core Identity (Cookie-based) |
| Mapping | AutoMapper |
| Frontend | HTML5, CSS3, JavaScript, Bootstrap |
| Admin UI | Custom admin template |

---

## 📦 Installation

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/yourusername/streamify.git
cd streamify
```

### 2️⃣ Restore Dependencies

```bash
dotnet restore
```

### 3️⃣ Configure the Database

Update the connection string in `Movie Streamer Platform/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=StreamifyDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 4️⃣ Run Migrations

```bash
dotnet ef database update --project Infrastructure --startup-project "Movie Streamer Platform"
```

Or in Package Manager Console:

```powershell
Update-Database
```

### 5️⃣ Run the Application

```bash
dotnet run --project "Movie Streamer Platform"
```

The app will be available at `https://localhost:7221` or `http://localhost:5257`.

> **First run**: An admin account is automatically seeded.
> - Email: `admin@gmail.com`
> - Password: `Admin@123`

---

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=StreamifyDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Routing

The app uses area-based routing:

| Area | Pattern | Purpose |
|---|---|---|
| Admin | `/Admin/{controller}/{action}/{id?}` | Admin dashboard |
| Customer | `/Customer/{controller}/{action}/{id?}` | Customer-facing pages |
| Default | `/{controller=Home}/{action=Index}/{id?}` | Public pages |

---

## 📱 Screenshots

### 🏠 Home Page
<img width="1357" height="640" alt="Home Page" src="https://github.com/user-attachments/assets/574674cc-488b-4aa5-845b-a1267d2a844e" />

### 💳 Subscription Page
<img width="1356" height="639" alt="Subscription" src="https://github.com/user-attachments/assets/3d5b9aac-f1b4-48e3-9b6d-a8fa2be867fa" />

### 💰 Payment Integration
<img width="1354" height="637" alt="Payment" src="https://github.com/user-attachments/assets/648f916e-8694-46f3-92a3-a0f28702e1cb" />

### 🛠️ Admin — Movies Management
<img width="1327" height="633" alt="image" src="https://github.com/user-attachments/assets/d4cd1d20-a067-4592-81c4-3af7d03008e8" />

### 🛠️ Admin — Series Management
<img width="1366" height="639" alt="Admin Series" src="https://github.com/user-attachments/assets/477c49dc-f441-4ae2-8e85-f42bb44aea63" />

### 👥 Admin — User Management
<img width="1363" height="635" alt="User Management" src="https://github.com/user-attachments/assets/7a793106-0700-45ce-b06b-65649437a5b2" />

### 🎬 Browse Movies
<img width="672" height="568" alt="image" src="https://github.com/user-attachments/assets/7a41f944-f8fb-4d21-a4aa-e45d5dbb6098" />


### 📺 Browse Series
<img width="1348" height="637" alt="View Series" src="https://github.com/user-attachments/assets/763277c2-905e-4b22-b814-79f54530657a" />

### 📺 Movie Details
<img width="928" height="531" alt="image" src="https://github.com/user-attachments/assets/2d4d6b9c-965c-46f6-992a-d1a8b08b8ac3" />

### Profile
<img width="894" height="573" alt="image" src="https://github.com/user-attachments/assets/7e048208-07fe-47f3-974a-a9c15b1a2def" />


---

## 🗺️ Development Roadmap

### ✅ Completed
- [x] Core domain models with soft delete and audit trail
- [x] Movie CRUD with category, trailer, rating, and free/premium flag
- [x] Series & Episode management
- [x] Category management
- [x] ASP.NET Identity with Admin/User roles
- [x] Admin area with full content management
- [x] Customer area with movie and series browsing
- [x] Generic repository pattern
- [x] AutoMapper integration
- [x] User watchlist and favorites

### 🔄 In Progress / Planned
- [ ] Subscription plans & payment processing (Stripe)
- [ ] Watch history with resume support
- [ ] User ratings and reviews
- [ ] Search and filtering by genre/year/rating
- [ ] Trending and top-rated sections
- [ ] Real-time features (SignalR)
- [ ] Caching layer (IMemoryCache → Redis)
- [ ] FluentValidation integration
- [ ] Unit and integration tests
- [ ] Docker support

---

## 🤝 Contributing

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** your changes (`git commit -m 'Add amazing feature'`)
4. **Push** to the branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

### Code Style Guidelines
- Follow C# coding conventions
- Keep domain logic inside entity methods (e.g., `movie.Create(...)`, `movie.Update(...)`)
- Do not expose setters on domain properties — use domain methods
- Write meaningful commit messages

---

## 📄 License

This project is licensed under the MIT License.

```
MIT License — Copyright (c) 2024 Streamify Team
```

---

## 👥 Authors

- **Mohammed Atef** — Backend Development — [GitHub](https://github.com/Mohammed-Atef2004)
- **Shady Atef** — Frontend Development — [GitHub](https://github.com/shady-ateff)

---

## 📞 Support

- 📧 Email: muhamedatef.82@gmail.com

---

<div align="center">

**Made with ❤️ by the Streamify Team**

[⬆ Back to Top](#-streamify---online-movie--series-streaming-platform)

</div>
