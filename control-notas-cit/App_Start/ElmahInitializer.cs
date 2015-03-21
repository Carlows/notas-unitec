using control_notas_cit;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(ElmahInitializer), "Initialize")]

namespace control_notas_cit
{
    using Elmah.SqlServer.EFInitializer;

    public static class ElmahInitializer
    {
        public static void Initialize()
        {
            using (var context = new ElmahContext())
            {
                context.Database.Initialize(true);
            }
        }
    }
}