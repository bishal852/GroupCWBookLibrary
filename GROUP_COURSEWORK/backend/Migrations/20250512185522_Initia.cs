using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class Initia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "DiscountEndDate", "DiscountStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), new DateTime(2025, 5, 22, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), new DateTime(2025, 5, 7, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5865), new byte[] { 103, 213, 55, 127, 215, 247, 17, 211, 241, 138, 169, 79, 220, 122, 228, 19, 183, 33, 88, 189, 217, 9, 123, 143, 97, 141, 51, 143, 122, 41, 17, 206, 204, 33, 129, 237, 52, 247, 107, 240, 182, 232, 3, 223, 152, 67, 92, 216, 72, 18, 81, 17, 195, 140, 121, 215, 30, 67, 233, 66, 131, 106, 129, 247 }, new byte[] { 238, 147, 207, 154, 222, 174, 252, 104, 10, 12, 170, 83, 216, 107, 126, 232, 48, 194, 39, 50, 105, 151, 168, 164, 73, 140, 169, 4, 171, 187, 206, 226, 221, 23, 129, 42, 61, 202, 140, 155, 32, 250, 172, 211, 12, 129, 233, 162, 141, 221, 0, 175, 63, 222, 63, 194, 87, 199, 124, 123, 70, 245, 253, 110, 41, 72, 98, 160, 250, 214, 212, 118, 215, 154, 123, 251, 73, 218, 94, 2, 250, 122, 245, 23, 156, 36, 13, 123, 176, 193, 238, 50, 177, 73, 169, 180, 92, 82, 64, 55, 44, 226, 143, 75, 25, 233, 98, 62, 226, 241, 176, 244, 117, 241, 71, 211, 101, 129, 104, 176, 124, 204, 23, 98, 71, 154, 28, 12 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 49, 37, 54, DateTimeKind.Utc).AddTicks(701), new byte[] { 12, 164, 100, 77, 230, 91, 5, 6, 126, 105, 19, 45, 191, 96, 41, 78, 44, 229, 220, 209, 28, 151, 125, 164, 203, 46, 33, 248, 227, 221, 51, 202, 253, 98, 234, 59, 144, 239, 11, 113, 199, 218, 129, 121, 122, 42, 108, 78, 167, 49, 80, 36, 151, 217, 24, 96, 106, 223, 191, 142, 113, 197, 93, 173 }, new byte[] { 189, 165, 245, 17, 171, 240, 228, 244, 126, 172, 232, 245, 125, 87, 243, 55, 93, 163, 37, 145, 119, 184, 87, 225, 90, 136, 108, 123, 23, 163, 87, 97, 99, 13, 0, 250, 61, 162, 11, 51, 41, 24, 146, 75, 208, 111, 40, 6, 33, 172, 134, 45, 67, 7, 216, 162, 20, 35, 23, 249, 24, 155, 143, 114, 102, 127, 130, 23, 20, 248, 68, 21, 155, 128, 140, 15, 47, 159, 32, 71, 202, 82, 212, 209, 65, 185, 113, 74, 36, 174, 63, 171, 178, 80, 6, 158, 143, 124, 42, 196, 121, 45, 197, 199, 206, 75, 36, 129, 38, 202, 103, 213, 97, 137, 25, 111, 126, 247, 27, 51, 125, 218, 241, 89, 61, 181, 138, 25 } });
        }
    }
}
