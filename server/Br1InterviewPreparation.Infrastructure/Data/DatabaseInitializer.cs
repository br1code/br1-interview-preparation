using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            Console.WriteLine("DatabaseInitializer: Initializing database.");

            Console.WriteLine("DatabaseInitializer: Applying pending migrations, if any. Creating database if does not already exist.");
            await context.Database.MigrateAsync();

            await SeedData(context);

            Console.WriteLine("DatabaseInitializer: Database initialized successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            throw;
        }
    }

    private static async Task SeedData(ApplicationDbContext context)
    {
        if (context.Categories.Any() || context.Questions.Any() || context.Answers.Any())
        {
            Console.WriteLine("DatabaseInitializer: Skipping seed data");
            return;
        }

        // Categories
        Console.WriteLine("DatabaseInitializer: Seeding the database with Categories ...");

        var category1 = new Category { Name = "Databases" };
        var category2 = new Category { Name = "Design Patterns" };

        context.Categories.AddRange(category1, category2);
        await context.SaveChangesAsync();

        // Questions
        Console.WriteLine("DatabaseInitializer: Seeding the database with Questions ...");

        var question1 = new Question
        {
            Content = "What is an index?",
            Hint = "Trees, trees everywhere.",
            Category = category1
        };
        var question2 = new Question
        {
            Content = "Which design patterns do you use the most?",
            Hint = "Singleton, Factory, Strategy, Mediator, etc.",
            Category = category2
        };

        context.Questions.AddRange(question1);
        await context.SaveChangesAsync();


        // Answers
        Console.WriteLine("DatabaseInitializer: Seeding the database with Answers ...");

        var answer1 = new Answer { VideoFilename = "1849385.mp4", Question = question1 };
        var answer2 = new Answer { VideoFilename = "1937423.mp4", Question = question2 };

        context.Answers.AddRange(answer1, answer2);
        await context.SaveChangesAsync();
    }
}
