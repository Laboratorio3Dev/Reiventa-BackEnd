using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.BackOffice;
using Reinventa.Dominio.UsuarioRoles;


namespace Reinventa.Persistencia.BackOffice
{
    public class LAB_Context : DbContext
    {
        public LAB_Context(DbContextOptions<LAB_Context> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 👇 Aquí configuras que Menu no tiene clave (porque viene de un SP)
            modelBuilder.Entity<Menu>().HasNoKey();
            modelBuilder.Entity<Rol>().HasNoKey();
        }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Rol> Roles { get; set; }       
        public DbSet<LAB_Log> LAB_Log { get; set; }
        public DbSet<LAB_Usuario> LAB_Usuario { get; set; }
    }
}
