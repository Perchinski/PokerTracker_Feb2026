# ♠️ PokerTracker

## 📖 Project Concept & Features
**PokerTracker** is a sleek, high-performance web application designed to organize, manage, and track private poker tournaments and competitive series.

* 🔐 **Identity Management** — Secure registration and login powered by ASP.NET Core Identity.
* 🏆 **Tournament Lifecycle** — Transition games from *Open* → *Running* → *Finished*.
* 👥 **Real-time Rosters** — Dynamic player registration.
* 👑 **Hall of Fame** — Track historical performance by checking previous winners.
* 🔍 **Advanced Filtering & Pagination** — Search by name/format, toggle views based on user involvement, and efficiently navigate through large lists of tournaments with built-in pagination.
* 📢 **Admin Announcements & Locations** — Manage upcoming events and game locations effortlessly.

---

## 🛠️ Tech Stack

| Layer | Technology |
| :--- | :--- |
| **Framework** | ASP.NET Core MVC (.NET 8) |
| **Authentication** | ASP.NET Core Identity (UI via Razor Pages) |
| **Database** | SQL Server + Entity Framework Core |
| **Frontend** | Razor Views, Bootstrap 5, CSS3 Custom Properties, jQuery |
| **Testing** | NUnit, Moq, WebApplicationFactory |

---

## 🏗️ Architecture & Layers
The application strictly follows an **N-Tier Architecture** with separated concerns:

* **Web Layer (`PokerTracker`):** Contains Controllers, Views, and static assets. Responsible for handling HTTP requests, user sessions, and returning UI views.
* **Services Layer (`PokerTracker.Services.Core`):** Houses the core business logic, acting as the bridge between the Web layer and Data access.
* **Data Layer (`PokerTracker.Data`):** Contains the EF Core `ApplicationDbContext`, Migrations, Repository implementations, and Seed Configurations.
* **Domain Models (`PokerTracker.Data.Models`):** Defines the core entities matching database tables.
* **ViewModels (`PokerTracker.ViewModels`):** Data Transfer Objects (DTOs) tailored precisely to what the Views need to display or post.
* **Cross-Cutting Concerns (`PokerTracker.GCommon`):** Houses shared constants and validations accessible by all other layers.

### 🧪 Test Suite
* **`PokerTracker.Services.Tests`:** NUnit test project containing 100% coverage for all business logic and edge cases.
* **`PokerTracker.Web.Tests`:** Contains integration tests and controller routing validation using `WebApplicationFactory`.

## 🧠 Design Decisions
* **Custom DI Extension:** A cleaner `Program.cs` is achieved through a custom Dependency Injection extension method with assembly scanning, which registers services automatically.
* **Repository Pattern:** Centralizes data access logic, enabling easier testing and business logic decoupling.
* **Global Exception Handling:** Ensures unexpected errors are gracefully caught and dealt with, providing friendly error views to users and maintaining application stability.

## ✅ Validations
* **Server-side Validation:** Comprehensive use of Data Annotations on ViewModels and Entities to enforce constraints.
* **Client-side Validation:** Handled via jQuery and ASP.NET Core validation scripts for immediate feedback without full page reloads.

## 🗃️ Database & Seeding
* **Custom Entities:** Features specialized entities including `Tournament`, `Player`, `Location`, `Announcement`, and more.
* **Seed Data:** * Automatically populates the **Admin** role and assigns it to specific users upon first run.
  * Injects standard tournament formats, initial demo users, locations, and announcements.
* **Soft Delete:** Implemented efficiently to retain records behind the scenes while keeping UI views clean.

---

## 🚀 Setup Instructions

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or full)
* [Visual Studio 2022+](https://visualstudio.microsoft.com/) (recommended) or any IDE with .NET support

### Setup
1. **Clone the Repository**
   ```bash
   git clone https://github.com/Perchinski/PokerTracker_Feb2026.git
   cd PokerTracker_Feb2026
   ```

2. **Configure Database Connection**
   You can configure your SQL Server connection string using **User Secrets** (Recommended) or by modifying the `appsettings.Development.json` file.

   **Option A: Using User Secrets (Visual Studio)**
   * Right-click the `PokerTracker` project in the Solution Explorer and select **Manage User Secrets**.
   * Add the following JSON to the `secrets.json` file that opens and update the connection string:
     
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PokerTracker;Trusted_Connection=True;Encrypt=False"
       }
     }
     ```

   **Option B: Using appsettings.Development.json**
   * Open `PokerTracker/appsettings.Development.json` and update the connection string:
     
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PokerTracker;Trusted_Connection=True;Encrypt=False"
     }
     ```
   
   *Note: Configure the Startup Projects and select `PokerTracker` as the startup project.*
