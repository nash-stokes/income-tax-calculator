using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class DML_Seed_Tax_Band_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaxBands",
                columns: new[] {"LowerLimit", "UpperLimit", "Rate" },
                values: new object[,]
                {
                    {0, 5000, 0 },
                    {5000, 20000, 20 },
                    {20000, null, 40 }
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
