using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class Initi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784), new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784), new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784), new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784), new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "DiscountEndDate", "DiscountStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784), new DateTime(2025, 5, 22, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784), new DateTime(2025, 5, 7, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784), new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(784) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(701), "admin@admin.com", new byte[] { 12, 164, 100, 77, 230, 91, 5, 6, 126, 105, 19, 45, 191, 96, 41, 78, 44, 229, 220, 209, 28, 151, 125, 164, 203, 46, 33, 248, 227, 221, 51, 202, 253, 98, 234, 59, 144, 239, 11, 113, 199, 218, 129, 121, 122, 42, 108, 78, 167, 49, 80, 36, 151, 217, 24, 96, 106, 223, 191, 142, 113, 197, 93, 173 }, new byte[] { 189, 165, 245, 17, 171, 240, 228, 244, 126, 172, 232, 245, 125, 87, 243, 55, 93, 163, 37, 145, 119, 184, 87, 225, 90, 136, 108, 123, 23, 163, 87, 97, 99, 13, 0, 250, 61, 162, 11, 51, 41, 24, 146, 75, 208, 111, 40, 6, 33, 172, 134, 45, 67, 7, 216, 162, 20, 35, 23, 249, 24, 155, 143, 114, 102, 127, 130, 23, 20, 248, 68, 21, 155, 128, 140, 15, 47, 159, 32, 71, 202, 82, 212, 209, 65, 185, 113, 74, 36, 174, 63, 171, 178, 80, 6, 158, 143, 124, 42, 196, 121, 45, 197, 199, 206, 75, 36, 129, 38, 202, 103, 213, 97, 137, 25, 111, 126, 247, 27, 51, 125, 218, 241, 89, 61, 181, 138, 25 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317), new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317), new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317), new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317), new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "DiscountEndDate", "DiscountStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317), new DateTime(2025, 5, 22, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317), new DateTime(2025, 5, 7, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317), new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2317) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 15, 41, 764, DateTimeKind.Utc).AddTicks(2113), "admin@system.com", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } });
        }
    }
}
