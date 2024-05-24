using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShamsaStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class userpassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "683f15fa-430f-4c6b-a558-41c1f56c4cb5");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a1d33a9e-3341-4fbf-99c4-4b75ef9b13ec", null, "User", "USER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3348e0c0-a5f9-41bf-9b4b-cc25ac9e8ae2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "04dd1813-0af4-408b-a7cd-7956c230c609", "AQAAAAIAAYagAAAAELdmu1hrcnT4UGIEyzHrFzRqNVSMyr28iMcBZA4Sgv3pKUSzRmdz8VczkNHJl99TuA==", "Marziye" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1d33a9e-3341-4fbf-99c4-4b75ef9b13ec");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "683f15fa-430f-4c6b-a558-41c1f56c4cb5", null, "User", "USER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3348e0c0-a5f9-41bf-9b4b-cc25ac9e8ae2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UserName" },
                values: new object[] { "554e4367-c1e1-4df6-a14e-b40ced03126f", "AQAAAAIAAYagAAAAEI/TvZ+kqFm0dOANxk4FbkPROJ3WatR6F7QdBxBv8I12Bid3HbmfAhqS8m8SNz5YMA==", "Erfan" });
        }
    }
}
