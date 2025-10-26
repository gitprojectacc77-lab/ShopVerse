using System.Text.Json;

namespace ShopVerse.Utils
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession s, string key, T value)
            => s.SetString(key, JsonSerializer.Serialize(value));
        public static T? GetObject<T>(this ISession s, string key)
            => s.GetString(key) is { } str ? JsonSerializer.Deserialize<T>(str) : default;
    }
}
