﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Br1InterviewPreparation.Infrastructure.Data;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Infrastructure.Repositories;
using Br1InterviewPreparation.Infrastructure.Services;

namespace Br1InterviewPreparation.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null
                );
            });
        });

        // Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();

        // Services
        services.AddSingleton<IVideoStorageService, VideoStorageService>(); // singleton for now ...

        return services;
    }
}
