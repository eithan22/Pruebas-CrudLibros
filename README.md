# Pruebas-CrudLibros 📚🧪

A **Full-Stack Book Management System** built with **ASP.NET Core 8**, combining a RESTful Web API and an ASP.NET Core MVC web application, both sharing the same Clean Architecture backend. Features session-based authentication, full CRUD via MVC views, and automated testing with **Selenium WebDriver**.

---

## 📋 Description

This project extends the Crud-Libros REST API by adding a fully functional web front-end using ASP.NET Core MVC with Razor Views. Users must log in to access the book management interface. The system demonstrates how to connect an MVC presentation layer to an existing service and repository layer, and includes automated UI tests using Selenium.

---

## ✨ Features

- 🔐 **Session Authentication** — Login required to access all book management pages
- 📖 **Full CRUD (MVC Views)** — Create, Read, Update, and logical Delete via Razor Views
- 🌐 **REST API** — Parallel RESTful API with identical functionality
- 🏗️ **Clean Architecture** — Domain, Application, Persistence, API, and Presentation layers
- 🧪 **Selenium Tests** — Automated UI testing with Selenium WebDriver
- ✅ **Input Validation** — Title length limits and ModelState validation
- ♻️ **Soft Delete** — Logical deletion via Eliminado flag

---

## 🛠️ Technologies

| Category | Technology |
|----------|-----------|
| Language | C# |
| Framework | ASP.NET Core 8 (Web API + MVC) |
| Architecture | Clean Architecture |
| Auth | Session-based authentication |
| Frontend | Razor Views (ASP.NET Core MVC) |
| Testing | Selenium WebDriver |
| Database | SQL Server (EF Core) |

---

## 🏗️ Architecture

```
Pruebas-CrudLibros/
├── Api.Libros/               REST API Layer
│   └── Controllers/
│       └── LibrosController.cs    Full CRUD REST endpoints
│
├── Presentacion.Libro/       MVC Presentation Layer
│   ├── Controllers/
│   │   ├── AccesoController.cs    Login / Logout (session auth)
│   │   └── LibrosController.cs    CRUD via Razor Views
│   └── Views/                     Razor view templates
│
├── Aplication/               Application Layer
│   ├── Interfaces/ILibroService.cs
│   └── Services/LibroService.cs   Business logic
│
├── Domian/                   Domain Layer
│   └── Entidades/Libro.cs         Id, Titulo, Autor, Isbn, Eliminado
│
├── Persistencia/             Infrastructure Layer
│   ├── Data/AppBDContext.cs        EF Core DbContext
│   └── Repositories/              Repository implementations
│
└── TestsLibros/              Selenium Test Layer
    └── [UI automated tests]
```

---

## 🔐 Authentication Flow

```
User visits /Libros → No session → Redirect to /Acceso/Login
User logs in → Session["Usuario"] set → Access granted
All Libros actions check: HttpContext.Session.GetString("Usuario")
```

---

## 📡 REST API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/Libros | Get all books |
| GET | /api/Libros/{id} | Get book by ID |
| POST | /api/Libros | Create a book |
| PUT | /api/Libros/{id} | Update a book |
| DELETE | /api/Libros/{id} | Soft-delete a book |
| GET | /api/Libros/search?term= | Search books |

## 🖥️ MVC Routes

| Route | Action |
|-------|--------|
| /Acceso/Login | Login page |
| /Libros | Index — list all books |
| /Libros/Create | Create new book form |
| /Libros/Edit/{id} | Edit existing book |
| /Libros/Delete/{id} | Delete confirmation |

---

## 🚀 Installation

Prerequisites: .NET 8 SDK · SQL Server · Visual Studio 2022 · ChromeDriver (for Selenium)

```bash
git clone https://github.com/Eithan22/Pruebas-CrudLibros.git
cd Pruebas-CrudLibros
dotnet restore
dotnet ef database update --project Persistencia --startup-project Api.Libros
# Run API: dotnet run --project Api.Libros
# Run MVC: dotnet run --project Presentacion.Libro
```

---

## 💡 Skills Demonstrated

- ✅ **Full-Stack Development** — REST API + MVC frontend sharing the same backend
- ✅ **Session Authentication** — Custom login/logout with session management
- ✅ **Clean Architecture** — 5-layer system with clear separation of concerns
- ✅ **Razor Views** — Server-side rendered MVC views with validation
- ✅ **Selenium WebDriver** — Automated UI testing
- ✅ **Soft Delete** — Logical deletion pattern
- ✅ **Input Validation** — Server-side ModelState with custom error messages

---

## 🔮 Future Improvements

- [ ] Replace session auth with JWT / ASP.NET Core Identity
- [ ] Add user roles and permissions
- [ ] Expand Selenium test suite coverage
- [ ] Add pagination to book list
- [ ] Deploy to Azure App Service

---

## 👨‍💻 Author

**Eithan** — Backend Developer · Santo Domingo, Dominican Republic 🇩🇴
🎓 Software Development @ ITLA · 📧 eithanread1@gmail.com
[LinkedIn](https://linkedin.com/in/eithan-r) · [GitHub](https://github.com/Eithan22)

---

*MIT License*
