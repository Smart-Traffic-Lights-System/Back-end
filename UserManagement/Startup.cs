using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserData;
using UserData.Entities;
using UserManagement.Business.Users;
using UserManagement.Models;

namespace UserManagement;

public class Startup
{
    public IConfiguration configRoot { get; }
    public string MyAllowSpecificOrigins { get; }

    public Startup(IConfiguration configuration)
    {
        configRoot = configuration;
        MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    }

    /*  
     *  Services of User Management API
     */
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();

        /***** [1] Configure ORM *****/
        services.AddDbContext<UserDbContext>();

        /***** [2] Configure Repositories *****/
        services.AddScoped(
            typeof(IAuthenticationService), typeof(AuthenticationService)
        );
        services.AddScoped(
            typeof(IUserService), typeof(UserService)
        );
            

        /***** [3] Configure AutoMapper *****/
        var configuration = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<User, RegisterUserDto>().ReverseMap();
            cfg.CreateMap<UserRole, RoleDto>().ReverseMap();
        });


        /***** [4] Configure CORS *****/
        services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("http://localhost:3000",  // Client URL   
                                       "https://localhost:5297") // Server URL
                                       .AllowAnyHeader()
                                       .AllowAnyMethod();
                });                    
        });

        services.AddHttpContextAccessor();

        /***** [5] Configure Controllers *****/
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        /***** [6] Configure Swagger *****/
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManagement", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        /***** [7] Configure Authentication *****/
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // For development only, set to true in production
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configRoot["JWT:Issuer"],
                    ValidAudience = configRoot["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configRoot["JWT:Key"]))
                };
            });
    }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManagement v1");
                });
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
}