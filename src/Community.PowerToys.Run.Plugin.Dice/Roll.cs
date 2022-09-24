namespace Community.PowerToys.Run.Plugin.Dice
{
    /// <summary>
    /// Roll result from the https://rolz.org API.
    /// </summary>
    public class Roll
    {
        /// <summary>
        /// Roll expression.
        /// </summary>
        public string? Input { get; set; }

        /// <summary>
        /// Roll result.
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// Individual roll results.
        /// </summary>
        public string? Details { get; set; }
    }
}
