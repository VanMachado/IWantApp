using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;

namespace EndPoints;

public static class ProblemDetailsExtensions
{
    public static Dictionary<string, string[]> ConvertToProblemDetails(
        this IReadOnlyCollection<Notification> notifications)
    {
        return notifications
                .GroupBy(X => X.Key)
                .ToDictionary(y => y.Key,
                g => g.Select(z => z.Message)
                .ToArray());
    }

    public static Dictionary<string, string[]> ConvertToProblemDetails(
        this IEnumerable<IdentityError> error)
    {
        var dictionary = new Dictionary<string, string[]>();
        dictionary.Add("Error", error.Select(x => x.Description)
            .ToArray());

        return dictionary;
    }
}
