using Microsoft.EntityFrameworkCore;
using PhotoArchiveCoilsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoArchiveCoilsWeb.Data
{
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options) : base(options)
        { }

        public DbSet<PhotoArchive> PhotoArchives { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhotoArchive>().Property(t => t.File).HasColumnType("VARBINARY(MAX) FILESTREAM");

            modelBuilder.Entity<PhotoArchive>().Property(m => m.Id).HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL").IsRequired();


            // ...

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }

    }
}
