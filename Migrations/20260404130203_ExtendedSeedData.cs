using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarPaintingStudio.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "CarBrand", "CarModel", "CarYear", "CreatedDate", "CustomerName", "Email", "Notes", "Phone", "ServiceId", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota", "Camry", 2020, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Георги Петров", "georgi@example.com", "Предпочита тъмно синьо", "0877111222", 1, 3, null },
                    { 2, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Honda", "Civic", 2019, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Мария Иванова", "maria@example.com", null, "0877222333", 3, 3, null },
                    { 3, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ford", "Focus", 2021, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Николай Стоянов", "nikolay@example.com", null, "0877333444", 2, 1, null },
                    { 5, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "BMW", "X5", 2023, new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Стефан Колев", "stefan@example.com", "Матово черно - целия автомобил", "0877555666", 4, 0, null }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Bio", "Email", "FullName", "HireDate", "IsActive", "Phone", "PhotoUrl", "Position", "YearsOfExperience" },
                values: new object[,]
                {
                    { 3, "Специалист по vinyl wrap и керамични покрития", "stoyan.nikolov@carpaint.bg", "Стоян Николов", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "0888345678", null, "Vinyl специалист", 5 },
                    { 4, "Майстор на полирането и детайлинга", "dimitar.vasilev@carpaint.bg", "Димитър Василев", new DateTime(2019, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "0888456789", null, "Полировчик", 6 }
                });

            migrationBuilder.InsertData(
                table: "GalleryItems",
                columns: new[] { "Id", "AfterImageUrl", "BeforeImageUrl", "CarBrand", "CarModel", "CompletedDate", "CreatedDate", "Description", "IsVisible", "Title" },
                values: new object[,]
                {
                    { 3, "/images/gallery/audi-after.jpg", "/images/gallery/audi-before.jpg", "Audi", "A6", new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Пълно боядисване в синьо металик с керамична защита", true, "Audi A6 - Синьо металик" },
                    { 4, "/images/gallery/porsche-after.jpg", "/images/gallery/porsche-before.jpg", "Porsche", "911", new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Цялостно фолиране с червен vinyl и гланц финиш", true, "Porsche 911 - Vinyl wrap червено" },
                    { 5, "/images/gallery/vw-after.jpg", "/images/gallery/vw-before.jpg", "Volkswagen", "Golf", new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Profesionalno полиране и нанасяне на керамично покритие", true, "VW Golf - Полиране и защита" },
                    { 6, "/images/gallery/toyota-after.jpg", "/images/gallery/toyota-before.jpg", "Toyota", "Supra", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Матово сиво покритие с vinyl акценти", true, "Toyota Supra - Матово сиво" }
                });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Content", "CreatedDate" },
                values: new object[] { "Страхотна работа! Колата изглежда като нова. Боядисаха я перфектно и в срок. Препоръчвам на всички!", new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Content", "CreatedDate" },
                values: new object[] { "Много съм доволна от резултата. Полирането е страхотно - колата свети! Екипът е много любезен и професионален.", new DateTime(2025, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "Content",
                value: "Добра работа, доволен съм. Малко по-дълго от очакваното но резултатът си заслужава.");

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AuthorEmail", "AuthorName", "Content", "CreatedDate", "IsApproved", "Rating", "ServiceId", "UserId" },
                values: new object[,]
                {
                    { 4, "alex@example.com", "Александра Тодорова", "Невероятен резултат с vinyl wrap-а! Колата изглежда като от шоурум. Ще се върна определено.", new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, null, null },
                    { 5, "test@example.com", "Тест Потребител", "Все още чака одобрение - тестов отзив.", new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, null, null }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CreatedDate", "Description", "DurationDays", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { 5, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Нанасяне на керамично покритие за дълготрайна защита на боята", 3, true, "Керамично покритие", 1200m },
                    { 6, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Цялостно или частично фолиране с висококачествен vinyl", 5, true, "Vinyl wrap", 2800m }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "CarBrand", "CarModel", "CarYear", "CreatedDate", "CustomerName", "Email", "Notes", "Phone", "ServiceId", "Status", "UserId" },
                values: new object[] { 4, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kia", "Sportage", 2022, new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Елена Димитрова", "elena@example.com", "Иска керамично покритие на цялото тяло", "0877444555", 5, 0, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Content", "CreatedDate" },
                values: new object[] { "Страхотна работа! Колата изглежда като нова. Препоръчвам на всички!", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Content", "CreatedDate" },
                values: new object[] { "Много съм доволна от резултата. Професионален екип и отлично качество.", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "Content",
                value: "Добра работа, доволен съм. Малко по-дълго от очакваното, но резултатът си заслужава.");
        }
    }
}
