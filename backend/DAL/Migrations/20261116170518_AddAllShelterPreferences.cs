using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAllShelterPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "ניתן להגיע למקלט ללא עלייה או ירידה במדרגות", "כניסה ללא מדרגות" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "הכניסה והתנועה במקלט מתאימות לאדם המשתמש בכיסא גלגלים", "גישה לכיסא גלגלים" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "נגישות", "קיימת מעלית בדרך אל המקלט כאשר נדרשת עלייה או ירידה בקומות", "מעלית" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "נגישות", "קיימים שירותים המותאמים לאנשים עם מוגבלות", "שירותים נגישים" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "נוחות ואוורור", "המקלט מרווח ומאפשר שהייה נוחה", "מרחב גדול" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 6,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "נוחות ואוורור", "קיימים חלונות או פתחי אוורור טבעיים", "אוורור טבעי" });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "PreferenceId", "Category", "Description", "Name" },
                values: new object[,]
                {
                    { 7, "נוחות ואוורור", "קיים מזגן או פתרון קירור", "מיזוג אוויר" },
                    { 8, "נוחות ואוורור", "התאורה במקלט מספקת לשהייה בטוחה ונוחה", "תאורה טובה" },
                    { 9, "נוחות ואוורור", "יש במקלט כיסאות, ספסלים או מקומות ישיבה", "מקומות ישיבה" },
                    { 10, "מתקנים ושירותים", "קיימים שירותים במקלט או בצמוד אליו", "שירותים" },
                    { 11, "מתקנים ושירותים", "יש גישה למי שתייה", "מי שתייה" },
                    { 12, "מתקנים ושירותים", "קיימות נקודות חשמל זמינות", "חשמל" },
                    { 13, "מתקנים ושירותים", "יש קליטה סלולרית בתוך המקלט", "קליטה סלולרית" },
                    { 14, "מתקנים ושירותים", "קיים חיבור Wi-Fi או חיבור אינטרנט זמין", "אינטרנט" },
                    { 15, "מאפיינים נוספים", "מותר להיכנס למקלט עם בעלי חיים", "כניסה עם בעלי חיים" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "הגעה למקלט ללא צורך בעלייה או בירידה במדרגות", "ללא מדרגות" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "כניסה ותנועה בתוך המקלט מתאימות לכיסא גלגלים", "נגיש לכיסא גלגלים" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "מרחב", "מקלט מרווח, מתאים למי שזקוק ליותר מקום, למשל בגלל קוצר נשימה", "מרחב גדול" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "נוחות", "אוורור או כניסת אוויר מספקים בתוך המקלט", "אוורור טוב" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "מרחב", "מקלט שבדרך כלל שוהים בו מעט אנשים ביחס לגודלו", "צפיפות נמוכה" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "PreferenceId",
                keyValue: 6,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "נוחות", "במקלט יש מקומות ישיבה", "מקומות ישיבה" });
        }
    }
}
