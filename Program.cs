using tasks.Services;
using tasks.Interfaces;
using tasks.Middlewares;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
        // This method gets called by the runtime. Use this method to add services to the container.
        // void ConfigureServices(IServiceCollection services)
        // {
            builder.Services
            //auth1
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.TokenValidationParameters = LoginService.GetTokenValidationParameters();
                });
            //auth2
            builder.Services.AddAuthorization(cfg =>
                {
                    cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
                    cfg.AddPolicy("User", policy => policy.RequireClaim("type", "Admin" , "User"));
                    // cfg.AddPolicy("ClearanceLevel1", policy => policy.RequireClaim("ClearanceLevel", "1", "2"));
                    // cfg.AddPolicy("ClearanceLevel2", policy => policy.RequireClaim("ClearanceLevel", "2"));
                });

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "tasks", Version = "v1" });
                //auth3
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                //auth4
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                    new string[] {}
                }
                });
            });
        // }

builder.Services.AddControllers();
builder.Services.AddTask();
builder.Services.AddUser();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.Map("/favicon.ico", (a) =>
    a.Run(async c => await Task.CompletedTask));

app.UseLogMiddleware("file.log");

app.Use(async (context, next) => 
{   
    await next.Invoke();
});


//

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