3. **Initialize Database (Package Manager Console)**
   Open the console by navigating to Tools > NuGet Package Manager > Package Manager Console.

   Select `PokerTracker.Data` in the "Default project" dropdown at the top of the console.

   Run the following command:
   ```powershell
   Update-Database
   ```

5. **Run Application**
   ```bash
   dotnet run --project PokerTracker
   ```
   Or open `PokerTracker.sln` in Visual Studio and press **F5**.

---

## 🧪 Demo Accounts

The database is seeded with three ready-to-use accounts (including an admin) and sample tournaments so you can explore immediately:

| Account | Email | Password | Role |
|---------|-------|----------|------|
| **Admin** | `admin1@pokertracker.com` | `Admin1!` | Administrator |
| **Player 1** | `player1@pokertracker.com` | `Player1!` | User |
| **Player 2** | `player2@pokertracker.com` | `Player2!` | User |

---

## 🎮 How It Works

### Tournament Lifecycle
| Status | Permissions & Actions |
| :--- | :--- |
| 🟢 **Open** | Players can join/leave. Owners/Admins can Edit, Delete, or Start the game. |
| 🟡 **Running** | Game is active. Roster is locked. Owner can click "Finish." |
| 🔴 **Finished** | Game concluded. Owner selects the Winner from the roster. |

### User Roles
* **Administrator:** System-wide authority. Has the ability to create, edit, and delete global Locations and Announcements, as well as moderate all tournaments.
* **Owner (Host):** The creator of a specific tournament. Has full administrative rights over that tournament's lifecycle and roster.
* **Player:** Registered users. Can browse global events, read announcements, and join open games.

---

## 📸 Screenshots

### Home Page
<img width="2560" height="1440" alt="Screenshot (10)" src="https://github.com/user-attachments/assets/296bab36-b0b1-4949-9b1e-380a24313b94" />

### Tournament List
<img width="2560" height="1440" alt="Screenshot (11)" src="https://github.com/user-attachments/assets/46340009-1278-4419-b995-7f1ea17abf3f" />

### Tournament Details
<img width="2560" height="1440" alt="Screenshot (12)" src="https://github.com/user-attachments/assets/e4a76998-03d7-488b-9e0b-0e69ff05d395" />

### Edit Tournament
<img width="2560" height="1440" alt="Screenshot (13)" src="https://github.com/user-attachments/assets/df460053-9bda-4107-8526-cf30cc373195" />

### Select Winner
<img width="2560" height="1440" alt="PokerTracker_Tournament_SelectWinner" src="https://github.com/user-attachments/assets/abdc21b6-76f4-44e0-8ff2-e0cb253189af" />

### Admin Panel
<img width="2560" height="1440" alt="Screenshot (14)" src="https://github.com/user-attachments/assets/b44ce5c3-9bcd-49b6-a80f-7820bf4bea16" />

### Location Management
<img width="2560" height="1440" alt="Screenshot (15)" src="https://github.com/user-attachments/assets/134fb9a0-cd60-46a1-94d7-954508a35977" />

### Add Location
<img width="2560" height="1440" alt="Screenshot (18)" src="https://github.com/user-attachments/assets/6ea76f9e-3553-4674-9954-22c37468f0a7" />

### User Management
<img width="2560" height="1440" alt="Screenshot (16)" src="https://github.com/user-attachments/assets/1d1865bf-b672-4753-90d9-27d6c0935fa6" />

### Announcement Management
<img width="2560" height="1440" alt="Screenshot (17)" src="https://github.com/user-attachments/assets/51cfe3aa-ddb8-486e-93d1-a2c2e6af817f" />

---

## 📄 License
This project is open-source and intended for educational and personal use.
