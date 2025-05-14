using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class InitialIdentitySetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_ProcessedByStaffId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProcessedByStaffId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Wishlists",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLoginAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Reviews",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedDate",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CancelledDate",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Carts",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Carts",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Books",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Books",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DiscountStartDate",
                table: "Books",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DiscountEndDate",
                table: "Books",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Books",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Banners",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Banners",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Banners",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Banners",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Banners",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 13, 3, 24, 30, 471, DateTimeKind.Utc).AddTicks(6091), new byte[] { 167, 155, 188, 213, 52, 212, 85, 33, 233, 172, 252, 185, 119, 196, 40, 159, 169, 1, 165, 31, 205, 174, 77, 236, 1, 3, 68, 236, 109, 51, 11, 230, 84, 107, 129, 242, 141, 141, 50, 71, 241, 98, 13, 92, 149, 24, 118, 6, 225, 141, 59, 142, 176, 184, 138, 149, 51, 206, 169, 31, 45, 16, 107, 236 }, new byte[] { 112, 79, 174, 0, 150, 157, 38, 73, 71, 30, 126, 107, 180, 2, 201, 182, 170, 160, 124, 19, 66, 235, 22, 242, 52, 42, 177, 153, 180, 0, 193, 170, 65, 145, 208, 218, 104, 120, 17, 192, 94, 239, 20, 85, 39, 67, 105, 123, 218, 136, 133, 154, 64, 121, 37, 78, 118, 145, 173, 109, 225, 214, 90, 217, 80, 79, 60, 193, 78, 254, 49, 245, 96, 170, 238, 148, 42, 31, 116, 7, 224, 41, 193, 11, 254, 86, 196, 25, 200, 111, 208, 42, 123, 215, 251, 1, 68, 18, 143, 53, 140, 162, 236, 160, 59, 168, 7, 44, 134, 195, 42, 40, 64, 161, 31, 139, 209, 237, 150, 103, 80, 182, 115, 76, 178, 196, 187, 160 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Wishlists",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLoginAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CancelledDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Carts",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Carts",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DiscountStartDate",
                table: "Books",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DiscountEndDate",
                table: "Books",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Banners",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Banners",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Banners",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Banners",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Banners",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AverageRating", "CoverImageUrl", "CreatedAt", "Description", "DiscountEndDate", "DiscountPrice", "DiscountStartDate", "Format", "Genre", "ISBN", "IsOnSale", "Language", "Price", "PublicationDate", "Publisher", "StockQuantity", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", 4.5, "/images/great-gatsby.jpg", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), "A story of wealth, love, and the American Dream in the 1920s.", null, null, null, "Paperback", "Classic", "9780743273565", false, "English", 12.99m, new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Scribner", 50, "The Great Gatsby", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) },
                    { 2, "Harper Lee", 4.7999999999999998, "/images/to-kill-a-mockingbird.jpg", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), "A powerful story of racial injustice and moral growth in the American South.", null, null, null, "Paperback", "Classic", "9780061120084", false, "English", 14.99m, new DateTime(1960, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), "HarperCollins", 45, "To Kill a Mockingbird", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) },
                    { 3, "George Orwell", 4.7000000000000002, "/images/1984.jpg", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), "A dystopian novel about totalitarianism, surveillance, and thought control.", null, null, null, "Paperback", "Dystopian", "9780451524935", false, "English", 11.99m, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Signet Classic", 60, "1984", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) },
                    { 4, "Jane Austen", 4.5999999999999996, "/images/pride-and-prejudice.jpg", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), "A romantic novel about the Bennet family and the proud Mr. Darcy.", null, null, null, "Paperback", "Romance", "9780141439518", false, "English", 9.99m, new DateTime(1813, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Penguin Classics", 40, "Pride and Prejudice", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) },
                    { 5, "J.R.R. Tolkien", 4.9000000000000004, "/images/the-hobbit.jpg", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), "A fantasy novel about the adventures of Bilbo Baggins.", new DateTime(2025, 5, 22, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), 10.99m, new DateTime(2025, 5, 7, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967), "Paperback", "Fantasy", "9780547928227", true, "English", 13.99m, new DateTime(1937, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Houghton Mifflin Harcourt", 55, "The Hobbit", new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5967) }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 12, 18, 55, 22, 525, DateTimeKind.Utc).AddTicks(5865), new byte[] { 103, 213, 55, 127, 215, 247, 17, 211, 241, 138, 169, 79, 220, 122, 228, 19, 183, 33, 88, 189, 217, 9, 123, 143, 97, 141, 51, 143, 122, 41, 17, 206, 204, 33, 129, 237, 52, 247, 107, 240, 182, 232, 3, 223, 152, 67, 92, 216, 72, 18, 81, 17, 195, 140, 121, 215, 30, 67, 233, 66, 131, 106, 129, 247 }, new byte[] { 238, 147, 207, 154, 222, 174, 252, 104, 10, 12, 170, 83, 216, 107, 126, 232, 48, 194, 39, 50, 105, 151, 168, 164, 73, 140, 169, 4, 171, 187, 206, 226, 221, 23, 129, 42, 61, 202, 140, 155, 32, 250, 172, 211, 12, 129, 233, 162, 141, 221, 0, 175, 63, 222, 63, 194, 87, 199, 124, 123, 70, 245, 253, 110, 41, 72, 98, 160, 250, 214, 212, 118, 215, 154, 123, 251, 73, 218, 94, 2, 250, 122, 245, 23, 156, 36, 13, 123, 176, 193, 238, 50, 177, 73, 169, 180, 92, 82, 64, 55, 44, 226, 143, 75, 25, 233, 98, 62, 226, 241, 176, 244, 117, 241, 71, 211, 101, 129, 104, 176, 124, 204, 23, 98, 71, 154, 28, 12 } });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProcessedByStaffId",
                table: "Orders",
                column: "ProcessedByStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_ProcessedByStaffId",
                table: "Orders",
                column: "ProcessedByStaffId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
