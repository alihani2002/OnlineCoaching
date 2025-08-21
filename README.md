# 🏋️‍♂️ Personal Coaching Platform

A comprehensive **personal coaching platform** built with **ASP.NET Core MVC** that enables coaches to provide **online coaching packages, digital books, and courses** to clients in a secure, interactive, and role-based environment.  

---

## 🚀 Overview

This platform allows **coaches** to deliver personalized coaching experiences while giving **clients** flexibility to subscribe to coaching packages, purchase books, enroll in courses, or combine these services.  

Key features include:  
- ✅ Online Coaching with subscription packages  
- ✅ Secure digital book purchases (view-only, anti-copy & anti-download)  
- ✅ Interactive courses with free demos, progress tracking, and completion logging  
- ✅ Messaging & feedback system between clients and coaches  
- ✅ In-app notifications (approvals, plan updates, expiry reminders)  
- ✅ Transformation tracking with **before/after images**  
- ✅ Unified client dashboard for all approved services  
- ✅ Reusable resources (exercises, meal plans, course materials) for coaches  

---

## ✨ Features

### 👨‍💻 For Coaches
- Create **multiple subscription packages** (1 month → 1 year) with pricing and details  
- Approve client requests for coaching, books, and courses  
- Assign **personalized nutrition and workout plans** from a predefined database  
- Manage **course content, book library, exercises, foods, and questions**  
- Track client progress in courses, workouts, and meal plans  
- Review **client feedback** and transformation submissions  

### 🧑‍🎓 For Clients
- Register and choose any service (coaching, books, courses, or all)  
- Receive **customized plans** based on questionnaire responses  
- Access purchased **books (view-only, secure)** and **courses (lifetime access)**  
- Preview **demo content** before purchasing  
- View all active subscriptions, books, and courses in a **personal dashboard**  
- Submit **feedback** to the coach and upload **before/after transformation images**  
- Get **real-time notifications** about approvals, updates, and subscription expiry  


## 🛠️ Tech Stack
- **ASP.NET Core MVC** (latest version)  
- **Entity Framework Core** (Code-First, Migrations)  
- **Identity** for authentication and role-based access  
- **Bootstrap 5** + **Font Awesome** for frontend styling  
- **SQL Server** (default database, can be changed)  

---

## 🚀 Getting Started

### Prerequisites
- [.NET 7+ SDK](https://dotnet.microsoft.com/download)  
- SQL Server / LocalDB  
- Visual Studio 2022 or VS Code  

### Installation
1. Clone the repo:
   ```bash
   git clone https://github.com/alihani2002/OnlineCoaching.git
   cd PersonalCoachingPlatform
cd PersonalCoachingPlatform
Apply migrations:
dotnet ef database update
Run the application:
dotnet run --project Coaching.WebUI
