using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace mascotas
{
    public partial class petDBContext : DbContext
    {
        public petDBContext()
        {
        }

        public petDBContext(DbContextOptions<petDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Canton> Cantons { get; set; }
        public virtual DbSet<PetBreed> PetBreeds { get; set; }
        public virtual DbSet<PetSpecie> PetSpecies { get; set; }
        public virtual DbSet<PetState> PetStates { get; set; }
        public virtual DbSet<PostImage> PostImages { get; set; }
        public virtual DbSet<PostPet> PostPets { get; set; }
        public virtual DbSet<Provincium> Provincia { get; set; }
        public virtual DbSet<Sector> Sectors { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Canton>(entity =>
            {
                entity.HasKey(e => e.IdCanton)
                    .HasName("PK_city_id");

                entity.ToTable("canton", "mydb");

                entity.HasIndex(e => e.Name, "city$city_name");

                entity.Property(e => e.IdCanton)
                    .ValueGeneratedNever()
                    .HasColumnName("id_canton");

                entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("name");

                entity.HasOne(d => d.IdProvinciaNavigation)
                    .WithMany(p => p.Cantons)
                    .HasForeignKey(d => d.IdProvincia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_canton_provincia");
            });

            modelBuilder.Entity<PetBreed>(entity =>
            {
                entity.HasKey(e => e.IdPetBreed)
                    .HasName("PK_pet_breed_id");

                entity.ToTable("pet_breed", "mydb");

                entity.HasIndex(e => e.IdSpecie, "fk_pet_breed_pet_specie_idx");

                entity.HasIndex(e => e.BreedName, "pet_breed$breed_type");

                entity.Property(e => e.IdPetBreed).HasColumnName("id_petBreed");

                entity.Property(e => e.BreedName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("breed_name");

                entity.Property(e => e.IdSpecie).HasColumnName("id_specie");

                entity.HasOne(d => d.IdSpecieNavigation)
                    .WithMany(p => p.PetBreeds)
                    .HasForeignKey(d => d.IdSpecie)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pet_breed$fk_pet_breed_pet_specie");
            });

            modelBuilder.Entity<PetSpecie>(entity =>
            {
                entity.HasKey(e => e.IdPetSpecie)
                    .HasName("PK_pet_specie_id");

                entity.ToTable("pet_specie", "mydb");

                entity.HasIndex(e => e.SpecieName, "pet_specie$specie_type");

                entity.Property(e => e.IdPetSpecie).HasColumnName("id_petSpecie");

                entity.Property(e => e.SpecieName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("specie_name");
            });

            modelBuilder.Entity<PetState>(entity =>
            {
                entity.HasKey(e => e.IdState)
                    .HasName("PK_state");

                entity.ToTable("pet_state");

                entity.Property(e => e.IdState)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("id_state")
                    .IsFixedLength(true);

                entity.Property(e => e.StateName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("state_name");
            });

            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.HasKey(e => e.IdImage)
                    .HasName("PK_post_images");

                entity.ToTable("post_image");

                entity.Property(e => e.IdImage).HasColumnName("id_image");

                entity.Property(e => e.IdPostPet).HasColumnName("id_postPet");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("url");

                entity.HasOne(d => d.IdPostPetNavigation)
                    .WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.IdPostPet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_images_post_pet");
            });

            modelBuilder.Entity<PostPet>(entity =>
            {
                entity.HasKey(e => e.IdPostPet)
                    .HasName("PK_post_pet_id");

                entity.ToTable("post_pet", "mydb");

                entity.HasIndex(e => e.IdPetSpecie, "fk_lost_pet_pet_type1_idx");

                entity.HasIndex(e => e.IdUser, "fk_lost_pet_user1_idx");

                entity.Property(e => e.IdPostPet).HasColumnName("id_postPet");

                entity.Property(e => e.Contact)
                    .HasMaxLength(50)
                    .HasColumnName("contact")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.IdCanton).HasColumnName("id_canton");

                entity.Property(e => e.IdPetBreed).HasColumnName("id_petBreed");

                entity.Property(e => e.IdPetSpecie).HasColumnName("id_petSpecie");

                entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");

                entity.Property(e => e.IdSector).HasColumnName("id_sector");

                entity.Property(e => e.IdState)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("id_state")
                    .IsFixedLength(true);

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.LastTimeSeen)
                    .HasColumnType("datetime")
                    .HasColumnName("last_timeSeen");

                entity.Property(e => e.LinkMapSeen).HasColumnName("link_mapSeen");

                entity.Property(e => e.PetAge)
                    .HasMaxLength(20)
                    .HasColumnName("pet_age");

                entity.Property(e => e.PetName)
                    .HasMaxLength(45)
                    .HasColumnName("pet_name");

                entity.Property(e => e.PetSpecialCondition).HasColumnName("pet_specialCondition");

                entity.Property(e => e.Reward)
                    .HasColumnType("money")
                    .HasColumnName("reward");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdCantonNavigation)
                    .WithMany(p => p.PostPets)
                    .HasForeignKey(d => d.IdCanton)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_pet_city");

                entity.HasOne(d => d.IdPetBreedNavigation)
                    .WithMany(p => p.PostPets)
                    .HasForeignKey(d => d.IdPetBreed)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_pet_pet_breed");

                entity.HasOne(d => d.IdPetSpecieNavigation)
                    .WithMany(p => p.PostPets)
                    .HasForeignKey(d => d.IdPetSpecie)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_pet_pet_specie");

                entity.HasOne(d => d.IdProvinciaNavigation)
                    .WithMany(p => p.PostPets)
                    .HasForeignKey(d => d.IdProvincia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_pet_provincia");

                entity.HasOne(d => d.IdSectorNavigation)
                    .WithMany(p => p.PostPets)
                    .HasForeignKey(d => d.IdSector)
                    .HasConstraintName("FK_post_pet_sector");

                entity.HasOne(d => d.IdStateNavigation)
                    .WithMany(p => p.PostPets)
                    .HasForeignKey(d => d.IdState)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_pet_state");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.PostPets)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_pet$fk_lost_pet_user1");
            });

            modelBuilder.Entity<Provincium>(entity =>
            {
                entity.HasKey(e => e.IdProvincia);

                entity.ToTable("provincia");

                entity.HasComment("");

                entity.Property(e => e.IdProvincia)
                    .ValueGeneratedNever()
                    .HasColumnName("Id_provincia");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Sector>(entity =>
            {
                entity.HasKey(e => e.IdSector)
                    .HasName("PK_sectoryciudadela_id");

                entity.ToTable("sector", "mydb");

                entity.HasIndex(e => e.IdCanton, "fk_Sector_city1_idx");

                entity.HasIndex(e => e.Name, "sectoryciudadela$sector_name_UNIQUE");

                entity.Property(e => e.IdSector)
                    .ValueGeneratedNever()
                    .HasColumnName("id_sector");

                entity.Property(e => e.IdCanton).HasColumnName("id_canton");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.IdCantonNavigation)
                    .WithMany(p => p.Sectors)
                    .HasForeignKey(d => d.IdCanton)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sectoryciudadela$fk_Sector_city1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK_user_id");

                entity.ToTable("user", "mydb");

                entity.HasIndex(e => e.Email, "user$email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.CellNumber)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("cell_number");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.FacebookProfile).HasColumnName("facebook_profile");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
