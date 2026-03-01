# 🎫 Ticket System

A full-stack ticket management system built with **.NET 8**, featuring a **WPF desktop client** and an **ASP.NET Core Web API** backend with **Oracle Database**.

---

## 📐 Architecture

| Layer        | Project                 | Technology                    |
| ------------ | ----------------------- | ----------------------------- |
| **Frontend** | `Ticket_System_App`     | WPF (.NET 8, MVVM)            |
| **Backend**  | `Ticket_System_Backend` | ASP.NET Core Web API (.NET 8) |
| **Database** | Oracle XE               | Oracle via Docker             |
| **ORM**      | Entity Framework Core 8 | Oracle.EntityFrameworkCore    |

---

## ✨ Features

- **Ticket Management** — Create, update, view, and delete tickets
- **Comment System** — Add comments/discussions to tickets
- **Status Tracking** — Full status history for every ticket
- **User Management** — User CRUD and role-based access
- **Authentication** — JWT Bearer token authentication
- **Real-time Ready** — Architecture supports SignalR integration

---

## 🗂️ Project Structure

```
Ticket_Project.sln
├── Ticket_System_Backend/        # ASP.NET Core Web API
│   ├── Controllers/              # API endpoints
│   │   ├── AuthController        # Login / authentication
│   │   ├── TicketsController     # Ticket CRUD
│   │   ├── CommentsController    # Comments on tickets
│   │   ├── StatusHistoryController
│   │   └── UsersController       # User management
│   ├── Models/                   # EF Core entities
│   ├── DTOs/                     # Request/Response objects
│   ├── Repositories/             # Data access layer
│   ├── Services/                 # Business logic layer
│   ├── Middlewares/              # Custom middleware
│   ├── Migrations/               # EF Core migrations
│   └── Data/                     # DbContext configuration
│
├── Ticket_System_App/            # WPF Desktop Client
│   ├── Views/                    # XAML UI pages
│   ├── ViewModels/               # MVVM view models
│   ├── Models/                   # Client-side models
│   ├── Services/                 # API service clients
│   └── Helpers/                  # Utility classes
│
└── docker-compose.yml            # Oracle DB container
```

---

## 🛠️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for Oracle DB)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommended)

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/<your-username>/Ticket_Project.git
cd Ticket_Project
```

### 2. Start the Oracle Database

```bash
docker-compose up -d
```

This spins up an **Oracle XE** container on port `1521`.

### 3. Apply EF Core Migrations

```bash
cd Ticket_System_Backend
dotnet ef database update
```

### 4. Run the Backend API

```bash
dotnet run --project Ticket_System_Backend
```

The API will be available at `https://localhost:5001` with **Swagger UI** at `/swagger`.

### 5. Run the WPF Client

Open the solution in Visual Studio, set `Ticket_System_App` as the startup project, and press **F5**.

---

## 🔑 API Endpoints

| Method   | Endpoint                        | Description               |
| -------- | ------------------------------- | ------------------------- |
| `POST`   | `/api/auth/login`               | Authenticate & get JWT    |
| `GET`    | `/api/tickets`                  | List all tickets          |
| `POST`   | `/api/tickets`                  | Create a new ticket       |
| `PUT`    | `/api/tickets/{id}`             | Update a ticket           |
| `DELETE` | `/api/tickets/{id}`             | Delete a ticket           |
| `GET`    | `/api/comments/{ticketId}`      | Get comments for a ticket |
| `POST`   | `/api/comments`                 | Add a comment             |
| `GET`    | `/api/statushistory/{ticketId}` | Get status history        |
| `GET`    | `/api/users`                    | List all users            |
| `POST`   | `/api/users`                    | Create a user             |

---

## ⚙️ Configuration

Update `appsettings.json` in the backend project to configure:

- **Database connection string** (Oracle)
- **JWT settings** (secret key, issuer, audience, expiration)

---

## 🧰 Tech Stack

- **Runtime:** .NET 8
- **Backend:** ASP.NET Core Web API
- **Frontend:** WPF (Windows Presentation Foundation)
- **Database:** Oracle XE (via Docker)
- **ORM:** Entity Framework Core 8
- **Auth:** JWT Bearer Tokens
- **API Docs:** Swagger / Swashbuckle
- **Containerization:** Docker Compose

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
