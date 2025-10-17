using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCatgory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_Category_category_id",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "categories");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "categories",
                newName: "modified_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "categories",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "categories",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "categories",
                newName: "category_name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "categories",
                newName: "category_id");

            migrationBuilder.AlterColumn<string>(
                name: "category_name",
                table: "categories",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "uq_categories_category_name",
                table: "categories",
                column: "category_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_category_id",
                table: "products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_category_id",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "uq_categories_category_name",
                table: "categories");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "modified_at",
                table: "Category",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Category",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Category",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "category_name",
                table: "Category",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Category",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Category",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_Category_category_id",
                table: "products",
                column: "category_id",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
