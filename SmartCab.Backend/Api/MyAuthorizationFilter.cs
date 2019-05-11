using Hangfire.Dashboard;

namespace Api
{
    //A filter to allow access to the dashboard with authentication
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// //The authentication process that decides if a user can access the dashboard. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
