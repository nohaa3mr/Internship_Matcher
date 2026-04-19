namespace InternshipMatcher.API.MinimalAPIs
{

    public static class EndpointExtensions
    {
        public static WebApplication MapAllEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/v1");

            api.MapGroup("/Recruiters").MapRecruiters();
            api.MapGroup("/internships").MapInternships();

            return app;
        }
    }
}
