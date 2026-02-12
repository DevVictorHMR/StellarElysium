using StellarElysium.Application.Interfaces.Providers;

namespace StellarElysium.Infrastructure.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
