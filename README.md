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
| IDE | Visual Studio 2022 |

---

## 🏗️ Архитектура

Проектът следва класическия **MVC (Model-View-Controller)** шаблон с допълнителен **Services Layer** за бизнес логиката:

```
CarPaintingStudio/
├── Areas/
│   └── Admin/                    # Admin Area (MVC Area)
│       ├── Controllers/
│       │   ├── DashboardController.cs
│       │   ├── ServicesController.cs
│       │   ├── AppointmentsController.cs
│       │   └── ReviewsController.cs
│       └── Views/
├── Controllers/                  # Public контролери
│   ├── HomeController.cs
│   ├── AccountController.cs
│   ├── ServicesController.cs
│   ├── AppointmentsController.cs
│   ├── ReviewsController.cs
│   └── GalleryController.cs
├── Services/                     # Services Layer (бизнес логика)
│   ├── IServiceService.cs / ServiceService.cs
│   ├── IAppointmentService.cs / AppointmentService.cs
│   └── IReviewService.cs / ReviewService.cs
├── Models/                       # Entity модели
│   ├── ApplicationUser.cs        # ASP.NET Identity потребител
│   ├── Service.cs
│   ├── Appointment.cs
│   ├── GalleryItem.cs
│   ├── Employee.cs
│   └── Review.cs
├── ViewModels/                   # View Models
│   ├── ServiceViewModel.cs
│   ├── ServiceFilterViewModel.cs
│   ├── CreateAppointmentViewModel.cs
│   ├── AppointmentFilterViewModel.cs
│   ├── CreateReviewViewModel.cs
│   ├── PaginatedList.cs
│   ├── AppointmentStatsViewModel.cs
│   └── ReviewStatsViewModel.cs
├── Data/
│   ├── ApplicationDbContext.cs   # IdentityDbContext
│   └── DbSeeder.cs               # Seed на роли и Admin акаунт
├── Views/                        # Razor изгледи
├── CarPaintingStudio.Tests/      # Unit тест проект
│   ├── TestDbContextFactory.cs
│   └── Services/
│       ├── ServiceServiceTests.cs
│       ├── AppointmentServiceTests.cs
│       └── ReviewServiceTests.cs
├── Program.cs
└── appsettings.json
```

### Слоеве

- **Controllers** — routing, валидация на входящите данни, избор на view
- **Services** — бизнес логика, изолирана от контролерите
- **Models** — entity модели, директно свързани с базата данни
- **ViewModels** — данни, специфично оформени за конкретен изглед

---

## 🎯 Entity модели (5 модела)

| Модел | Описание |
|-------|---------|
| `ApplicationUser` | Разширен Identity потребител с FullName и RegisteredOn |
| `Service` | Услуга на сервиза (боядисване, полиране и др.) |
| `Appointment` | Записан час от клиент |
| `GalleryItem` | Снимка преди/след от галерията |
| `Employee` | Служител на сервиза |
| `Review` | Отзив от клиент с рейтинг 1-5 |

---

## 👥 Роли и достъп

| Роля | Достъп |
|------|--------|
| **Гост** | Начало, Услуги (преглед), Галерия, Запиши час, Отзиви (преглед) |
| **User** | Всичко за гост + Моите записвания, Напиши отзив |
| **Admin** | Всичко + Admin панел (управление на услуги, записвания, отзиви) |

### Admin акаунт (seed данни)
```
Email:    admin@carpaint.bg
Парола:   Admin123!
```

---

## 🔑 Основни функционалности

### Публична секция
- 🏠 **Начална страница** — услуги, галерия, testimonials, контакти
- 🛠️ **Услуги** — преглед с pagination (6 на страница), търсене, филтър по цена, сортиране
- 📅 **Запиши час** — форма за записване (достъпна и за гости)
- 🖼️ **Галерия** — проекти преди/след
- ⭐ **Отзиви** — одобрени отзиви с рейтинг и статистика
- 👥 **За нас** — екип и информация за сервиза

### Потребителска секция (след вход)
- 📋 **Моите записвания** — pagination, филтър по статус и период, търсене
- ✍️ **Напиши отзив** — формуляр с интерактивни звезди (1-5)

### Admin Area (`/Admin`)
- 📊 **Dashboard** — статистика: услуги, записвания, служители, галерия
- 🛠️ **Управление на услуги** — CRUD + активиране/деактивиране
- 📅 **Управление на записвания** — промяна на статус, търсене, филтриране
- ⭐ **Управление на отзиви** — одобрение/отхвърляне/изтриване

---

## 🔒 Сигурност

