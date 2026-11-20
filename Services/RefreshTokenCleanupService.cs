using BookwormAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BookwormAPI.Services;

public class RefreshTokenCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RefreshTokenCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Run once every 24 hours
        using var timer = new PeriodicTimer(TimeSpan.FromHours(24));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var cutoff = DateTime.UtcNow.AddDays(-7); // Optional grace period for audit logs

            await context.RefreshTokens
                .Where(rt => rt.ExpiresAt < DateTime.UtcNow || (rt.IsRevoked && rt.CreatedAt < cutoff))
                .ExecuteDeleteAsync(stoppingToken);
        }
    }
}
