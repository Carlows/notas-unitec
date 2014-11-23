using control_notas_cit.Models.Entidades;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace IdentitySample.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        // Propiedades personalizadas del usuario
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }

        public virtual Proyecto Proyecto { get; set; }
        public virtual Celula Celula { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
                
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public static ApplicationDbContext GetDBContext()
        {
            return HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        // DbSets personalizados en donde se guardara la data de la aplicación
        // La clase IdentityDbContext ya contiene Users y Roles
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Celula> Celulas { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Minuta> Minutas { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<Semana> Semanas { get; set; }
    }
}