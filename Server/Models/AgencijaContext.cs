using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class AgencijaContext : DbContext
    {
        public DbSet<Nekretnina> Nekretnine { get;  set;}
        public DbSet<Stanovnik> Stanovnici { get;  set;}
        public DbSet<Kompanija> Kompanije { get;  set;}
        public DbSet<Grad> Gradovi { get;  set;}        
        public DbSet<Ugovor> Ugovori { get;  set;}
        public DbSet<UgovorKompanije> UgovoriKompanija { get;  set;}


        public AgencijaContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Stanovnik>()
                        .HasMany<Ugovor>()
                        .WithOne(p => p.Kupac);

            modelBuilder.Entity<Stanovnik>()
                        .HasMany<Ugovor>()
                        .WithOne(p => p.Prodavac);   

            modelBuilder.Entity<Nekretnina>()
                        .HasOne<UgovorKompanije>(u => u.NekretninaUgovorKompanije)
                        .WithOne(p => p.Objekat)
                        .HasForeignKey<UgovorKompanije>(b => b.NekretninaFK);
                                 
        }
    }
}