using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClinicStaffAndAppointmentConsultation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AttendingStaffId",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsultationCompletedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsultationNotes",
                table: "Appointments",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsultationStartedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralHospital",
                table: "Appointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralReason",
                table: "Appointments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClinicStaff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    StaffRegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicStaff_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AttendingStaffId",
                table: "Appointments",
                column: "AttendingStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicStaff_ClinicId",
                table: "ClinicStaff",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicStaff_Email",
                table: "ClinicStaff",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicStaff_StaffRegistrationNumber",
                table: "ClinicStaff",
                column: "StaffRegistrationNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_ClinicStaff_AttendingStaffId",
                table: "Appointments",
                column: "AttendingStaffId",
                principalTable: "ClinicStaff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_ClinicStaff_AttendingStaffId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "ClinicStaff");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AttendingStaffId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AttendingStaffId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ConsultationCompletedAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ConsultationNotes",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ConsultationStartedAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ReferralHospital",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ReferralReason",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
