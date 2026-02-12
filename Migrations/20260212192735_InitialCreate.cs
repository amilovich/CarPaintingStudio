using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarPaintingStudio.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: false),
                    PhotoUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    HireDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Bio = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GalleryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    BeforeImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    AfterImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    CarBrand = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CarModel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsVisible = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationDays = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    CarBrand = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CarModel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CarYear = table.Column<int>(type: "INTEGER", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Bio", "Email", "FullName", "HireDate", "IsActive", "Phone", "PhotoUrl", "Position", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, "Специалист с над 15 години опит в автомобилното боядисване", "ivan.georgiev@carpaint.bg", "Иван Георгиев", new DateTime(2010, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "0888123456", null, "Главен боядисвач", 15 },
                    { 2, "Експерт по матови покрития и специални ефекти", "petar.dimitrov@carpaint.bg", "Петър Димитров", new DateTime(2017, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "0888234567", null, "Боядисвач", 8 }
                });

            migrationBuilder.InsertData(
                table: "GalleryItems",
                columns: new[] { "Id", "AfterImageUrl", "BeforeImageUrl", "CarBrand", "CarModel", "CompletedDate", "CreatedDate", "Description", "IsVisible", "Title" },
                values: new object[,]
                {
                    { 1, "/images/gallery/merc-after.jpg", "/images/gallery/merc-before.jpg", "Mercedes", "E-Class", new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 12, 21, 27, 33, 835, DateTimeKind.Local).AddTicks(9170), "Пълно боядисване в перлено бяло със защитно покритие", true, "Mercedes E-Class - Перлено бяло" },
                    { 2, "/images/gallery/bmw-after.jpg", "/images/gallery/bmw-before.jpg", "BMW", "M3", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 12, 21, 27, 33, 835, DateTimeKind.Local).AddTicks(9175), "Специално матово покритие с керамична защита", true, "BMW M3 - Матово черно" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CreatedDate", "Description", "DurationDays", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 12, 21, 27, 33, 835, DateTimeKind.Local).AddTicks(9010), "Пълно боядисване на автомобила във всякакъв цвят по избор", 7, true, "Пълно боядисване", 2500m },
                    { 2, new DateTime(2026, 2, 12, 21, 27, 33, 835, DateTimeKind.Local).AddTicks(9014), "Боядисване на отделни елементи - врати, калници, капаци", 3, true, "Частично боядисване", 800m },
                    { 3, new DateTime(2026, 2, 12, 21, 27, 33, 835, DateTimeKind.Local).AddTicks(9018), "Професионално полиране и нанасяне на защитно покритие", 2, true, "Полиране и защита", 450m },
                    { 4, new DateTime(2026, 2, 12, 21, 27, 33, 835, DateTimeKind.Local).AddTicks(9029), "Специално матово покритие за уникален външен вид", 8, true, "Матово боядисване", 3200m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceId",
                table: "Appointments",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "GalleryItems");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
