﻿using System.Web.Mvc;

namespace IdentitySample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new Elmah.Contrib.Mvc.ElmahHandleErrorAttribute());
        }
    }
}
