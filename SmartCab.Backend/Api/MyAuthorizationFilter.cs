﻿using Hangfire.Dashboard;

namespace Api
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return true;
        }
    }
}
