using System.Runtime.CompilerServices;

namespace ItConsultations.Utilities.Guards;

public static class Guard
{
    /// <summary>
    /// Checks if object is not null, in other case throws ArgumentNullException
    /// </summary>
    public static T NotNull<T>(T value, [CallerArgumentExpression("value")] string? parameterName = null) where T : class 
        => value ?? throw new ArgumentNullException(parameterName);

    /// <summary>
    /// Checks if value is not null and is not empty for collections, arrays, strings
    /// </summary>
    public static T NotNullOrEmpty<T>(T value, [CallerArgumentExpression("value")] string? parameterName = null)
        => IsNullOrEmpty(value) ? throw new ArgumentException($"{typeof(T).Name} cannot be null or empty", parameterName) : value;

    public static string NotNullOrWhiteSpace(string value, [CallerArgumentExpression("value")] string? parameterName = null)
        => string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("String cannot be null, empty or whitespace", parameterName) : value;

    /// <summary>
    /// Checks if condition executes, in other case throws exception
    /// </summary>
    public static void That(bool condition, string message, [CallerArgumentExpression("condition")] string? parameterName = null)
    {
        if (!condition)
        {
            throw new ArgumentException(message, parameterName);
        }
    }

    /// <summary>
    /// Checks if condition executes and returns value if condition is true
    /// </summary>
    public static T That<T>(T value, Func<T, bool> condition, string message, [CallerArgumentExpression("value")] string? parameterName = null)
        => condition(value) ? value : throw new ArgumentException(message, parameterName);

    private static bool IsNullOrEmpty<T>(T value)
    {
        if (value == null)
        {
            return true;
        }

        return value switch
        {
            string str => string.IsNullOrEmpty(str),
            System.Collections.ICollection collection => collection.Count == 0,
            System.Collections.IEnumerable enumerable => !enumerable.Cast<object>().Any(),
            _ => false
        };
    }
}
