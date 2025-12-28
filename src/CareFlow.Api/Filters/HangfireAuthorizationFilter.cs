using Hangfire.Dashboard;

namespace CareFlow.Api.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        // In production, ensure the user is authenticated and is an Admin
        // This requires the user to be logged in via the main app (cookie or token)
        // Since this is an API, typically you access Hangfire via a secure tunnel or 
        // using a specific middleware that authenticates the request.
        
        // For simplicity in this demo, we allow local requests or if the user is authenticated as Admin.
        // Note: MapHangfireDashboard usually runs on the same pipeline, 
        // so if you use Cookie Auth it works. If using only Bearer tokens, 
        // the browser won't send the token automatically for the Hangfire UI.
        
        // Strict approach:
        return httpContext.User.Identity?.IsAuthenticated == true && 
               httpContext.User.IsInRole("Admin");

        // allow local for dev ease if preferred:
        // return httpContext.User.Identity?.IsAuthenticated == true && httpContext.User.IsInRole("Admin") || httpContext.Request.Host.Host == "localhost";
    }
}
