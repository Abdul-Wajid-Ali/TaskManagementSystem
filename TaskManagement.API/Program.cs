using TaskManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register all application services (DbContext, Auth, Repos, etc.)
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// 1. Exception handling first
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // catch unhandled exceptions
    app.MapOpenApi();                // dev-only API docs
}

// 2. Security & static handling
app.UseHttpsRedirection();

// 3. AuthN & AuthZ
app.UseAuthentication();
app.UseAuthorization();

// 4. Controllers / endpoints
app.MapControllers();

await app.RunAsync();