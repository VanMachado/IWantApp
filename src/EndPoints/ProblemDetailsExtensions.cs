using Flunt.Notifications;

namespace EndPoints;

public static class ProblemDetailsExtensions
{
    public static Dictionary<string, string[]> ConvertToProblemDetails(this IReadOnlyCollection<Notification> notifications)
    {
        return notifications
                .GroupBy(X => X.Key)
                .ToDictionary(y => y.Key,
                g => g.Select(z => z.Message)
                .ToArray());
    }
}
