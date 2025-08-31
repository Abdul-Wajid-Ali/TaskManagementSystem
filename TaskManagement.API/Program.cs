using TaskManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register all application services (DbContext, Auth, Repos, etc.)
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Expose OpenAPI docs in development only
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Configure middleware
app.UseHttpsRedirection();   // Redirect HTTP → HTTPS
app.UseAuthentication();     // Validate JWTs
app.UseAuthorization();      // Enforce [Authorize] policies

// Map controller routes
app.MapControllers();

await app.RunAsync();