using AuthenticationSystem;
var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// Core Framework Services
services.AddHttpContextAccessor();
services.AddControllers();

// Application Modules
services.AddDBConnection(configuration);
services.AddServiceConfig();
services.AddAuth(configuration);

var app = builder.Build();

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    // Optional: app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
