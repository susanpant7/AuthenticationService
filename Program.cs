using AuthenticationSystem;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// Core Framework Services
services.AddHttpContextAccessor();
services.AddControllers();

// Application Modules
services.AddDBConnection(configuration);
services.RegisterServicesAndRepositories();
services.AddAuth(configuration);

services.ConfigureCors();

var app = builder.Build();

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    // Optional: app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseCustomMiddlewares();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
