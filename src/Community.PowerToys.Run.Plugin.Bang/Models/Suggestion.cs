namespace Community.PowerToys.Run.Plugin.Bang.Models
{
    /// <summary>
    /// AutoComplete suggestion.
    /// </summary>
    public class Suggestion
    {
        /// <summary>
        /// Bang phrase.
        /// </summary>
        public string Phrase { get; set; }

        /// <summary>
        /// Website.
        /// </summary>
        public string? Snippet { get; set; }
    }
}
