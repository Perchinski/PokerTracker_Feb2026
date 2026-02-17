# ♠️ PokerTracker

A sleek, high-performance web application for organizing, managing, and tracking private poker tournaments. Built with **ASP.NET Core MVC** and **.NET 8**.

---

## 📖 About
**PokerTracker** simplifies the logistics of home games and competitive series. Create and join tournaments, manage the live game lifecycle, and crown your champions—all from a single, responsive dashboard.

## ✨ Key Features
* 🔐 **Identity Management** — Secure registration and login powered by ASP.NET Core Identity.
* 🏆 **Tournament Lifecycle** — Transition games through *Open* → *Running* → *Finished* stages.
* 👥 **Real-time Rosters** — Dynamic player registration; join or leave with a single click.
* 👑 **Hall of Fame** — Assign winners to finished tournaments to track historical performance.
* 🔍 **Advanced Filtering** — Search by name or format, and toggle "Joined" or "Owned" views.
* 🎨 **Poker Aesthetic** — A modern dark-themed UI built with Bootstrap 5 and Bootstrap Icons.

---

## 🛠️ Tech Stack

| Layer | Technology |
| :--- | :--- |
| **Framework** | ASP.NET Core MVC (.NET 8) |
| **Authentication** | ASP.NET Core Identity (UI via Razor Pages) |
| **Database** | SQL Server + Entity Framework Core |
| **Frontend** | Razor Views, Bootstrap 5, CSS3 Custom Properties |
| **Architecture** | Service Layer Pattern with Dependency Injection |

---

## 📁 Project Structure
```text
PokerTracker/                → Web Layer (Controllers, Views, Static Assets)
PokerTracker.Data.Models/    → Domain Entities (Tournament, Player, etc.)
PokerTracker.Data/           → Data Context, Seed Data, Migrations, & Configurations
PokerTracker.ViewModels/     → Data Transfer Objects (DTOs) for Views
PokerTracker.Services.Core/  → Business Logic & Service Interfaces
PokerTracker.Common/         → Constants, Enums, and Shared Validation
```

---

## 🚀 Getting Started

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

2. **Configure Connection String and select PokerTracker as your startup project**

   Update `PokerTracker/appsettings.Development.json` with your local SQL Server instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PokerTracker;Trusted_Connection=True;Encrypt=False"
   }
   ```
   Configure the Startup Projects and select PokerTracker as the startup project

4. **Initialize Database (Package Manager Console)**

   Open the console by navigating to Tools > NuGet Package Manager > Package Manager Console.

   Run the following command:
   ```powershell
   Update-Database -Project PokerTracker.Data -StartupProject PokerTracker
   ```
   This creates the database, seeds tournament formats, demo users, and sample tournaments.

   Note: If "PokerTracker" is already set as your startup project in the Solution Explorer, and "PokerTracker.Data" is selected in the "Default project" dropdown at the top of the console, you can    simply run:
    ```powershell
   Update-Database
   ```

6. **Run Application**
   ```bash
   dotnet run --project PokerTracker
   ```
   Or open `PokerTracker.sln` in Visual Studio and press **F5**.

7. **Register an account or log in with a demo account** and start creating tournaments!
   
---

## 🧪 Demo Accounts

The database is seeded with two ready-to-use accounts and sample tournaments so you can explore immediately:

| Account | Email | Password |
|---------|-------|----------|
| **Player 1** | `player1@pokertracker.com` | `Player1!` |
| **Player 2** | `player2@pokertracker.com` | `Player2!` |

### Seeded Tournaments

| Tournament | Format | Status | Created By | Winner |
|-----------|--------|--------|------------|--------|
| Friday Night Holdem | Texas Hold'em - No Limit | 🟢 Open | Player 1 | — |
| Weekend Bounty Bash | Bounty / Knockout | ⚫ Finished | Player 2 | Player 1 |

Both players are registered in both tournaments. Log in as either account to see the "Joined" and "Your Tournament" badges in action.

---

## 🎮 How It Works

### Tournament Lifecycle
| Status | Permissions & Actions |
| :--- | :--- |
| 🟢 **Open** | Players can join/leave. Owners can Edit, Delete, or Start the game. |
| 🟡 **Running** | Game is active. Roster is locked. Owner can click "Finish." |
| 🔴 **Finished** | Game concluded. Owner selects the Winner from the roster. |

### User Roles
* **Owner (Host):** The creator. Has full administrative rights over the tournament lifecycle.
* **Player:** Registered users. Can browse and join open games.

---

## 📸 Screenshots

### Home Page
<img width="2560" height="1440" alt="PokerTracker_Home_Index" src="https://github.com/user-attachments/assets/a1e3f38a-481a-4b52-aa60-b539ec95ff74" />

### Tournament List
<img width="2560" height="1440" alt="PokerTracker_Tournaments_Index" src="https://github.com/user-attachments/assets/bc509434-9c45-4b59-98d2-ee17391250f8" />

### Tournament Details
<img width="2560" height="1440" alt="PokerTracker_Tournament_Details" src="https://github.com/user-attachments/assets/2fd5cc86-003e-48db-a269-6f0430cd5f07" />

### Create Tournament
<img width="2560" height="1440" alt="PokerTracker_Tournaments_Create" src="https://github.com/user-attachments/assets/3e9bb19f-1bd0-4a41-b97d-0f059eaafdd6" />

### Select Winner
<img width="2560" height="1440" alt="PokerTracker_Tournament_SelectWinner" src="https://github.com/user-attachments/assets/abdc21b6-76f4-44e0-8ff2-e0cb253189af" />

---


## ⚙️ Configuration
Manage security and lockout policies directly via `appsettings.json` without touching the source code:

```json
"IdentityOptions": {
  "Password": {
    "RequireDigit": true,
    "RequiredLength": 6,
    "RequireUppercase": false
  },
  "Lockout": {
    "MaxFailedAccessAttempts": 5,
    "DefaultLockoutTimeSpanMin": 5
  }
}
```

---

## 🗃️ Database Features
* **Soft Delete:** Uses a Global Query Filter on `IsDeleted` so data is never truly lost, just hidden from the UI.
* **Seeded Data:** Automatically populates 6 standard formats (Texas Hold'em, Omaha, etc.) on the first migration.
* **Referential Integrity:** Configured with `DeleteBehavior.Restrict` on critical links to prevent accidental data loss of tournament history.

---

## 📄 License
This project is open-source and intended for educational and personal use.