- **ASP.NET Identity** за автентикация и авторизация
- **[Authorize(Roles = "Admin")]** за Admin Area
- **AutoValidateAntiForgeryToken** — глобален CSRF филтър
- **[ValidateAntiForgeryToken]** на всички POST форми
- **XSS защита** чрез Razor автоматично HTML encoding
- **Custom 404/500 pages** с `UseStatusCodePagesWithReExecute`
- Server-side + client-side валидация на всички форми

---

## 📄 Валидация

Всички entity модели имат DataAnnotations атрибути:

```csharp
[Required(ErrorMessage = "Името е задължително")]
[StringLength(100, MinimumLength = 2)]
[EmailAddress(ErrorMessage = "Невалиден имейл")]
[Range(50, 10000, ErrorMessage = "Цената трябва да е между 50 и 10000 лв.")]
```

Client-side валидацията е активирана чрез `_ValidationScriptsPartial`.

---

## 🗄️ База данни и Seed данни

Проектът използва **SQLite** с **Code First** подход.

### Seed данни (автоматично при стартиране)

- **6 услуги** — Пълно боядисване, Частично, Полиране, Матово, Керамично, Vinyl wrap
- **4 служители** — с биографии и опит
- **6 галерийни записа** — Mercedes, BMW, Audi, Porsche, VW, Toyota
- **5 записвания** — с различни статуси
- **5 отзива** — 4 одобрени + 1 чакащ
- **Роли** — Admin и User
- **Admin акаунт** — `admin@carpaint.bg`

---

## 🧪 Unit тестове

Проектът включва **xUnit** тест проект с **43 теста** покриващи Services Layer-а.

### Структура на тестовете

```
CarPaintingStudio.Tests/
├── TestDbContextFactory.cs       # InMemory DB factory с изолация
└── Services/
    ├── ServiceServiceTests.cs    # 14 теста
    ├── AppointmentServiceTests.cs # 15 теста
    └── ReviewServiceTests.cs     # 14 теста
```

### Стартиране на тестовете

```bash
dotnet test CarPaintingStudio.Tests/CarPaintingStudio.Tests.csproj
```

### Покрити сценарии

**ServiceService (14 теста)**
- Търсене и филтриране по цена и сортиране
- GetById — съществуващ и несъществуващ Id
- Create, Update, Delete, ToggleActive, Exists

**AppointmentService (15 теста)**
- Admin вижда всички, User вижда само своите
- Филтриране по статус, период, търсене по клиент
- Create, ChangeStatus, Delete, GetStats

**ReviewService (14 теста)**
- Само одобрени отзиви са публично видими
- Approve, Reject, Delete
- Статистика: брой, средна оценка, pending

---

## 🚀 Инсталация и стартиране

### Изисквания
- .NET 8.0 SDK
- Visual Studio 2022

### Стъпки

```bash
# 1. Клониране на проекта
git clone https://github.com/amilovich/CarPaintingStudio.git
cd CarPaintingStudio

# 2. Възстановяване на пакетите
dotnet restore

# 3. Прилагане на миграциите
dotnet ef database update

# 4. Стартиране
dotnet run
```

Приложението ще е достъпно на **https://localhost:5001**

### Admin вход
```
Email:  admin@carpaint.bg
Парола: Admin123!
```

---

## 📊 Контролери (6 публични + 4 Admin)

| Контролер | Маршрут | Описание |
|-----------|---------|---------|
| HomeController | `/` | Начало, За нас, Error pages |
| AccountController | `/Account` | Register, Login, Logout |
| ServicesController | `/Services` | Публичен преглед с pagination |
| AppointmentsController | `/Appointments` | Записвания (логнати) |
| ReviewsController | `/Reviews` | Отзиви (публично + Create за логнати) |
| GalleryController | `/Gallery` | Галерия |
| Admin/DashboardController | `/Admin` | Статистика |
| Admin/ServicesController | `/Admin/Services` | CRUD услуги |
| Admin/AppointmentsController | `/Admin/Appointments` | Управление записвания |
| Admin/ReviewsController | `/Admin/Reviews` | Одобрение отзиви |

---

## 📈 Git история

Проектът е разработен с **30 комита** разпределени в **3 дни**, демонстриращи реален development процес:

- **Ден 1** — Identity, Areas, роли, AccountController, Admin Dashboard, CRUD за услуги и записвания, Review модел, error pages, Authorize, seed данни
- **Ден 2** — Pagination, Search/Filter, Services Layer, рефакториране на контролери, Unit Tests, README
- **Ден 3** — допълнителни функционалности

---

## 📝 Лиценз

Проектът е разработен за образователни цели — ASP.NET Advanced курс @ SoftUni.

---

<div align="center">
  <p>Направено с ❤️ и .NET 8.0</p>
  <p>© 2025 CarPainting Studio | ASP.NET Advanced @ SoftUni</p>
</div>
