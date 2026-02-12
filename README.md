# CarPainting Studio

ASP.NET Core MVC приложение за управление на автосервиз за боядисване.

## Технологии

- ASP.NET Core 8.0 MVC
- Entity Framework Core 8.0
- SQLite база данни
- Bootstrap 5
- jQuery

## Функционалности

- Управление на услуги (CRUD)
- Записване на часове за клиенти
- Галерия с завършени проекти
- Информация за екипа

## Стартиране

### Изисквания
- .NET 8.0 SDK
- Visual Studio 2022 или VS Code

### Инсталация

1. Клонирайте проекта
```bash
git clone [repo-url]
cd CarPaintingStudio
```

2. Възстановете пакетите
```bash
dotnet restore
```

3. Създайте базата данни
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Стартирайте приложението
```bash
dotnet run
```

Отворете https://localhost:5001 в браузъра.

## Структура

- `Controllers/` - MVC контролери
- `Models/` - Модели на данните
- `Views/` - Razor изгледи
- `Data/` - Database context
- `ViewModels/` - View models
- `wwwroot/` - Статични файлове

## База данни

Проектът използва SQLite с Entity Framework Core и Code First подход.

Моделите включват:
- Service - Услуги на сервиза
- Appointment - Записани часове
- GalleryItem - Елементи в галерията
- Employee - Служители

## Лиценз

Проектът е разработен за образователни цели.
