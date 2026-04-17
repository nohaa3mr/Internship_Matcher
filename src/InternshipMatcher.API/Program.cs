using InternshipMatcher.API.Helpers;
using InternshipMatcher.API.Middlewares;
using InternshipMatcher.Application.Interfaces;
using Prometheus;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterDbConnection(builder.Configuration);
builder.Services.AddControllers();
builder.Services.SwaggerRegisteration();
builder.Services.AddAuthAndAuthorizationWithJWT(builder.Configuration);
builder.Services.AddResponseCompressionEnc();
builder.Services.AddRequestErrorDetails();
builder.Services.AddPipelineBehaviour();
builder.Services.MediateR();
builder.Services.AddCORS();
builder.Host.Serilog(builder.Configuration);
Serilog.Debugging.SelfLog.Enable(msg =>
{
    Console.WriteLine(msg);
});
builder.Services.AddRateLimit();
builder.Services.AddHttpClient<IAIService, AIService>(); 
Log.Information("TEST LOG - should create table");
Log.Error("TEST ERROR LOG");
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
app.UseRouting();

// 5. CORS
app.UseCors();

// 6. Auth
app.UseAuthentication();
app.UseAuthorization();

// 7. Rate Limiting
app.UseRateLimiter();

// 8. Endpoints
app.MapControllers();
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
app.Run();
