using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DB_Shelters: DbContext
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
            // מזהים קבועים (1-6) ולעולם לא משתמשים בהם מחדש. העדפות שהמנהל יוסיף אחר כך יקבלו 7 ומעלה
            modelBuilder.Entity<Preference>().HasData(
                new Preference
                {
                    PreferenceId = 1,
                    Name = "ללא מדרגות",
                    Description = "הגעה למקלט ללא צורך בעלייה או בירידה במדרגות",
                    Category = "נגישות"
                },
                new Preference
                {
                    PreferenceId = 2,
                    Name = "נגיש לכיסא גלגלים",
                    Description = "כניסה ותנועה בתוך המקלט מתאימות לכיסא גלגלים",
                    Category = "נגישות"
                },
                new Preference
                {
                    PreferenceId = 3,
                    Name = "מרחב גדול",
                    Description = "מקלט מרווח, מתאים למי שזקוק ליותר מקום, למשל בגלל קוצר נשימה",
                    Category = "מרחב"
                },
                new Preference
                {
                    PreferenceId = 4,
                    Name = "אוורור טוב",
                    Description = "אוורור או כניסת אוויר מספקים בתוך המקלט",
                    Category = "נוחות"
                },
                new Preference
                {
                    PreferenceId = 5,
                    Name = "צפיפות נמוכה",
                    Description = "מקלט שבדרך כלל שוהים בו מעט אנשים ביחס לגודלו",
                    Category = "מרחב"
                },
                new Preference
                {
                    PreferenceId = 6,
                    Name = "מקומות ישיבה",
                    Description = "במקלט יש מקומות ישיבה",
                    Category = "נוחות"
                });
        }


    }
}
