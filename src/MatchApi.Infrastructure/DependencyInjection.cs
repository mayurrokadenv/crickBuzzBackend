using MatchApi.Application.Common.Interfaces;
using MatchApi.Infrastructure.Persistence;
using MatchApi.Infrastructure.Repositories;
using MatchApi.Infrastructure.Security;
using MatchApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Caching.StackExchangeRedis;


namespace MatchApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Connection string 'SqlServer' is not configured.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
                sql.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName)));
        //services.AddStackExchangeRedisCache(options =>
        //{
        //    options.Configuration = configuration.GetConnectionString("Redis");
        //    options.InstanceName = "MatchApi:";
        //});

        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IFixtureRepository, FixtureRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>(); 
        services.AddScoped<ISportRepository, SportRepository>();
        services.AddScoped<ISportRoleRepository, SportRoleRepository>();
        services.AddScoped<ICommentaryRepository, CommentaryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IScorecardRepository, ScorecardRepository>();

        services.AddHttpClient<ICricApiService, CricApiService>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["CricApi:BaseUrl"]!);
        });

        services.AddHttpClient<ICricbuzzService, CricApiService>(client =>
        {
            client.BaseAddress = new Uri("https://www.cricbuzz.com/");

            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 Chrome/150.0.0.0 Safari/537.36");
        });


        services.AddHttpClient<ICricbuzzCommentaryService, CricbuzzCommentaryService>(
    client =>
    {
        client.BaseAddress = new Uri("https://www.cricbuzz.com/");

        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 Chrome/150.0.0.0 Safari/537.36");
    });
        return services;
    }
}
