using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BulletinBoard.Data.Migrations
{
    public partial class ChangeActiveFlagToVisitCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "JobOffers");

            migrationBuilder.AddColumn<int>(
                name: "Visits",
                table: "JobOffers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visits",
                table: "JobOffers");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "JobOffers",
                nullable: false,
                defaultValue: false);
        }
    }
}
