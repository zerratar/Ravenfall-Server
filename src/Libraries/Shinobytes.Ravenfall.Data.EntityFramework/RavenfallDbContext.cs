using Microsoft.EntityFrameworkCore;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.Data.EntityFramework.Legacy
{
    public partial class RavenfallDbContext : DbContext
    {
        private readonly string connectionString;

        public RavenfallDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public RavenfallDbContext(DbContextOptions<RavenfallDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appearance> Appearance { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<InventoryItem> InventoryItem { get; set; }
        public virtual DbSet<ShopItem> ShopItem { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<GameObject> GameObject { get; set; }
        public virtual DbSet<Npc> Npc { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Transform> Transform { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<GameObjectInstance> GameObjectInstance { get; set; }
        public virtual DbSet<NpcInstance> NpcInstance { get; set; }
        public virtual DbSet<ItemDrop> ItemDrop { get; set; }
        public virtual DbSet<Professions> Professions { get; set; }
        public virtual DbSet<Attributes> Attributes { get; set; }
        public virtual DbSet<EntityAction> EntityAction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appearance>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<Transform>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<Attributes>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<Professions>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<EntityAction>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());
            
            modelBuilder.Entity<ItemDrop>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<GameObject>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<GameObjectInstance>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<NpcInstance>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<Session>(entity =>
                entity.Property(e => e.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Npc>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ShopItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("datetime");
            });
        }
    }
}
