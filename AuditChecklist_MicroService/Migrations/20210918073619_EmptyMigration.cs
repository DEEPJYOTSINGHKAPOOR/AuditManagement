using Microsoft.EntityFrameworkCore.Migrations;

namespace AuditChecklist_MicroService.Migrations
{
    public partial class EmptyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 0 + " , 'Have all Change requests followed SDLC before PROD move?' " + ")");
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 0 + " , 'Have all Change requests been approved by the application owner?' " + ")");
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 0 + " , 'Are all artifacts like CR document, Unit test cases available?' " + ")");
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 0 + " , 'Is the SIT and UAT sign-off available?' " + ")");
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 0 + " , 'Is data deletion from the system done with application owner approval?' " + ")");

            //////Sox questions.
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 1 + " , 'Have all Change requests followed SDLC before PROD move?' " + ")");
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 1 + " , 'Have all Change requests been approved by the application owner?' " + ")");
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 1 + " , 'For a major change, was there a database backup taken before and after PROD move?' " + ")");
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 1 + " , 'Has the application owner approval obtained while adding a user to the system?' " + ")");
            //migrationBuilder.Sql($"INSERT INTO dbo.AuditQuestions VALUES (" + 1 + " , 'Is data deletion from the system done with application owner approval?' " + ")");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
