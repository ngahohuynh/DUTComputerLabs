using Microsoft.EntityFrameworkCore.Migrations;

namespace DUTComputerLabs.API.Migrations
{
    public partial class EditRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Feedbacks_FeedbackId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FeedbackId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks",
                column: "BookingId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FeedbackId",
                table: "Bookings",
                column: "FeedbackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Feedbacks_FeedbackId",
                table: "Bookings",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
