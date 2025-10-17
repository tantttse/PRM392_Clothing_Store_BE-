using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class audit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "categories",
                newName: "modified_by");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "categories",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "carts",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "cart_items",
                newName: "modified_by");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "cart_items",
                newName: "modified_at");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "cart_items",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "cart_items",
                newName: "created_at");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "categories",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "modified_by",
                table: "categories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "categories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "modified_by",
                table: "cart_items",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "cart_items",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "cart_items",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "modified_by",
                table: "categories",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "categories",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "carts",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "modified_by",
                table: "cart_items",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "modified_at",
                table: "cart_items",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "cart_items",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "cart_items",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "categories",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "cart_items",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "cart_items",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "cart_items",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "NOW()");
        }
    }
}
