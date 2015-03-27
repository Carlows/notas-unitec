using control_notas_cit.Models.Entidades;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.ModelConfiguration.Conventions;
using control_notas_cit.Migrations;

namespace IdentitySample.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("CustomConnection")
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
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