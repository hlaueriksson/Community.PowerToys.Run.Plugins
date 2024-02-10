using System.Globalization;

namespace Community.PowerToys.Run.Plugin.Twitch
{
    internal static class Extensions
    {
        public static string Elapsed(this string started)
        {
            if (string.IsNullOrEmpty(started))
            {
                return string.Empty;
            }

            if (DateTime.TryParseExact(started, "yyyy-MM-ddTHH:mm:ssZ", null, DateTimeStyles.AdjustToUniversal, out DateTime result))
            {
                var elapsed = DateTime.UtcNow.Subtract(result);

                return $"{(int)elapsed.TotalHours}:{elapsed.Minutes:00}:{elapsed.Seconds:00}";
            }

            return string.Empty;
        }

        public static string Format(this int viewers)
        {
            return viewers.ToString("#,##0", CultureInfo.InvariantCulture);
        }

        public static string Clean(this string query)
        {
            return query
                .Replace(Command.Previous, string.Empty, StringComparison.Ordinal)
                .Replace(Command.Next, string.Empty, StringComparison.Ordinal)
                .Trim();
        }

        public static string Pipe(this string title, params string[] values)
        {
            return string.Join(" | ", values.Prepend(title).Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
