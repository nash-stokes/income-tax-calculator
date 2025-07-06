using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DML_Seed_Tax_Band_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaxBands",
                columns: new[] { "Id", "LowerLimit", "UpperLimit", "Rate" },
                values: new object[,]
                {
                    { 1, 0, 5000, 0 },
                    { 2, 5000, 20000, 20 },
                    { 3, 20000, null, 40 }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaxBands",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3 });
        }
    }
}
