namespace control_notas_cit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alumnoes",
                c => new
                    {
                        AlumnoID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Cedula = c.String(),
                        Telefono = c.String(),
                        Email = c.String(),
                        Celula_CelulaID = c.Int(),
                    })
                .PrimaryKey(t => t.AlumnoID)
                .ForeignKey("dbo.Celulas", t => t.Celula_CelulaID)
                .Index(t => t.Celula_CelulaID);
            
            CreateTable(
                "dbo.Asistencias",
                c => new
                    {
                        AsistenciaID = c.Int(nullable: false, identity: true),
                        Asistio = c.Boolean(),
                        Alumno_AlumnoID = c.Int(nullable: false),
                        Celula_CelulaID = c.Int(),
                        Semana_SemanaID = c.Int(),
                    })
                .PrimaryKey(t => t.AsistenciaID)
                .ForeignKey("dbo.Alumnoes", t => t.Alumno_AlumnoID, cascadeDelete: true)
                .ForeignKey("dbo.Celulas", t => t.Celula_CelulaID)
                .ForeignKey("dbo.Semanas", t => t.Semana_SemanaID)
                .Index(t => t.Alumno_AlumnoID)
                .Index(t => t.Celula_CelulaID)
                .Index(t => t.Semana_SemanaID);
            
            CreateTable(
                "dbo.Celulas",
                c => new
                    {
                        CelulaID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Proyecto_ProyectoID = c.Int(),
                    })
                .PrimaryKey(t => t.CelulaID)
                .ForeignKey("dbo.Proyectoes", t => t.Proyecto_ProyectoID)
                .Index(t => t.Proyecto_ProyectoID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Cedula = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Celula_CelulaID = c.Int(),
                        Proyecto_ProyectoID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Celulas", t => t.Celula_CelulaID)
                .ForeignKey("dbo.Proyectoes", t => t.Proyecto_ProyectoID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Celula_CelulaID)
                .Index(t => t.Proyecto_ProyectoID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Proyectoes",
                c => new
                    {
                        ProyectoID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        CalendarioActualID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProyectoID);
            
            CreateTable(
                "dbo.Calendarios",
                c => new
                    {
                        CalendarioID = c.Int(nullable: false, identity: true),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFinal = c.DateTime(nullable: false),
                        SemanaActualID = c.Int(nullable: false),
                        IsLastWeek = c.Boolean(),
                        Finalizado = c.Boolean(),
                        Notas_Minutas_Valor = c.Single(),
                        Notas_Asistencias_Valor = c.Single(),
                        Notas_Evaluacion_Final_Valor = c.Single(),
                        Proyecto_ProyectoID = c.Int(),
                    })
                .PrimaryKey(t => t.CalendarioID)
                .ForeignKey("dbo.Proyectoes", t => t.Proyecto_ProyectoID)
                .Index(t => t.Proyecto_ProyectoID);
            
            CreateTable(
                "dbo.Notas",
                c => new
                    {
                        NotaID = c.Int(nullable: false, identity: true),
                        Nota_Minutas = c.Single(),
                        Nota_Asistencia = c.Single(),
                        Nota_EvaluacionFinal = c.Single(),
                        Nota_Final = c.Single(),
                        Alumno_AlumnoID = c.Int(nullable: false),
                        Calendario_CalendarioID = c.Int(),
                    })
                .PrimaryKey(t => t.NotaID)
                .ForeignKey("dbo.Alumnoes", t => t.Alumno_AlumnoID, cascadeDelete: true)
                .ForeignKey("dbo.Calendarios", t => t.Calendario_CalendarioID)
                .Index(t => t.Alumno_AlumnoID)
                .Index(t => t.Calendario_CalendarioID);
            
            CreateTable(
                "dbo.Semanas",
                c => new
                    {
                        SemanaID = c.Int(nullable: false, identity: true),
                        NumeroSemana = c.Int(nullable: false),
                        Actividad = c.String(),
                        Descripcion = c.String(),
                        Iniciada = c.Boolean(nullable: false),
                        Finalizada = c.Boolean(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Calendario_CalendarioID = c.Int(),
                    })
                .PrimaryKey(t => t.SemanaID)
                .ForeignKey("dbo.Calendarios", t => t.Calendario_CalendarioID)
                .Index(t => t.Calendario_CalendarioID);
            
            CreateTable(
                "dbo.Minutas",
                c => new
                    {
                        MinutaID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Aprobada = c.Boolean(),
                        Contenido = c.String(),
                        Celula_CelulaID = c.Int(),
                        Semana_SemanaID = c.Int(),
                    })
                .PrimaryKey(t => t.MinutaID)
                .ForeignKey("dbo.Celulas", t => t.Celula_CelulaID)
                .ForeignKey("dbo.Semanas", t => t.Semana_SemanaID)
                .Index(t => t.Celula_CelulaID)
                .Index(t => t.Semana_SemanaID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Proyecto_ProyectoID", "dbo.Proyectoes");
            DropForeignKey("dbo.Celulas", "Proyecto_ProyectoID", "dbo.Proyectoes");
            DropForeignKey("dbo.Minutas", "Semana_SemanaID", "dbo.Semanas");
            DropForeignKey("dbo.Minutas", "Celula_CelulaID", "dbo.Celulas");
            DropForeignKey("dbo.Semanas", "Calendario_CalendarioID", "dbo.Calendarios");
            DropForeignKey("dbo.Asistencias", "Semana_SemanaID", "dbo.Semanas");
            DropForeignKey("dbo.Calendarios", "Proyecto_ProyectoID", "dbo.Proyectoes");
            DropForeignKey("dbo.Notas", "Calendario_CalendarioID", "dbo.Calendarios");
            DropForeignKey("dbo.Notas", "Alumno_AlumnoID", "dbo.Alumnoes");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Celula_CelulaID", "dbo.Celulas");
            DropForeignKey("dbo.Asistencias", "Celula_CelulaID", "dbo.Celulas");
            DropForeignKey("dbo.Alumnoes", "Celula_CelulaID", "dbo.Celulas");
            DropForeignKey("dbo.Asistencias", "Alumno_AlumnoID", "dbo.Alumnoes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Minutas", new[] { "Semana_SemanaID" });
            DropIndex("dbo.Minutas", new[] { "Celula_CelulaID" });
            DropIndex("dbo.Semanas", new[] { "Calendario_CalendarioID" });
            DropIndex("dbo.Notas", new[] { "Calendario_CalendarioID" });
            DropIndex("dbo.Notas", new[] { "Alumno_AlumnoID" });
            DropIndex("dbo.Calendarios", new[] { "Proyecto_ProyectoID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Proyecto_ProyectoID" });
            DropIndex("dbo.AspNetUsers", new[] { "Celula_CelulaID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Celulas", new[] { "Proyecto_ProyectoID" });
            DropIndex("dbo.Asistencias", new[] { "Semana_SemanaID" });
            DropIndex("dbo.Asistencias", new[] { "Celula_CelulaID" });
            DropIndex("dbo.Asistencias", new[] { "Alumno_AlumnoID" });
            DropIndex("dbo.Alumnoes", new[] { "Celula_CelulaID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Minutas");
            DropTable("dbo.Semanas");
            DropTable("dbo.Notas");
            DropTable("dbo.Calendarios");
            DropTable("dbo.Proyectoes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Celulas");
            DropTable("dbo.Asistencias");
            DropTable("dbo.Alumnoes");
        }
    }
}
