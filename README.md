# 🎬 Streamify - Online Movie & Series Streaming Platform

<div align="center">

*Your Ultimate Destination for Movies and Series Streaming*

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)]()
[![Version](https://img.shields.io/badge/version-2.0.0-orange.svg)]()

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
- [🔐 Security Notes](#-security-notes)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

---

## 🎯 Overview

**Streamify** is a modern, full-featured online movie and series streaming platform built with **.NET 10** and **Entity Framework Core 10**. The platform follows **Clean Architecture** principles with a clear separation between Core, Application, Infrastructure, and Presentation layers.

### 🎥 What Makes Streamify Special?

- **Clean Architecture**: Domain-Driven Design with proper layer separation and dependency inversion
- **Fully Async Pipeline**: Every repository, service and controller action is async end-to-end
- **Real Soft Delete**: Global EF Core query filters hide soft-deleted content everywhere — including favorites/watchlist join rows
- **Role-Based Access**: Admin and User roles with automatic seeding on startup
- **Free / Premium Content Model**: Each movie and series can be marked as free or premium (`IsFree` flag); watching and downloading premium content requires authentication
- **Server-Side Search, Filter & Pagination**: Movies and series listings support text search, category filter, sorting, and page navigation
- **Star Ratings & Reviews**: Users rate movies 1–5 stars; the movie's rating is recalculated from real user reviews
- **Areas-Based Structure**: Separate Admin and Customer areas for clean routing

---

## ✨ Features

### 🎬 Core Content
- **Movies**: Browse with search/category/sort/pagination, details page, in-app watch player, trailer section, download (auth required)
- **Series & Episodes**: Full series management with an episode player (`/Series/View/{id}?episodeId=`), episode listing, per-episode view/download tracking
- **Categories (Genres)**: Content organized by categories, used for filtering in listings

### ⭐ Reviews & Ratings
- Authenticated users post reviews with a **1–5 star rating** (anti-forgery protected AJAX)
- Movie rating is **aggregated from real reviews** (`RecalculateMovieRatingAsync`) — no more hardcoded static rating
- Reviews display with star icons on the movie details page

### 🔐 Authentication & Authorization
- ASP.NET Core Identity with role-based authorization (`Admin`, `User`)
- Cookie-based authentication (HttpOnly, SameSite=Lax, sliding expiration)
- Login / Register / Profile / Edit Profile / Access Denied pages
- **Seeded admin account read from configuration** — never hardcoded (see [Configuration](#️-configuration))
- Paywall: `Watch` and `Download` actions require `[Authorize]`; premium downloads require the Admin role (subscription integration point)

### 🛠️ Admin Panel
- Manage **Movies** (Create, Edit, Delete with confirmation + POST/anti-forgery)
- Manage **Series** (Create, Edit, Delete with confirmation + POST/anti-forgery)
- Manage **Episodes** — full CRUD (previously dead code with no UI)
- Manage **Categories** (Create, Edit, Delete via confirmation page + POST)
- User Management with role assignment (Admin-only, protected from self-deletion)

### 📊 Content Tracking
- View count and download count per movie and episode (atomic `ExecuteUpdate` increments)
- Aggregated star rating per movie
- Release year tracking

---

## 🏗️ Architecture

Streamify follows **Clean Architecture** with four distinct layers:

```
├── 🧠 Core/                        
│   ├── Models/                       ← namespace: Core.Models
│   │   ├── CommonData.cs               ← Abstract base (audit + soft delete)
│   │   ├── ApplicationUser.cs          ← IdentityUser extension
│   │   ├── Movie.cs                    ← Movie entity (domain methods + counters)
│   │   ├── Series.cs                   ← Series entity
│   │   ├── Episode.cs                  ← Episode entity
│   │   ├── Review.cs                   ← Review entity (with 1-5 Rating)
│   │   ├── UserMovie.cs                ← Favorites/watchlist join (movies)
│   │   ├── UserSeries.cs               ← Favorites/watchlist join (series)
│   │   └── Category.cs                 ← Category/Genre entity
│   └── Repositories/                 ← namespace: Core.Repositories (all async)
│       ├── IGenericRepository.cs       ← Base async CRUD contract
│       ├── IMovieRepository.cs         ← + SearchAsync, ExecuteUpdate counters, UpdateRatingAsync
│       ├── ISeriesRepository.cs
│       ├── IEpisodeRepository.cs
│       ├── ICategoryRepository.cs
│       ├── IReviewRepository.cs        ← + GetMovieAverageRatingAsync
│       ├── IUserMovieRepository.cs
│       └── IUserSeriesRepository.cs
│
├── 📦 Application/                      ← namespaces: BLL.Services.*, BLL.ViewModels
│   ├── Services/
│   │   ├── Abstraction/                ← Movie, Series, Episode, Category,
│   │   │                                 Account, Review, UserMovie, UserSeries
│   │   └── Implementation/             ← fully async implementations
│   ├── ViewModels/                     ← MovieVM, SeriesVM, EpisodeVM, CategoryVM,
│   │                                     ProfileVM, LoginUserVM, RegisterUserVM, UserVM
│   │                                     (all with DataAnnotations validation)
│   ├── Mapping/
│   │   └── DomainProfile.cs            ← AutoMapper configuration
│   └── Helpers/
│       └── Load.cs                     ← Hardened file upload (whitelist, size limit,
│                                             path-traversal guard)
│
├── 🗄️ Infrastructure/               
│   ├── Presistance/Database/
│   │   └── ApplicationDbContext.cs     ← EF Core 10 DbContext + global query filters
│   ├── Repositories/Implementation/    ← async, AsNoTracking reads, soft-delete-aware
│   └── Migrations/
│       └── ..._CleanBaseline.cs        ← Single clean baseline migration
│
└── 🌐 Movie Streamer Platform/         
    ├── Areas/
    │   ├── Admin/
    │   │   ├── Controllers/            ← Home, Movie, Series, Episode, Category, User
    │   │   └── Views/                  ← incl. new Episode CRUD views
    │   └── Customer/
    │       ├── Controllers/HomeController.cs
    │       └── Views/
    ├── Controllers/
    │   ├── AccountController.cs        ← Login/Register/Logout/Profile/EditProfile
    │   ├── MoviesController.cs         ← Index(search,categoryId,sortBy,page) Watch Download
    │   ├── SeriesController.cs         ← Index(...) Details View(episode player) Download
    │   ├── ReviewController.cs         ← Create(rating) + GetMovieReviews
    │   ├── SubscriptionController.cs
    │   └── ErrorController.cs
    ├── Seed/
    │   └── DbSeeder.cs                 ← Migrate + roles + admin (from config) + demo data
    ├── Views/
    │   ├── Account/       (Login, Register, Profile, EditProfile, AccessDenied)
    │   ├── Movies/        (Index, Details, Watch)
    │   ├── Series/        (Index, Details, View=episode player)
    │   ├── Home/          (Index, Privacy)
    │   ├── Subscriptions/ (Index)
    │   ├── Error/         (NotFound)
    │   └── Shared/        (_Layout, Error)
    └── Program.cs                      ← Clean Identity wiring + async seeding
```

### 🧱 Domain Model Highlights

**CommonData** — abstract base class inherited by all content entities:

| Field | Type | Notes |
|---|---|---|
| CreatedAt | DateTime? | Auto-set on creation |
| UpdatedAt | DateTime? | Auto-set on update |
| IsDeleted | bool? | Soft delete flag (hidden by global query filters) |
| DeletedAt | DateTime? | Timestamp of deletion |
| CreatedBy | string? | Username who created |
| UpdatedBy | string? | Username who updated |
| DeletedBy | string? | Username who deleted |

**Soft delete is real**: `Movie`, `Series`, `Category`, `Episode`, `Review` all carry global query filters (`IsDeleted == false`), and the favorites/watchlist join entities (`UserMovie`, `UserSeries`) have matching filters that follow their principal — a soft-deleted movie's favorites disappear from every query automatically.

**Review** fields: `UserId`, `MovieId`, `Comment`, `Rating` (1–5, clamped), `AddedAt`

---

## 🚀 Getting Started

### 📋 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or full instance)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### 🎯 Tech Stack

| Layer | Technology |
|---|---|
| Backend | .NET 10, ASP.NET Core MVC |
| Database | SQL Server + Entity Framework Core 10 |
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
    "DefaultConnection": "Server=localhost;Database=Movie;Trusted_Connection=True;Encrypt=False;"
  }
}
```

> ⚠️ **Upgrading from v1.x (old `DAL.Models` schema)?** The migration history was replaced by a single clean baseline (`CleanBaseline`). Drop the old database first:
> ```bash
> dotnet ef database drop --force --project Infrastructure --startup-project "Movie Streamer Platform"
> ```

### 4️⃣ Run the Application

```bash
dotnet run --project "Movie Streamer Platform"
```

On first run the app **automatically applies migrations and seeds**:
- Roles: `Admin`, `User`
- Admin account (from `Seed:AdminEmail` / `Seed:AdminPassword` in Development settings)
- A demo catalogue (categories, sample movies with public sample-video URLs, one series with episodes)

Migrations can also be applied manually:

```bash
dotnet ef database update --project Infrastructure --startup-project "Movie Streamer Platform"
```

### 5️⃣ Create New Migrations

```bash
dotnet ef migrations add <Name> --project Infrastructure --startup-project "Movie Streamer Platform"
```

---

## ⚙️ Configuration

### appsettings.Development.json (seed credentials stay OUT of source)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Movie;Trusted_Connection=True;Encrypt=False;"
  },
  "Seed": {
    "AdminEmail": "admin@gmail.com",
    "AdminPassword": "Admin@123"
  }
}
```

For production, set these via environment variables or a secrets manager — the seeder only creates the admin when the `Seed` section is present (Development).

### Routing

The app uses area-based routing:

| Area | Pattern | Purpose |
|---|---|---|
| Admin | `/Admin/{controller}/{action}/{id?}` | Admin dashboard (Admin role only) |
| Customer | `/Customer/{controller}/{action}/{id?}` | Customer-facing pages |
| Default | `/{controller=Home}/{action=Index}/{id?}` | Public pages |

---

## 📱 Screenshots

### 🏠 Home Page
<img width="1357" height="640" alt="Home Page" src="https://github.com/user-attachments/assets/574674cc-488b-4aa5-845b-a1267d2a844e" />

### 💳 Subscription Page
<img width="1356" height="639" alt="Subscription" src="https://github.com/user-attachments/assets/3d5b9aac-f1b4-48e3-9b6d-a8fa2be867fa" />

### 💰 Payment Integration
<img width="1354" height="637" alt="Payment" src="https://github.com/user-attachments/assets/648f916e-92a3-a0f28702e1cb" />

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
<img width="928" height="531" alt="image" src="https://github.com/user-attachments/assets/2d4d6b9c-905e-4b22-a4aa-e45d5dbb6098" />

### Profile
<img width="894" height="573" alt="image" src="https://github.com/user-attachments/assets/7e048208-07fe-47f3-974a-a9c15b1a2def" />

---

## 🔐 Security Notes

- **Anti-forgery tokens** on every state-changing POST (admin CRUD, favorites/watchlist toggles, reviews)
- **Admin deletes are POST-only** with confirmation dialogs — no destructive GET links
- **Authorization**: Admin area restricted to the `Admin` role; Watch/Download require login; premium downloads are Admin-gated
- **Hardened uploads**: extension whitelist, size limit, path-traversal-safe file naming
- **Seed credentials** are read from configuration, never hardcoded

---

## 🗺️ Development Roadmap

### ✅ Completed
- [x] Core domain models with soft delete, audit trail, and global query filters
- [x] Fully async repository/service/controller pipeline
- [x] Movie CRUD with category, trailer, rating, and free/premium flag
- [x] Series & **Episode** management (full admin CRUD)
- [x] Category management
- [x] ASP.NET Identity with Admin/User roles + config-driven admin seeding
- [x] Admin area with full content management
- [x] User watchlist and favorites (movies + series, soft-delete aware)
- [x] **User star ratings and reviews with aggregated movie rating**
- [x] **Search, category filter, sorting, and pagination on listings**
- [x] **In-app watch pages (movie player + series episode player)**
- [x] **Free/Premium badges and paywall gates on Watch/Download**

### 🔄 In Progress / Planned
- [ ] Subscription plans & payment processing (Stripe)
- [ ] Watch history with resume support
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
- Keep domain logic inside entity methods (e.g., `movie.IncrementViews()`, `Review.Create(...)`)
- All new I/O-bound code must be async
- All POST endpoints require `[ValidateAntiForgeryToken]`

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
