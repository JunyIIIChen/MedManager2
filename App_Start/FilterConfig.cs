﻿using System.Web;
using System.Web.Mvc;

namespace MedManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // this is a security implementation
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
