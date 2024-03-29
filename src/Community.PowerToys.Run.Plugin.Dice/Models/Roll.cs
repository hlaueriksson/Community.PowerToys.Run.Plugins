namespace Community.PowerToys.Run.Plugin.Dice.Models
{
    /// <summary>
    /// Roll result from the Rolz API.
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
