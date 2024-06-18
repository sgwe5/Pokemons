﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pokemons.API.Handlers;
using Pokemons.API.Middlewares;
using Pokemons.Core.ApiHandlers;
using Pokemons.Core.MapperProfiles;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.Core.Services.BattleService;
using Pokemons.Core.Services.PlayerService;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database;
using Pokemons.DataLayer.Database.Repositories.BattleRepos;
using Pokemons.DataLayer.Database.Repositories.MarketRepos;
using Pokemons.DataLayer.Database.Repositories.PlayerRepos;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;
using Pokemons.DataLayer.MasterRepositories.BattleRepository;
using Pokemons.DataLayer.MasterRepositories.MarketRepository;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;
using StackExchange.Redis;
using TimeProvider = Pokemons.Core.Providers.TimeProvider.TimeProvider;

namespace Pokemons.Core.Extensions;

public static class WebApplicationBuilderExtenstion
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        ConfigureMapper(builder);
        ConfigureServices(builder);
        ConfigurePipeline(builder);
        ConfigureDbContext(builder);
        ConfigureApiHandlers(builder);
        ConfigureRepositories(builder);
        ConfigureTimeProvider(builder);
        ConfigureCacheRepository(builder);
        ConfigureDatabaseRepositories(builder);
    }

    private static void ConfigureMapper(IHostApplicationBuilder builder)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new BattleProfile());
            mc.AddProfile(new MarketProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);
    }

    private static void ConfigureApiHandlers(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthHandler, AuthHandler>();
        builder.Services.AddScoped<IBattleHandler, BattleHandler>();
        builder.Services.AddScoped<IMarketHandler, MarketHandler>();
    }

    private static void ConfigureDatabaseRepositories(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<IPlayerDatabaseRepository, PlayerDatabaseRepository>();
        builder.Services.AddScoped<IBattleDatabaseRepository, BattleDatabaseRepository>();
        builder.Services.AddScoped<IMarketDatabaseRepository, MarketDatabaseRepository>();
    }

    private static void ConfigureRepositories(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
        builder.Services.AddScoped<IBattleRepository, BattleRepository>();
        builder.Services.AddScoped<IMarketRepository, MarketRepository>();
    }

    private static void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IPlayerService, PlayerService>();
        builder.Services.AddTransient<IBattleService, BattleService>();
    }
    
    private static void ConfigureTimeProvider(IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ITimeProvider, TimeProvider>();
    }

    private static void ConfigureCacheRepository(IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? 
                                          throw new ArgumentNullException()));

        builder.Services.AddSingleton<ICacheRepository, CacheRepository>();
    }

    private static void ConfigurePipeline(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddTransient<AuthMiddleware>();
    }
    
    private static void ConfigureDbContext(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContextFactory<AppDbContext>(optionsAction =>
        {
            optionsAction.UseNpgsql(builder.Configuration.GetConnectionString("Database") 
                                    ?? throw new ArgumentNullException());
        });
    }
}