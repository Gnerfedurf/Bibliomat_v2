using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bibliomat.Common.Model
{
    public partial class BibliomatContext : DbContext
    {
        public BibliomatContext()
        {
        }

        public BibliomatContext(DbContextOptions<BibliomatContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Bibliomat;Trusted_Connection=True;");
                optionsBuilder.UseSqlServer("Server=tcp:bibliomat.database.windows.net,1433;Initial Catalog=Bibliomat;Persist Security Info=False;User ID=gnerfedurf;Password=AwD_123_+;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Title).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Book)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Book__UserID__25869641");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Pass).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
