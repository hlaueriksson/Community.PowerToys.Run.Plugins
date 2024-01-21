namespace Community.PowerToys.Run.Plugin.Need.Models
{
    /// <summary>
    /// Key/value record.
    /// </summary>
    public class Record
    {
        /// <summary>
        /// The key.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// The value.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// When the record was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// When the record was updated.
        /// </summary>
        public DateTime? Updated { get; set; }
    }
}
