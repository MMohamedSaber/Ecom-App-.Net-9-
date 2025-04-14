# 🛒 E-Commerce Web API

## 📖 Description  
This is a simple E-Commerce Web API project built with ASP.NET Core.  
The system handles user registration, login, and email confirmation.  
It uses JWT (JSON Web Token) for secure authentication and Redis for caching.

## 📌 Features  
- User registration and login system  
- Email confirmation via SMTP  
- Secure JWT token generation and validation  
- Redis caching integration for better performance  
- Product and order management (CRUD operations)  
- Password hashing and identity management using ASP.NET Identity  
- API tested using Postman

## 🛠️ Tools & Technologies  
- ASP.NET Core Web API  
- Entity Framework Core  
- SQL Server  
- Redis  
- ASP.NET Identity  
- JWT (JSON Web Token)  
- SMTP Email Service  
- Postman  

## 📦 How to Run  

1️⃣ Install the required packages using NuGet  
2️⃣ Update your `appsettings.json` with your database, JWT, SMTP, and Redis configurations  
3️⃣ Run database migrations:
```bash
dotnet ef database update
