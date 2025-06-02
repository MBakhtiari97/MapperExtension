
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace MapperExtension;

public class Program
{
    private static string GetSecret()
    {
        var userSecretId = typeof(MapperExtension.Program).Assembly
                        .GetCustomAttributes(typeof(UserSecretsIdAttribute), false)
                        .OfType<UserSecretsIdAttribute>()
                        .FirstOrDefault()?.UserSecretsId;
        if (userSecretId == null)
            throw new Exception("Init Application Configuration File Has Not Set Yet!");
        return userSecretId!;
    }

    private static void AddConnectionStrings(WebApplicationBuilder builder)
    {
        var userSecretId = GetSecret();
        builder.Configuration.AddUserSecrets(userSecretId);

        var masterConnectionString = builder.Configuration.GetConnectionString("MasterConnection");

        builder.Services.AddDbContext<MasterDbContext>(options =>
                                                         options.UseSqlServer(masterConnectionString)
                                                         .EnableSensitiveDataLogging());
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        AddConnectionStrings(builder);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
