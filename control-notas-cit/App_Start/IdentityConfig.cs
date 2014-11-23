using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using control_notas_cit.Models.Entidades;

namespace IdentitySample.Models
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = false,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = false;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            return manager;
        }
    }

    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }

    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {

            if (db == null)
            {
                throw new ArgumentNullException("db", "Context no puede ser nulo.");
            }
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

            Proyecto p = new Proyecto() { Nombre = "Centro de Innovacion Tecnologica", Descripcion = "Centro de Innovacion Tecnologica" };

            Celula c = new Celula() { Nombre = "Desarrollo de Software", Descripcion = "Desarrollo de una aplicacion que controle los proyectos UNITEC.", Proyecto = p };

            string roleName = "Admin";
            string coordinadorRole = "Coordinador";
            string profesorRole = "Profesor";

            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }

            if (!roleManager.RoleExists(coordinadorRole))
            {
                roleManager.Create(new IdentityRole(coordinadorRole));
            }

            if (!roleManager.RoleExists(profesorRole))
            {
                roleManager.Create(new IdentityRole(profesorRole));
            }

            //// Admin
            string userName = "admin@cit.com";
            string password = "admin123";
            string email = "admin@cit.com";

            ApplicationUser user = userManager.FindByName(userName);

            if (user == null)
            {
                IdentityResult result = userManager.Create(new ApplicationUser { UserName = userName, Email = email }, password);
                user = userManager.FindByName(userName);
            }

            if (!userManager.IsInRole(user.Id, roleName))
            {
                userManager.AddToRole(user.Id, roleName);
            }

            ///////////////////////////////

            //// Coordinador

            string coorUsername = "coordinador@cit.com";
            string coorEmail = "coordinador@cit.com";
            string coorPwd = "coordinador123";

            ApplicationUser coorUser = userManager.FindByName(coorUsername);

            if (coorUser == null)
            {
                IdentityResult result = userManager.Create(new ApplicationUser { UserName = coorUsername, Email = coorEmail, Nombre = "Test", Apellido = "Coordinador", Cedula = "23.522.896", Celula = c, Proyecto = p }, coorPwd);
                coorUser = userManager.FindByName(coorUsername);
            }

            if (!userManager.IsInRole(coorUser.Id, coordinadorRole))
            {
                userManager.AddToRole(coorUser.Id, coordinadorRole);
            }

            //////////////////////////////

            //// Profesor

            string profUsername = "profesor2@cit.com";
            string profEmail = "profesor2@cit.com";
            string profPwd = "profesor123";

            ApplicationUser profUser = userManager.FindByName(profUsername);

            if (profUser == null)
            {
                IdentityResult result = userManager.Create(new ApplicationUser { UserName = profUsername, Email = profEmail, Nombre = "Carlos", Apellido = "Martinez", Cedula = "23.522.896", Proyecto = p, PhoneNumber = "12321313" }, profPwd);
                profUser = userManager.FindByName(profUsername);
            }

            if (!userManager.IsInRole(profUser.Id, profesorRole))
            {
                userManager.AddToRole(profUser.Id, profesorRole);
            }

            ////////////////////////////

            profUsername = "profesor@cit.com";
            profEmail = "profesor@cit.com";
            profPwd = "profesor123";

            profUser = userManager.FindByName(profUsername);

            if (profUser == null)
            {
                IdentityResult result = userManager.Create(new ApplicationUser { UserName = profUsername, Email = profEmail, Nombre = "Joseito", Apellido = "Martinez", Cedula = "23.522.896", Proyecto = p, PhoneNumber = "12321313" }, profPwd);
                profUser = userManager.FindByName(profUsername);
            }

            if (!userManager.IsInRole(profUser.Id, profesorRole))
            {
                userManager.AddToRole(profUser.Id, profesorRole);
            }

            var r = new Random();
            //////////////////////////////  
            // Creo 10 profesores
            for (int x = 1; x <= 10; x++)
            {
                string usrnm = String.Format("profesor{0}@example.com", x);
                string usremail = String.Format("profesor{0}@example.com", x);
                string usrpwd = "profesor123";

                ApplicationUser appusrprof = userManager.FindByName(usrnm);

                Proyecto pytc = new Proyecto() { Nombre = "Proyecto prueba " + x, Descripcion = "Phasellus gravida semper nisi. Aenean tellus metus, bibendum sed, posuere ac, mattis non, nunc. Curabitur ligula sapien, tincidunt non, euismod vitae, posuere imperdiet." };

                for (int y = 1; y <= 4; y++)
                {
                    int num = r.Next(100);
                    string cuser = String.Format("coordinador{0}@example.com", num);
                    string cemail = String.Format("coordinador{0}@example.com", num);
                    string cpwd = "coordinador123";

                    ApplicationUser appcoord = userManager.FindByName(cuser);

                    if(appcoord == null) 
                    {
                        IdentityResult cresult = userManager.Create(new ApplicationUser{ UserName = cuser, Email = cemail, Nombre = "Prueba" + y, Apellido = "prueba", Cedula = "1234567", PhoneNumber = "123132", Proyecto = pytc }, cpwd);
                        appcoord = userManager.FindByName(cuser); 
                    }

                    if(!userManager.IsInRole(appcoord.Id, coordinadorRole))
                    {
                        userManager.AddToRole(appcoord.Id, coordinadorRole);
                    }
                }

                if (appusrprof == null)
                {
                    IdentityResult result = userManager.Create(new ApplicationUser { UserName = usrnm, Email = usremail, Nombre = "Prueba" + x, Apellido = "prueba", Cedula = "23.522.896", PhoneNumber = "1232132", Proyecto = pytc }, usrpwd);
                    appusrprof = userManager.FindByName(usrnm);
                }

                if (!userManager.IsInRole(appusrprof.Id, profesorRole))
                {
                    userManager.AddToRole(appusrprof.Id, profesorRole);
                }
            }

            //////////////////////////////

            //for (int x = 1; x <= 10; x++)
            //{
            //    Proyecto pytc = new Proyecto() { Nombre = "Proyecto prueba " + x, Descripcion = "Phasellus gravida semper nisi. Aenean tellus metus, bibendum sed, posuere ac, mattis non, nunc. Curabitur ligula sapien, tincidunt non, euismod vitae, posuere imperdiet." };
            //    db.Proyectos.Add(pytc);
            //}

            //db.SaveChanges();

            List<Proyecto> lista = db.Proyectos.ToList();            

            foreach (Proyecto project in lista)
            {
                for (int x = 1; x <= 4; x++)
                {
                    Celula celula = new Celula()
                    {
                        Nombre = "Celula prueba " + r.Next(100),
                        Descripcion = "Phasellus gravida semper nisi. Aenean tellus metus, bibendum sed, posuere ac, mattis non, nunc. Curabitur ligula sapien, tincidunt non, euismod vitae, posuere imperdiet, leo..",
                        Proyecto = project,
                        Alumnos = new List<Alumno>()
                        {
                            new Alumno(){ Nombre = "Alumno 1", Apellido = "Alumno 1", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" },
                            new Alumno(){ Nombre = "Alumno 2", Apellido = "Alumno 2", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" },
                            new Alumno(){ Nombre = "Alumno 3", Apellido = "Alumno 3", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" },
                            new Alumno(){ Nombre = "Alumno 4", Apellido = "Alumno 4", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" },
                            new Alumno(){ Nombre = "Alumno 5", Apellido = "Alumno 5", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" },
                            new Alumno(){ Nombre = "Alumno 6", Apellido = "Alumno 6", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" },
                            new Alumno(){ Nombre = "Alumno 7", Apellido = "Alumno 7", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" },
                            new Alumno(){ Nombre = "Alumno 8", Apellido = "Alumno 8", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" },
                            new Alumno(){ Nombre = "Alumno 9", Apellido = "Alumno 9", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" },
                            new Alumno(){ Nombre = "Alumno 10", Apellido = "Alumno 10", Telefono = "12345678", Cedula = "12345678", Email = "test@example.com" }
                        }
                    };

                    db.Celulas.Add(celula);
                }
            }

            db.SaveChanges();
        }
    }



    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}