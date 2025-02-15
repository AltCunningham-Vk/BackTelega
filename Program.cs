using BackTelega.Hubs;
using System.Text;
using BackTelega.Data;
using BackTelega.Repositories;
using BackTelega.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace BackTelega
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var redisHost = builder.Configuration["Redis__Host"] ?? "redis";
            var redisPort = builder.Configuration["Redis__Port"] ?? "6379";
            var redisConnectionString = $"{redisHost}:{redisPort},abortConnect=false,connectRetry=5";
          
            // Добавляем контекст БД с получением строки подключения из `appsettings.json`
            builder.Services.AddDbContext<ClontelegramContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Регистрация репозиториев
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            // Регистрация сервисов
            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddSingleton<RedisCacheService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            // Добавляем контроллеры
            builder.Services.AddControllers();
            // Добавляем сервисы SignalR
            builder.Services.AddSignalR();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Введите токен в формате: Bearer {your JWT token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5100") // Указываем конкретные разрешенные адреса
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials(); // Разрешаем отправку с куками и авторизацией
                    });
            });
            

            var app = builder.Build();
            app.UseRouting();
            app.UseCors("AllowSpecificOrigins"); // Включаем CORS
            app.UseAuthentication();
            app.UseAuthorization();
          
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
              c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackTelega API v1");
              c.RoutePrefix = string.Empty; // Открывать Swagger по корневому URL
            });
            
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
                RequestPath = "/uploads"
            });
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
        
            app.UseRouting();
            app.UseHttpsRedirection();
            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub"); // Добавляем хаб SignalR
            app.Run();
        }
    }
}
