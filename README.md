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
- [🔧 API Documentation](#-api-documentation)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)
- [👥 Authors](#-authors)
- [🙏 Acknowledgments](#-acknowledgments)

---

## 🎯 Overview

**Streamify** is a modern, full-featured online movie and series streaming platform built with .NET 8 and Entity Framework Core. The platform provides users with a seamless streaming experience, featuring user management, subscription handling, payment processing, and content management capabilities.

### 🎥 What Makes Streamify Special?

- **N-tier Architecture**: Implements Domain-Driven Design with clear separation of concerns
- **Modern UI**: Responsive design with intuitive user experience
- **Secure**: Robust authentication and authorization system
- **Payment Integration**: Seamless payment processing for subscriptions
- **Admin Panel**: Comprehensive content and user management

---

## ✨ Features

### 🎬 Core Features
- **Movie & Series Streaming**: High-quality video streaming with adaptive bitrate
- **User Management**: Registration, authentication, and profile management
- **Subscription System**: Multiple subscription tiers with different access levels
- **Watchlist**: Personal watchlist and viewing history
- 🌍 **Localization & Globalization** – Multi-language support for a wider audience.  
- 🕒 **Background Jobs** – Automated tasks running in the background (e.g., subscription checks, data cleanup).  
- 📧 **Email Notifications** – Automatic email sending for account verification, password reset, and subscription updates.  

### 🔐 Authentication & Security
- Role-based authorization (Admin, User)
- Secure password hashing
- Session management

### 💳 Payment & Subscriptions
- Subscription management
- Billing history

### 📊 Administrative Features
- Content management system
- Subscription analytics
- System monitoring

---

## 🏗️ Architecture

Streamify follows **Clean Architecture** principles with a clear separation of concerns:

```
├── 🎯 BLL (Business Logic Layer)/
│   ├── 📁 Dependencies/
│   ├── 📁 Helpers/
│   │   └── 📄 Load.cs
│   ├── 📁 Mapping/
│   │   └── 📄 DomainProfile.cs
│   ├── 📁 Services/
│   │   ├── 📁 Abstraction/
│   │   │   ├── 📄 IAccountService.cs
│   │   │   ├── 📄 ICategoryService.cs
│   │   │   ├── 📄 IEpisodeService.cs
│   │   │   ├── 📄 IMovieService.cs
│   │   │   ├── 📄 IPaymentService.cs
│   │   │   ├── 📄 ISeriesService.cs
│   │   │   └── 📄 IUserManagerService.cs
│   │   └── 📁 Implementation/
│   │       ├── 📄 AccountService.cs
│   │       ├── 📄 CategoryService.cs
│   │       ├── 📄 EpisodeService.cs
│   │       ├── 📄 MovieService.cs
│   │       ├── 📄 PaymentService.cs
│   │       ├── 📄 SeriesService.cs
│   │       ├── 📄 StripePaymentService.cs
│   │       └── 📄 UserManagerService.cs
│   │
│   ├── 📁 ViewModels/
│   │   ├── 📄 CategoryVM.cs
│   │   ├── 📄 EpisodeVM.cs
│   │   ├── 📄 LoginUserVM.cs
│   │   ├── 📄 MovieVM.cs
│   │   ├── 📄 RegisterUserVM.cs
│   │   ├── 📄 SeriesVM.cs
│   │   └── 📄 UserVM.cs
│
├── 🗄️ DAL (Data Access Layer)/
│   ├── 📁 Dependencies/
│   ├── 📁 Database/
│   │   └── 📄 ApplicationDbContext.cs
│   ├── 📁 Migrations/
│   ├── 📁 Models/
│   │   ├── 📄 ApplicationUser.cs
│   │   ├── 📄 Category.cs
│   │   ├── 📄 CommonData.cs
│   │   ├── 📄 Episode.cs
│   │   ├── 📄 ErrorViewModel.cs
│   │   ├── 📄 Movie.cs
│   │   └── 📄 Series.cs
│   └── 📁 Repositories/
│       ├── 📁 Abstraction/
│       │   ├── 📄 ICategoryRepository.cs
│       │   ├── 📄 IEpisodeRepository.cs
│       │   ├── 📄 IGenericRepository.cs
│       │   ├── 📄 IMovieRepository.cs
│       │   └── 📄 ISeriesRepository.cs
│       └── 📁 Implementation/
│           ├── 📄 CategoryRepository.cs
│           ├── 📄 EpisodeRepository.cs
│           ├── 📄 GenericRepository.cs
│           ├── 📄 MovieRepository.cs
│           └── 📄 SeriesRepository.cs
│
└── 🌐 PL (Presentation Layer)/
    ├── 📁 Connected Services/
    ├── 📁 Dependencies/
    ├── 📁 Properties/
    ├── 📁 wwwroot/
    ├── 📁 Areas/
    │   ├── 📁 Admin/
    │   │   └── 📁 Views/
    │   └── 📁 Customer/
    │       └── 📁 Views/
    ├── 📁 Controllers/
    │   ├── 📄 AccountController.cs
    │   ├── 📄 ErrorController.cs
    │   ├── 📄 HomeController.cs
    │   ├── 📄 MoviesController.cs
    │   ├── 📄 PaymentController.cs
    │   ├── 📄 SeriesController.cs
    │   ├── 📄 SubscriptionController.cs
    │   └── 📄 UserManagerController.cs
    ├── 📁 Views/
    │   ├── 📁 Account/
    │   ├── 📁 Error/
    │   ├── 📁 Home/
    │   ├── 📁 Movies/
    │   ├── 📁 Payment/
    │   ├── 📁 Series/
    │   ├── 📁 Shared/
    │   └── 📁 Subscriptions/
    ├── 📄 _ViewImports.cshtml
    ├── 📄 _ViewStart.cshtml
    ├── 📄 appsettings.json
    ├── 📄 Class1.cs
    └── 📄 Program.cs

```

### 🎯 Business Logic Layer (BLL)

#### 📋 Service Abstractions
- **IAccountService.cs** - User authentication and account management interface
- **ICategoryService.cs** - Content category management interface  
- **IEpisodeService.cs** - TV series episode management interface
- **IMovieService.cs** - Movie content management interface
- **IPaymentService.cs** - Payment processing interface
- **ISeriesService.cs** - TV series management interface
- **IUserManagerService.cs** - User administration interface

#### ⚙️ Service Implementations  
- **AccountService.cs** - Handles user registration, login, profile management
- **CategoryService.cs** - Manages content categories and genres
- **EpisodeService.cs** - Handles episode CRUD operations and metadata
- **MovieService.cs** - Manages movie content, metadata, and streaming
- **PaymentService.cs** - Core payment processing logic
- **SeriesService.cs** - Handles TV series operations and season management
- **StripePaymentService.cs** - Stripe payment gateway integration
- **UserManagerService.cs** - Advanced user management and administration

#### 🔧 Utilities
- **Load.cs** - Helper class for data loading and initialization
- **DomainProfile.cs** - AutoMapper configuration for entity-to-DTO mappings

### 🗄️ Data Access Layer (DAL)

#### 🏛️ Database Context
- **ApplicationDbContext.cs** - Entity Framework Core context with all entity configurations

#### 📊 Domain Models
- **ApplicationUser.cs** - Extended Identity user with streaming platform specific properties
- **Category.cs** - Content categorization and genre management
- **CommonData.cs** - Shared data models and common properties
- **Episode.cs** - Individual episode entity with series relationships  
- **ErrorViewModel.cs** - Error handling and display model
- **Movie.cs** - Movie entity with metadata, ratings, and streaming info
- **Series.cs** - TV series entity with episode collections and season data

#### 🔍 Repository Abstractions
- **ICategoryRepository.cs** - Category data access interface
- **IEpisodeRepository.cs** - Episode-specific data operations interface
- **IGenericRepository.cs** - Base repository pattern for common CRUD operations
- **IMovieRepository.cs** - Movie-specific data access interface
- **ISeriesRepository.cs** - Series data management interface

#### 💾 Repository Implementations
- **CategoryRepository.cs** - Category data access implementation with advanced queries
- **EpisodeRepository.cs** - Episode data operations with series relationships
- **GenericRepository.cs** - Base repository implementation for all entities
- **MovieRepository.cs** - Movie data access with filtering, searching, and streaming
- **SeriesRepository.cs** - Series data management with episode handling

### 🌐 Presentation Layer (PL)

#### 🎮 Controllers
- **AccountController.cs** - User authentication, registration, and profile management
- **ErrorController.cs** - Global error handling and custom error pages
- **HomeController.cs** - Landing page, dashboard, and general navigation
- **MoviesController.cs** - Movie browsing, details, and streaming interface
- **PaymentController.cs** - Payment processing, billing, and transaction handling
- **SeriesController.cs** - TV series browsing, episode navigation, and streaming
- **SubscriptionController.cs** - Subscription plans, upgrades, and management
- **UserManagerController.cs** - Administrative user management functionality

#### 📱 View Models
- **CategoryVM.cs** - Category display and management view model
- **EpisodeVM.cs** - Episode information and streaming view model
- **LoginUserVM.cs** - User login form and validation model
- **MovieVM.cs** - Movie display and interaction view model
- **RegisterUserVM.cs** - User registration form with validation
- **SeriesVM.cs** - Series information and episode listing model
- **UserVM.cs** - User profile and management view model

#### 🎨 View Structure
- **Account Views** - Login, registration, profile management interfaces
- **Error Views** - Custom error pages and exception handling
- **Home Views** - Landing pages, dashboard, and navigation
- **Movies Views** - Movie library, details, and player interfaces
- **Payment Views** - Checkout, billing history, and payment method management
- **Series Views** - Series library, season/episode navigation, and player
- **Shared Views** - Common layouts, partial views, and components
- **Subscriptions Views** - Plan selection, account management, and billing

#### ⚙️ Configuration Files
- **Program.cs** - Application startup, dependency injection, and middleware configuration
- **appsettings.json** - Application configuration, connection strings, and settings
- **_ViewImports.cshtml** - Global view imports and tag helpers
- **_ViewStart.cshtml** - Default layout and view configuration

---

## 🚀 Getting Started

### 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Full)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Node.js](https://nodejs.org/) (for client-side dependencies)
- [Git](https://git-scm.com/)

### 🎯 Tech Stack

- **Backend**: .NET 8, ASP.NET Core MVC
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap
- **Authentication**: ASP.NET Core Identity
- **Mapping**: AutoMapper
- **Testing**: xUnit, Moq
- **Containerization**: Docker (optional)

---

## 📦 Installation

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/yourusername/streamify.git
cd streamify
```

### 2️⃣ Restore Dependencies

```bash
# Restore NuGet packages
dotnet restore

# Install client-side packages (if using npm)
npm install
```

### 3️⃣ Database Setup

```bash
# Update connection string in appsettings.json
# Run migrations
dotnet ef database update --project DAL --startup-project PL

# Or using Package Manager Console in Visual Studio
Update-Database
```

### 4️⃣ Build and Run

```bash
# Build the solution
dotnet build

# Run the application
dotnet run --project PL
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

---

## ⚙️ Configuration

### 🔧 Application Settings

Update `appsettings.json` in the PL project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=StreamifyDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-here",
    "Issuer": "Streamify",
    "Audience": "StreamifyUsers",
    "ExpiryMinutes": 60
  },
  "PaymentSettings": {
    "StripePublishableKey": "pk_test_your_stripe_key",
    "StripeSecretKey": "sk_test_your_stripe_secret"
  }
}
```

### 🌍 Environment Variables

For production deployment, set these environment variables:

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=your_production_connection_string
JwtSettings__SecretKey=your_production_secret_key
```

---


## 📱 Screenshots

### 👤 User Registration & Login

![User Registration](screenshots/registration.png)

Users can register for new accounts or login with existing credentials. The platform supports:
- Email verification
- Password reset functionality
- Social login options (Google, Facebook)

### 🏠 Home & Content Discovery

#### Home Page
<img width="1357" height="640" alt="image" src="https://github.com/user-attachments/assets/574674cc-488b-4aa5-845b-a1267d2a844e" />

The home page features:
- Featured movies and series
- Trending content
- Personalized recommendations
- Category-based browsing

### 💳 Subscription Management

#### Subscription Page
<img width="1356" height="639" alt="image" src="https://github.com/user-attachments/assets/3d5b9aac-f1b4-48e3-9b6d-a8fa2be867fa" />

#### Payment Integration
<img width="1354" height="637" alt="image" src="https://github.com/user-attachments/assets/648f916e-8694-46f3-92a3-a0f28702e1cb" />


Users can:
- Choose from multiple subscription plans
- Manage payment methods
- View billing history
- Cancel or upgrade subscriptions

### 🛠️ Admin Dashboard
#### Movies Management
<img width="1366" height="635" alt="image" src="https://github.com/user-attachments/assets/2a107253-19af-4a20-b5d5-e58701bf6819" />

####  Series Management
<img width="1366" height="639" alt="image" src="https://github.com/user-attachments/assets/477c49dc-f441-4ae2-8e85-f42bb44aea63" />

#### Useres Management
<img width="1363" height="635" alt="image" src="https://github.com/user-attachments/assets/7a793106-0700-45ce-b06b-65649437a5b2" />

### 🛠️ Admin Dashboard
#### View Movies
<img width="1344" height="632" alt="image" src="https://github.com/user-attachments/assets/43b04349-4290-4e22-8f23-27fda2df6faa" />

#### View Series
<img width="1348" height="637" alt="image" src="https://github.com/user-attachments/assets/763277c2-905e-4b22-b814-79f54530657a" />


Administrative features include:
- Content management
- User management
- Analytics and reporting
- System configuration

---


## 🔧 API Documentation

### 🎬 Movies Endpoints

```http
GET /api/movies
GET /api/movies/{id}
POST /api/movies
PUT /api/movies/{id}
DELETE /api/movies/{id}
```

### 📺 Series Endpoints

```http
GET /api/series
GET /api/series/{id}
GET /api/series/{id}/episodes
POST /api/series
PUT /api/series/{id}
DELETE /api/series/{id}
```

### 👤 User Endpoints

```http
POST /api/auth/register
POST /api/auth/login
POST /api/auth/refresh
GET /api/users/profile
PUT /api/users/profile
```

### 💳 Subscription Endpoints

```http
GET /api/subscriptions/plans
POST /api/subscriptions/subscribe
GET /api/subscriptions/current
PUT /api/subscriptions/cancel
```

### 📊 Example API Response

```json
{
  "success": true,
  "data": {
    "id": 1,
    "title": "The Matrix",
    "description": "A computer programmer discovers reality is a simulation.",
    "genre": "Science Fiction",
    "releaseYear": 1999,
    "duration": 136,
    "rating": 8.7,
    "posterUrl": "/images/matrix-poster.jpg",
    "trailerUrl": "/videos/matrix-trailer.mp4"
  },
  "message": "Movie retrieved successfully"
}
```

---

## 🏛️ Database Schema

### 📊 Entity Relationship Diagram

#### DataBase Schema
<img width="794" height="521" alt="image" src="https://github.com/user-attachments/assets/d91426f5-7c0f-4e85-b280-f1bdcf755a4f" />


---
## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details.

### 🔄 Development Workflow

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** your changes (`git commit -m 'Add amazing feature'`)
4. **Push** to the branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

### 📝 Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Write unit tests for new features
- Ensure all tests pass before submitting PR

### 🐛 Bug Reports

When reporting bugs, please include:
- Steps to reproduce
- Expected vs actual behavior
- Environment details
- Screenshots if applicable

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

```
MIT License

Copyright (c) 2024 Streamify Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
```

---

## 👥 Authors

- **Mohammed Atef** - *Backend Development* - [YourGitHub](https://github.com/Mohammed-Atef2004)
- **Shady Atef** - *Frontend development* - [GitHub]([https://github.com/member2](https://github.com/shady-ateff))

---

## 🙏 Acknowledgments

- **Bootstrap** - For the responsive UI framework
- **Entity Framework Team** - For the excellent ORM
- **ASP.NET Core Team** - For the robust web framework
- **Community Contributors** - For their valuable feedback and contributions
- **Font Awesome** - For the beautiful icons
- **Unsplash** - For the stock photos used in development

---

## 📞 Support

Need help? We're here for you!

- 📧 **Email**: muhamedatef.82@gmail.com


---


<div align="center">

**Made with ❤️ by the Streamify Team**

[⬆ Back to Top](#-streamify---online-movie--series-streaming-platform)

</div>
