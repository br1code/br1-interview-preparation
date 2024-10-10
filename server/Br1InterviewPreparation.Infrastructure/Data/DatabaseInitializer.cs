using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(nameof(DatabaseInitializer));

        try
        {
            logger.LogInformation("Initializing database.");

            logger.LogInformation("Applying pending migrations, if any. Creating database if does not already exist.");
            await context.Database.MigrateAsync();

            await SeedData(context, logger);

            logger.LogInformation("Database initialized successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private static async Task SeedData(ApplicationDbContext context, ILogger logger)
    {
        if (context.Categories.Any() || context.Questions.Any() || context.Answers.Any())
        {
            logger.LogInformation("Skipping seed data as some data already exists.");
            return;
        }

        // Categories
        logger.LogInformation("Seeding the database with Categories ...");

        var category1 = new Category { Name = "Databases" };
        var category2 = new Category { Name = "Design Patterns" };

        context.Categories.AddRange(category1, category2);
        await context.SaveChangesAsync();

        // Questions
        logger.LogInformation("Seeding the database with Questions ...");

        var question1 = new Question
        {
            Content = "What is an index?",
            Hint = "Trees, trees everywhere.",
            CategoryId = category1.Id
        };
        var question2 = new Question
        {
            Content = "Which design patterns do you use the most?",
            Hint = "Singleton, Factory, Strategy, Mediator, etc.",
            CategoryId = category2.Id
        };

        context.Questions.AddRange(question1, question2);
        await context.SaveChangesAsync();

        // Answers
        logger.LogInformation("Seeding the database with Answers ...");

        var answer1 = new Answer { VideoFilename = "1849385.webm", QuestionId = question1.Id };
        var answer2 = new Answer { VideoFilename = "1937423.mp4", QuestionId = question2.Id };

        context.Answers.AddRange(answer1, answer2);
        await context.SaveChangesAsync();
    }
}
