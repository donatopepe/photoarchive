using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PhotoArchiveCoilsWeb.Models
{
    public partial class PhotoCoilsDbContext : DbContext
    {
        public PhotoCoilsDbContext()
        {
        }

        public PhotoCoilsDbContext(DbContextOptions<PhotoCoilsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PhotoArchives> PhotoArchives { get; set; }

        // Unable to generate entity type for table 'dbo.WebApiUploads'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=ID65426;Database=PhotoCoilsDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhotoArchives>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
