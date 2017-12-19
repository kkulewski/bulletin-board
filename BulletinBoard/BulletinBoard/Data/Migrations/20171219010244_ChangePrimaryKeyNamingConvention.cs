using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BulletinBoard.Data.Migrations
{
    public partial class ChangePrimaryKeyNamingConvention : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobTypeId",
                table: "JobTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "JobOfferId",
                table: "JobOffers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "JobCategoryId",
                table: "JobCategories",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "JobTypes",
                newName: "JobTypeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "JobOffers",
                newName: "JobOfferId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "JobCategories",
                newName: "JobCategoryId");
        }
    }
}
