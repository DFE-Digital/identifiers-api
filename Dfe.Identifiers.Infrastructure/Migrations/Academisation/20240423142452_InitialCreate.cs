using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Identifiers.Infrastructure.Migrations.Academisation
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "academisation");

            migrationBuilder.CreateTable(
                name: "FormAMatProject",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAMatProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferProject",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Urn = table.Column<int>(type: "int", nullable: false),
                    ProjectReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutgoingTrustUkprn = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Urn = table.Column<int>(type: "int", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrustReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SponsorReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormAMatProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_FormAMatProject_FormAMatProjectId",
                        column: x => x.FormAMatProjectId,
                        principalSchema: "academisation",
                        principalTable: "FormAMatProject",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_FormAMatProjectId",
                schema: "academisation",
                table: "Project",
                column: "FormAMatProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project",
                schema: "academisation");

            migrationBuilder.DropTable(
                name: "TransferProject",
                schema: "academisation");

            migrationBuilder.DropTable(
                name: "FormAMatProject",
                schema: "academisation");
        }
    }
}
