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

            modelBuilder.Ignore<NetTopologySuite.Geometries.Geometry>();

            // הגדרת מפתח מורכב לטבלת UserPreference
            modelBuilder.Entity<UserPreference>()
                .HasKey(up => new { up.UserId, up.PreferenceId });

            // הגדרת מפתח מורכב לטבלת ShelterFeature
            modelBuilder.Entity<ShelterFeature>()
                .HasKey(sf => new { sf.ShelterId, sf.PreferenceId });
        }


    }
}
