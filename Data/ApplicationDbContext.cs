using Microsoft.EntityFrameworkCore;
using Models;
namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Entità principali
        public DbSet<Complesso> Complessi { get; set; }
        public DbSet<Edificio> Edifici { get; set; }
        public DbSet<UnitaImmobiliare> UnitaImmobiliari { get; set; }

        // Entità di servizio
        public DbSet<Strutture> Strutture { get; set; }
        public DbSet<Infissi> Infissi { get; set; }
        public DbSet<IdraulicoAdduzione> IdraulicoAdduzione { get; set; }
        public DbSet<ScarichiIdriciFognari> ScarichiIdriciFognari { get; set; }
        public DbSet<ImpiantiElettrici> ImpiantiElettrici { get; set; }
        public DbSet<ImpiantoClimaAcs> ImpiantoClimaAcs { get; set; }
        public DbSet<AltriImpianti> AltriImpianti { get; set; }
        public DbSet<DocumentiGenerali> DocumentiGenerali { get; set; }
        public DbSet<SegnalazioneProblema> SegnalazioneProblema { get; set; }

        // Entità per la sincronizzazione
        public DbSet<SyncRecord> SyncRecords { get; set; }
        public DbSet<FileRecord> FileRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Complesso -> Edifici (one-to-many)
            modelBuilder.Entity<Complesso>()
                .HasMany<Edificio>()
                .WithOne()
                .HasForeignKey("ComplessoId")
                .OnDelete(DeleteBehavior.Cascade);

            // Edificio -> UnitaImmobiliari (one-to-many)
            modelBuilder.Entity<Edificio>()
                .HasMany<UnitaImmobiliare>()
                .WithOne()
                .HasForeignKey("EdificioId")
                .OnDelete(DeleteBehavior.Cascade);

            // Complesso -> Strutture (many-to-many)
            modelBuilder.Entity<Complesso>()
                .HasMany<Strutture>()
                .WithMany()
                .UsingEntity(j => j.ToTable("ComplessoStrutture"));

            // Edificio -> Strutture (many-to-many)
            modelBuilder.Entity<Edificio>()
                .HasMany<Strutture>()
                .WithMany()
                .UsingEntity(j => j.ToTable("EdificioStrutture"));

            // UnitaImmobiliare -> Strutture (many-to-many)
            modelBuilder.Entity<UnitaImmobiliare>()
                .HasMany<Strutture>()
                .WithMany()
                .UsingEntity(j => j.ToTable("UnitaImmobiliareStrutture"));

            // Edificio -> Infissi (many-to-many)
            modelBuilder.Entity<Edificio>()
                .HasMany<Infissi>()
                .WithMany()
                .UsingEntity(j => j.ToTable("EdificioInfissi"));

            // UnitaImmobiliare -> Infissi (many-to-many)
            modelBuilder.Entity<UnitaImmobiliare>()
                .HasMany<Infissi>()
                .WithMany()
                .UsingEntity(j => j.ToTable("UnitaImmobiliareInfissi"));

            // Complesso -> IdraulicoAdduzione (many-to-many)
            modelBuilder.Entity<Complesso>()
                .HasMany<IdraulicoAdduzione>()
                .WithMany()
                .UsingEntity(j => j.ToTable("ComplessoIdraulicoAdduzione"));

            // Edificio -> IdraulicoAdduzione (many-to-many)
            modelBuilder.Entity<Edificio>()
                .HasMany<IdraulicoAdduzione>()
                .WithMany()
                .UsingEntity(j => j.ToTable("EdificioIdraulicoAdduzione"));

            // UnitaImmobiliare -> IdraulicoAdduzione (many-to-many)
            modelBuilder.Entity<UnitaImmobiliare>()
                .HasMany<IdraulicoAdduzione>()
                .WithMany()
                .UsingEntity(j => j.ToTable("UnitaImmobiliareIdraulicoAdduzione"));

            // Complesso -> ScarichiIdriciFognari (many-to-many)
            modelBuilder.Entity<Complesso>()
                .HasMany<ScarichiIdriciFognari>()
                .WithMany()
                .UsingEntity(j => j.ToTable("ComplessoScarichiIdriciFognari"));

            // Edificio -> ScarichiIdriciFognari (many-to-many)
            modelBuilder.Entity<Edificio>()
                .HasMany<ScarichiIdriciFognari>()
                .WithMany()
                .UsingEntity(j => j.ToTable("EdificioScarichiIdriciFognari"));

            // UnitaImmobiliare -> ScarichiIdriciFognari (many-to-many)
            modelBuilder.Entity<UnitaImmobiliare>()
                .HasMany<ScarichiIdriciFognari>()
                .WithMany()
                .UsingEntity(j => j.ToTable("UnitaImmobiliareScarichiIdriciFognari"));

            // Complesso -> ImpiantoClimaAcs (many-to-many)
            modelBuilder.Entity<Complesso>()
                .HasMany<ImpiantoClimaAcs>()
                .WithMany()
                .UsingEntity(j => j.ToTable("ComplessoImpiantoClimaAcs"));

            // Edificio -> ImpiantoClimaAcs (many-to-many)
            modelBuilder.Entity<Edificio>()
                .HasMany<ImpiantoClimaAcs>()
                .WithMany()
                .UsingEntity(j => j.ToTable("EdificioImpiantoClimaAcs"));

            // UnitaImmobiliare -> ImpiantoClimaAcs (many-to-many)
            modelBuilder.Entity<UnitaImmobiliare>()
                .HasMany<ImpiantoClimaAcs>()
                .WithMany()
                .UsingEntity(j => j.ToTable("UnitaImmobiliareImpiantoClimaAcs"));

            // Complesso -> ImpiantiElettrici (many-to-many)
            modelBuilder.Entity<Complesso>()
                .HasMany<ImpiantiElettrici>()
                .WithMany()
                .UsingEntity(j => j.ToTable("ComplessoImpiantiElettrici"));

            // Edificio -> ImpiantiElettrici (many-to-many)
            modelBuilder.Entity<Edificio>()
                .HasMany<ImpiantiElettrici>()
                .WithMany()
                .UsingEntity(j => j.ToTable("EdificioImpiantiElettrici"));

            // UnitaImmobiliare -> ImpiantiElettrici (many-to-many)
            modelBuilder.Entity<UnitaImmobiliare>()
                .HasMany<ImpiantiElettrici>()
                .WithMany()
                .UsingEntity(j => j.ToTable("UnitaImmobiliareImpiantiElettrici"));

            // Complesso -> AltriImpianti (many-to-many)
            modelBuilder.Entity<Complesso>()
                .HasMany<AltriImpianti>()
                .WithMany()
                .UsingEntity(j => j.ToTable("ComplessoAltriImpianti"));

            // Edificio -> AltriImpianti (many-to-many)
            modelBuilder.Entity<Edificio>()
                .HasMany<AltriImpianti>()
                .WithMany()
                .UsingEntity(j => j.ToTable("EdificioAltriImpianti"));

            // UnitaImmobiliare -> AltriImpianti (many-to-many)
            modelBuilder.Entity<UnitaImmobiliare>()
                .HasMany<AltriImpianti>()
                .WithMany()
                .UsingEntity(j => j.ToTable("UnitaImmobiliareAltriImpianti"));

            // Complesso -> DocumentiGenerali (many-to-many)
            modelBuilder.Entity<Complesso>()
                .HasMany<DocumentiGenerali>()
                .WithMany()
                .UsingEntity(j => j.ToTable("ComplessoDocumentiGenerali"));

            // Edificio -> DocumentiGenerali (many-to-many)
            modelBuilder.Entity<Edificio>()
                .HasMany<DocumentiGenerali>()
                .WithMany()
                .UsingEntity(j => j.ToTable("EdificioDocumentiGenerali"));

            // UnitaImmobiliare -> DocumentiGenerali (many-to-many)
            modelBuilder.Entity<UnitaImmobiliare>()
                .HasMany<DocumentiGenerali>()
                .WithMany()
                .UsingEntity(j => j.ToTable("UnitaImmobiliareDocumentiGenerali"));

            // Strutture -> SegnalazioneProblema (one-to-many)
            modelBuilder.Entity<Strutture>()
                .HasMany<SegnalazioneProblema>()
                .WithOne()
                .HasForeignKey("StruttureId")
                .OnDelete(DeleteBehavior.Cascade);

            // Configurazione delle entità per la sincronizzazione
            modelBuilder.Entity<SyncRecord>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<FileRecord>()
                .HasKey(f => f.Id);
        }
    }
}