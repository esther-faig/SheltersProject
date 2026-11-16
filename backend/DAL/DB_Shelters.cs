using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DB_Shelters : DbContext
    {
        //בנאי שמקבל את המידע על החיבור למסד הנתונים
        public DB_Shelters(DbContextOptions<DB_Shelters> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Preference> Preference { get; set; }
        public DbSet<UserPreference> UserPreference { get; set; }
        public DbSet<Shelter> Shelter { get; set; }
        public DbSet<ShelterFeature> ShelterFeature { get; set; }
        public DbSet<AlertZone> AlertZone { get; set; }
        public DbSet<ProtectionTime> ProtectionTime { get; set; }

        //יצירת המסד נתונים
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // מפתח ראשי נשאר ה-Id (Identity) שבישות.
            // הייחודיות של הזוג נאכפת באינדקס ייחודי (במקום מפתח מורכב שמתנגש עם ה-Id)
            modelBuilder.Entity<UserPreference>()
                .HasIndex(up => new { up.UserId, up.PreferenceId })
                .IsUnique();

            modelBuilder.Entity<ShelterFeature>()
                .HasIndex(sf => new { sf.ShelterId, sf.PreferenceId })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<AlertZone>()
                .HasIndex(z => z.OfficialCode)
                .IsUnique();

            // דורש UseNetTopologySuite() בהגדרת ה-DbContext ב-Program.cs
            modelBuilder.Entity<AlertZone>()
                .Property(z => z.AreaGeometry)
                .HasColumnType("geography");

            // אינדקסים לחיפוש
            modelBuilder.Entity<Shelter>()
                .HasIndex(s => new { s.Status, s.IsActive });

            // ◄ אינדקס החדש לסינון הריבוע הגיאוגרפי לפי  ו־:
            // אינדקס לסינון הריבוע הגיאוגרפי)
            modelBuilder.Entity<Shelter>()
                .HasIndex(s => new { s.Latitude, s.Longitude });

            modelBuilder.Entity<ProtectionTime>()
                .HasIndex(p => new { p.AlertZoneId, p.AlertType, p.EffectiveFrom });

            // מחיקת אזור התרעה לא מוחקת מקלטים, רק מאפסת את האזור שלהם
            modelBuilder.Entity<Shelter>()
                .HasOne(s => s.AlertZone)
                .WithMany(z => z.Shelters)
                .HasForeignKey(s => s.AlertZoneId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Shelter>()
                .HasOne(s => s.User)
                .WithMany(u => u.Shelters)
                .HasForeignKey(s => s.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // מחיקת העדפה שנמצאת בשימוש נחסמת (במקום למחוק בשקט את ההעדפות של כל המשתמשים)
            modelBuilder.Entity<UserPreference>()
                .HasOne(up => up.Preference)
                .WithMany(p => p.UserPreferences)
                .HasForeignKey(up => up.PreferenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShelterFeature>()
                .HasOne(sf => sf.Preference)
                .WithMany(p => p.ShelterFeatures)
                .HasForeignKey(sf => sf.PreferenceId)
                .OnDelete(DeleteBehavior.Restrict);

            // ───────── נתוני התחלה: סוגי ההעדפות מהאפיון ─────────
            // מזהים קבועים ולעולם לא משתמשים בהם מחדש. העדפות שהמנהל יוסיף אחר כך יקבלו 7 ומעלה
            modelBuilder.Entity<Preference>().HasData(
                // נגישות
                new Preference
                {
                    PreferenceId = 1,
                    Name = "כניסה ללא מדרגות",
                    Description = "ניתן להגיע למקלט ללא עלייה או ירידה במדרגות",
                    Category = "נגישות"
                },
                new Preference
                {
                    PreferenceId = 2,
                    Name = "גישה לכיסא גלגלים",
                    Description = "הכניסה והתנועה במקלט מתאימות לאדם המשתמש בכיסא גלגלים",
                    Category = "נגישות"
                },
                new Preference
                {
                    PreferenceId = 3,
                    Name = "מעלית",
                    Description = "קיימת מעלית בדרך אל המקלט כאשר נדרשת עלייה או ירידה בקומות",
                    Category = "נגישות"
                },
                new Preference
                {
                    PreferenceId = 4,
                    Name = "שירותים נגישים",
                    Description = "קיימים שירותים המותאמים לאנשים עם מוגבלות",
                    Category = "נגישות"
                },

                // נוחות ואוורור
                new Preference
                {
                    PreferenceId = 5,
                    Name = "מרחב גדול",
                    Description = "המקלט מרווח ומאפשר שהייה נוחה",
                    Category = "נוחות ואוורור"
                },
                new Preference
                {
                    PreferenceId = 6,
                    Name = "אוורור טבעי",
                    Description = "קיימים חלונות או פתחי אוורור טבעיים",
                    Category = "נוחות ואוורור"
                },
                new Preference
                {
                    PreferenceId = 7,
                    Name = "מיזוג אוויר",
                    Description = "קיים מזגן או פתרון קירור",
                    Category = "נוחות ואוורור"
                },
                new Preference
                {
                    PreferenceId = 8,
                    Name = "תאורה טובה",
                    Description = "התאורה במקלט מספקת לשהייה בטוחה ונוחה",
                    Category = "נוחות ואוורור"
                },
                new Preference
                {
                    PreferenceId = 9,
                    Name = "מקומות ישיבה",
                    Description = "יש במקלט כיסאות, ספסלים או מקומות ישיבה",
                    Category = "נוחות ואוורור"
                },

                // מתקנים ושירותים
                new Preference
                {
                    PreferenceId = 10,
                    Name = "שירותים",
                    Description = "קיימים שירותים במקלט או בצמוד אליו",
                    Category = "מתקנים ושירותים"
                },
                new Preference
                {
                    PreferenceId = 11,
                    Name = "מי שתייה",
                    Description = "יש גישה למי שתייה",
                    Category = "מתקנים ושירותים"
                },
                new Preference
                {
                    PreferenceId = 12,
                    Name = "חשמל",
                    Description = "קיימות נקודות חשמל זמינות",
                    Category = "מתקנים ושירותים"
                },
                new Preference
                {
                    PreferenceId = 13,
                    Name = "קליטה סלולרית",
                    Description = "יש קליטה סלולרית בתוך המקלט",
                    Category = "מתקנים ושירותים"
                },
                new Preference
                {
                    PreferenceId = 14,
                    Name = "אינטרנט",
                    Description = "קיים חיבור Wi-Fi או חיבור אינטרנט זמין",
                    Category = "מתקנים ושירותים"
                },

                // מאפיינים נוספים
                new Preference
                {
                    PreferenceId = 15,
                    Name = "כניסה עם בעלי חיים",
                    Description = "מותר להיכנס למקלט עם בעלי חיים",
                    Category = "מאפיינים נוספים"
                }
            );






        }
    }
}