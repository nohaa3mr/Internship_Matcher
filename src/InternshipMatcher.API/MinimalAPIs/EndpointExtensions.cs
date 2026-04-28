namespace InternshipMatcher.API.MinimalAPIs
{

    public static class EndpointExtensions
    {
        public static WebApplication MapAllEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api");

            api.MapGroup("/Recruiters").MapRecruiters();
            api.MapGroup("/internships").MapInternships();
            api.MapGroup("/auth").MapUsersEndpoints();

            return app;
        }
    }
}
