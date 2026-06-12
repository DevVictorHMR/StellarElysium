using StellarElysium.Application.Interfaces.Providers;

namespace StellarElysium.Infrastructure.Providers;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
