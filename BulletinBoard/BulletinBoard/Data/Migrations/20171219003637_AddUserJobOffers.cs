using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BulletinBoard.Data.Migrations
{
    public partial class AddUserJobOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "JobOffers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_AuthorId",
                table: "JobOffers",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_AspNetUsers_AuthorId",
                table: "JobOffers",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_AspNetUsers_AuthorId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_AuthorId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "JobOffers");
        }
    }
}
