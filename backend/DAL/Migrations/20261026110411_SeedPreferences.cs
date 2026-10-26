using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "PreferenceId", "Category", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "נגישות", "הגעה למקלט ללא צורך בעלייה או בירידה במדרגות", "ללא מדרגות" },
                    { 2, "נגישות", "כניסה ותנועה בתוך המקלט מתאימות לכיסא גלגלים", "נגיש לכיסא גלגלים" },
                    { 3, "מרחב", "מקלט מרווח, מתאים למי שזקוק ליותר מקום, למשל בגלל קוצר נשימה", "מרחב גדול" },
                    { 4, "נוחות", "אוורור או כניסת אוויר מספקים בתוך המקלט", "אוורור טוב" },
                    { 5, "מרחב", "מקלט שבדרך כלל שוהים בו מעט אנשים ביחס לגודלו", "צפיפות נמוכה" },
                    { 6, "נוחות", "במקלט יש מקומות ישיבה", "מקומות ישיבה" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 6);
        }
    }
}
