namespace Community.PowerToys.Run.Plugin.Dice
{
    /// <summary>
    /// Roll option from the appsettings.json file.
    /// </summary>
    public class RollOption
    {
        /// <summary>
        /// Roll expression.
        /// </summary>
        public string? Expression { get; set; }

        /// <summary>
        /// Roll description.
        /// </summary>
        public string? Description { get; set; }
    }

    /// <summary>
    /// Options from the appsettings.json file.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Roll options.
        /// </summary>
        public IReadOnlyCollection<RollOption>? RollOptions { get; set; }
    }
}
