using Community.PowerToys.Run.Plugin.Need.Models;

namespace Community.PowerToys.Run.Plugin.Need
{
    /// <summary>
    /// Plugin settings.
    /// </summary>
    public class NeedSettings
    {
        /// <summary>
        /// Key-value store.
        /// </summary>
        public Dictionary<string, Record> Data { get; set; } = [];

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>The records.</returns>
        public IReadOnlyCollection<Record> GetRecords() => Data.Values.ToList().AsReadOnly();

        /// <summary>
        /// Gets matching records.
        /// </summary>
        /// <param name="query">A key/value query.</param>
        /// <returns>The records.</returns>
        public IReadOnlyCollection<Record> GetRecords(string query) => Data.Values
            .Where(x => x.Key.Contains(query, StringComparison.InvariantCultureIgnoreCase) || x.Value.Contains(query, StringComparison.InvariantCultureIgnoreCase))
            .ToList().AsReadOnly();

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The record.</returns>
        public Record? GetRecord(string key) => Data.GetValueOrDefault(key);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetRecord(string key, string value)
        {
            if (Data.TryGetValue(key, out Record? record))
            {
                record.Value = value;
                record.Updated = DateTime.UtcNow;
            }
            else
            {
                Data[key] = new Record { Key = key, Value = value, Created = DateTime.UtcNow };
            }
        }

        /// <summary>
        /// Removes the record.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The status.</returns>
        public bool RemoveRecord(string key) => Data.Remove(key);
    }
}
