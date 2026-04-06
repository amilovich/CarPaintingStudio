# 🚗 CarPainting Studio

<div align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8.0">
  <img src="https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="ASP.NET Core MVC">
  <img src="https://img.shields.io/badge/Entity_Framework-8.0-512BD4?style=for-the-badge&logo=nuget&logoColor=white" alt="EF Core">
  <img src="https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white" alt="Bootstrap">
  <img src="https://img.shields.io/badge/SQLite-3.0-003B57?style=for-the-badge&logo=sqlite&logoColor=white" alt="SQLite">
  <img src="https://img.shields.io/badge/xUnit-Tests-green?style=for-the-badge" alt="xUnit Tests">
</div>

<br />

## 📝 Описание на проекта

**CarPainting Studio** е уеб приложение за управление на автосервиз за боядисване на автомобили, изградено с **ASP.NET Core 8.0 MVC**. Позволява на клиентите да разглеждат услуги, да записват часове и да оставят отзиви, а на администраторите — да управляват пълния работен процес на сервиза.

---

## 🛠️ Технологичен стек

| Слой | Технология |
|------|-----------|
| Backend Framework | ASP.NET Core 8.0 MVC |
| ORM | Entity Framework Core 8.0 |
| База данни | SQLite (Code First) |
| Автентикация | ASP.NET Identity |
| Frontend | Bootstrap 5.3, Font Awesome 6.4, jQuery |
| Unit Tests | xUnit 2.6 + EF InMemory |

---

## 🏗️ Архитектура

Проектът следва **MVC** шаблон с допълнителен **Services Layer**:

```
CarPaintingStudio/
├── Areas/Admin/                  # Admin Area
│   ├── Controllers/              # Dashboard, Services, Appointments, Reviews
│   └── Views/
├── Controllers/                  # Public контролери
├── Services/                     # Бизнес логика (IServiceService, IAppointmentService, IReviewService)
├── Models/                       # 6 entity модела
├── ViewModels/                   # View Models + PaginatedList<T>
├── Data/                         # ApplicationDbContext + DbSeeder
├── Views/                        # Razor изгледи
├── CarPaintingStudio.Tests/      # xUnit тест проект
└── wwwroot/                      # CSS, JS, статични файлове
```

---

## 🎯 Entity модели (6 модела)

| Модел | Описание |
|-------|---------|
| `ApplicationUser` | Разширен Identity потребител с FullName |
| `Service` | Услуга на сервиза |
| `Appointment` | Записан час от клиент |
| `GalleryItem` | Снимка преди/след |
| `Employee` | Служител на сервиза |
| `Review` | Отзив с рейтинг 1-5 |

---

## 👥 Роли и достъп

| Роля | Достъп |
|------|--------|
| **Гост** | Начало, Услуги, Галерия, Запиши час, Отзиви |
| **User** | + Моите записвания, Напиши отзив |
| **Admin** | + Admin панел (пълно управление) |

**Admin акаунт:** `admin@carpaint.bg` / `Admin123!`

---

## 🔑 Основни функционалности

- **Услуги** — pagination, търсене, филтър по цена, сортиране
- **Записвания** — CRUD, статуси, филтриране, pagination
- **Отзиви** — интерактивни звезди, одобрение от Admin, pagination
- **Галерия** — преди/след modal, grid switcher
- **Admin Dashboard** — статистика с AJAX обновяване
- **Identity** — Register, Login, роли, AutoValidateAntiForgeryToken

---

## 🧪 Unit тестове

**43 теста** в `CarPaintingStudio.Tests/` с xUnit + EF InMemory:

```bash
dotnet test CarPaintingStudio.Tests/CarPaintingStudio.Tests.csproj
```

---

## 🚀 Инсталация

```bash
git clone https://github.com/amilovich/CarPaintingStudio.git
cd CarPaintingStudio
dotnet restore
dotnet ef database update
dotnet run
```

Отвори **https://localhost:5001** в браузъра.

---

## 📊 Git история — 30 комита за 3 дни

### 🔵 Ден 1 (Комити 1-10)
| # | Описание |
|---|---------|
| 1 | Identity пакети + ApplicationDbContext → IdentityDbContext + ApplicationUser |
| 2 | AccountController (Register/Login/Logout) + ViewModels + _Layout auth навигация |
| 3 | DbSeeder + Admin Area структура + DashboardController + _AdminLayout |
| 4 | Admin/ServicesController CRUD + ToggleActive |
| 5 | Admin/AppointmentsController CRUD + ChangeStatus бутони |
| 6 | Review модел (5-ти entity) + миграция + seed на отзиви |
| 7 | ReviewsController + Views + интерактивни звезди JS |
| 8 | Custom 404/500 pages + UseStatusCodePagesWithReExecute |
| 9 | [Authorize] атрибути + AutoValidateAntiForgeryToken глобално |
| 10 | Разширен seed + Admin/ReviewsController (Approve/Reject/Delete) |

### 🟢 Ден 2 (Комити 11-20)
| # | Описание |
|---|---------|
| 11 | PaginatedList<T> + ServiceFilterViewModel + pagination за Services/Index |
| 12 | AppointmentFilterViewModel + pagination/search/filter за Appointments |
| 13 | Services Layer — IServiceService, IAppointmentService, IReviewService |
| 14 | Рефакториране на всички контролери да използват Services Layer |
| 15 | Unit Tests проект — xUnit + EF InMemory, 43 теста |
| 16 | Пълна README документация |
| 17 | Admin Dashboard с AJAX GetStats + Services/Details с отзиви |
| 18 | ReviewFilterViewModel + pagination/search за Reviews/Index |
| 19 | Подобрен Appointments/Details с карти и покана за отзив |
| 20 | Подобрена Gallery страница с hover zoom и modal |

### 🟡 Ден 3 (Комити 21-30)
| # | Описание |
|---|---------|
| 21 | Подобрена About страница с модерен дизайн и технологии |
| 22 | Подобрена Gallery страница |
| 23 | Активни nav линкове в _Layout + тъмен navbar |
| 24 | Подобрени Login/Register форми — show/hide парола, strength indicator |
| 25 | Подобрен Admin Layout с активни nav линкове и секции |
| 26 | Подобрени error pages — 403, 404, 500 |
| 27 | Services Create/Edit + Reviews Details модернизирани |
| 28 | Appointments Edit/Delete + Services Delete модернизирани |
| 29 | Почистени site.css и site.js с scroll анимации |
| 30 | Финален cleanup — appsettings, README |

---

## 📝 Лиценз

Проектът е разработен за образователни цели — ASP.NET Advanced курс @ SoftUni.

---

<div align="center">
  <p>Направено с ❤️ и .NET 8.0</p>
  <p>© 2025 CarPainting Studio | ASP.NET Advanced @ SoftUni</p>
</div>
