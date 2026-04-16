using Cronos;

namespace Wemcy.RecipeApp.Backend.Services;

public class ShowcaseRefreshService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private static readonly CronExpression Schedule = CronExpression.Daily; // Every day at midnight UTC

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var next = Schedule.GetNextOccurrence(DateTimeOffset.UtcNow, TimeZoneInfo.Utc)!.Value;
            await Task.Delay(next - DateTimeOffset.UtcNow, stoppingToken);

            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IShowcaseRecipeService>();
            await service.UpdateShowcaseRecipesAsync();
        }
    }
}
