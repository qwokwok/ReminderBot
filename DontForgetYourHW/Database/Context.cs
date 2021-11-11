using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using DontForgetYourHW.Database.GenshinImpact;

namespace DontForgetYourHW.Database
{
    public partial class Context : DbContext
    {
        public virtual DbSet<Timezone> Timezone { get; set; }
        public virtual DbSet<Resin> Resin { get; set; }
        public virtual DbSet<Artifact> Artifact { get; set; }
        public virtual DbSet<Crystal> Crystal { get; set; }
        public virtual DbSet<Domain> Domain { get; set; }
        public virtual DbSet<Anime> Anime { get; set; }
        public virtual DbSet<Manga> Manga { get; set; }
        public virtual DbSet<Homework> Homework { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Link> Link { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = $"Data Source=data/database.db";
            var builder = new SqliteConnectionStringBuilder(connectionString);
            builder.DataSource = Path.Combine(AppContext.BaseDirectory, builder.DataSource);
            optionsBuilder.UseSqlite(builder.ToString());
        }
    }
}
