using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JTA.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateAppRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a new login role for your app
            migrationBuilder.Sql("CREATE ROLE jta_app WITH LOGIN PASSWORD 'your_secure_password';");

            // Grant permissions to the role (you can adjust permissions as needed)
            migrationBuilder.Sql("GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO jta_app;");

            // Optionally, grant access to sequences if necessary
            migrationBuilder.Sql("GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO jta_app;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revoke the granted permissions
            migrationBuilder.Sql("REVOKE SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public FROM jta_app;");

            // Optionally, revoke access to sequences if you granted it
            migrationBuilder.Sql("REVOKE USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public FROM jta_app;");

            // Drop the login role
            migrationBuilder.Sql("DROP ROLE IF EXISTS jta_app;");
        }
    }

}
