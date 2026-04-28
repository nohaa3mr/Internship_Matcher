using InternshipMatcher.API.Common.ResponseStructure;
using InternshipMatcher.API.Helpers;
using InternshipMatcher.API.Middlewares;
using InternshipMatcher.API.MinimalAPIs;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Infra.Services;
using Prometheus;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterDbConnection(builder.Configuration);
builder.Services.AddControllers();
builder.Services.SwaggerRegistration();
builder.Services.AddAuthAndAuthorizationWithJWT(builder.Configuration);
builder.Services.AddResponseCompressionEnc();
builder.Services.AddRequestErrorDetails();
builder.Services.AddPipelineBehaviour();
builder.Services.MediateR();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddCORS();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BaseEndpointParameters>();
builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
builder.Host.Serilog(builder.Configuration);
Serilog.Debugging.SelfLog.Enable(msg =>
{
    Console.WriteLine(msg);
});
builder.Services.AddRateLimit();
builder.Services.AddHttpClient<IAIService, AIService>(); 
var app = builder.Build();

app.UseHttpMetrics();
app.UseResponseCompression();
// 2. Exception Handling
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseStatusCodePages();
}

// 3. HTTPS
app.UseHttpsRedirection();
// 4. Routing
app.UseRouting();         // 1
app.UseCors(); // 2 ← before auth
app.UseAuthentication(); // 3
app.UseAuthorization();  // 4
app.MapControllers();    // 5
// 7. Rate Limiting
app.UseRateLimiter();
app.MapMetrics();

// 9. Swagger — after routing and endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Alert v1");
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
        options.DocExpansion(DocExpansion.List);
        options.EnableFilter();
        options.EnableDeepLinking();
    });
    app.MapSwagger().AllowAnonymous();
}
app.MapAllEndpoints();
app.Run();
