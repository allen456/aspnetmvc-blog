using aspnetmvc_blog.Models;
using aspnetmvc_blog.Models.Views;
using Microsoft.EntityFrameworkCore;

namespace aspnetmvc_blog.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<RoleModule> RoleModules { get; set; }
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGrant> UserGrants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppSetting>().ToTable("AppSetting");
            modelBuilder.Entity<AppLog>().ToTable("AppLog");
            modelBuilder.Entity<Group>().ToTable("Group");
            modelBuilder.Entity<Module>().ToTable("Module");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<User>().ToTable("User");

            modelBuilder.Entity<RoleModule>().ToTable("RoleModule");
            modelBuilder.Entity<UserModule>().ToTable("UserModule");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<UserGroup>().ToTable("UserGroup");
            modelBuilder.Entity<UserGrant>().ToTable("UserGrant");

            modelBuilder.Entity<UserGrant>()
                .HasOne(m => m.TargetUser)
                .WithMany(t => t.TargetGrant)
                .HasForeignKey(m => m.TargetId)
                .IsRequired(true);

            modelBuilder.Entity<UserGrant>()
                .HasOne(m => m.SourceUser)
                .WithMany(t => t.SourceGrant)
                .HasForeignKey(m => m.SourceId)
                .IsRequired(true);


            var cascadeFKs = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()).Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
