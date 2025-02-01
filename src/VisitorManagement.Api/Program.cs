using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VisitorManagement.Infrastructure.Data;
using VisitorManagement.Infrastructure.Services;
using VisitorManagement.Application.Common.Interfaces;
using VisitorManagement.Api.Middleware;
using VisitorManagement.Api.Services;
using VisitorManagement.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings?.SecretKey ?? throw new InvalidOperationException("JWT SecretKey is not configured."))
        )
    };
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(VisitorManagement.Application.Features.Visitors.Commands.CreateVisitor.CreateVisitorCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(VisitorManagement.Application.Features.Tenants.Commands.CreateTenant.CreateTenantCommand).Assembly);
});

// Configure DbContext
builder.Services.AddDbContext<MasterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MasterDatabase")));

// Register interfaces
builder.Services.AddScoped<IMasterDbContext>(provider => provider.GetRequiredService<MasterDbContext>());
builder.Services.AddScoped<ITenantDatabaseService, TenantDatabaseService>();

// Configure Services
builder.Services.AddScoped<ITenantServiceFactory, TenantServiceFactory>();
builder.Services.AddScoped<ITenantService>(provider => {
    var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;
    var tenantId = httpContext?.Items["TenantId"] as Guid? ?? Guid.Empty;
    var factory = provider.GetRequiredService<ITenantServiceFactory>();
    return factory.Create(tenantId);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseTenantMiddleware();
app.MapControllers();

app.Run();
