using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Community.PowerToys.Run.Plugin.Need.Models;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Need
{
    /// <summary>
    /// File storage.
    /// </summary>
    public interface INeedStorage
    {
        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>The records.</returns>
        IReadOnlyCollection<Record> GetRecords();

        /// <summary>
        /// Gets matching records.
        /// </summary>
        /// <param name="query">A key/value query.</param>
        /// <returns>The records.</returns>
        IReadOnlyCollection<Record> GetRecords(string query);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The record.</returns>
        Record? GetRecord(string key);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void SetRecord(string key, string value);

        /// <summary>
        /// Removes the record.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The status.</returns>
        bool RemoveRecord(string key);

        /// <summary>
        /// Loads file.
        /// </summary>
        void Load();

        /// <summary>
        /// Saves file.
        /// </summary>
        void Save();
    }

    /// <inheritdoc/>
    public class NeedStorage : INeedStorage
    {
        /// <summary>
        /// Default file name.
        /// </summary>
        public const string DefaultFileName = "NeedStorage.json";

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="NeedStorage"/> class.
        /// </summary>
        /// <param name="settings">Plugin settings.</param>
        public NeedStorage(NeedSettings settings)
        {
            Settings = settings;

            Load();
        }

        private NeedSettings Settings { get; }

        private Dictionary<string, Record> Data { get; set; } = [];

        /// <inheritdoc/>
        public IReadOnlyCollection<Record> GetRecords() => Data.Values.ToList().AsReadOnly();

        /// <inheritdoc/>
        public IReadOnlyCollection<Record> GetRecords(string query) => Data.Values.Where(x =>
            x.Key.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
            x.Value.Contains(query, StringComparison.InvariantCultureIgnoreCase))
            .ToList().AsReadOnly();

        /// <inheritdoc/>
        public Record? GetRecord(string key) => Data.GetValueOrDefault(key);

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool RemoveRecord(string key) => Data.Remove(key);

        /// <inheritdoc/>
        public void Load()
        {
            var path = GetPath();

            if (!File.Exists(path))
            {
                File.WriteAllText(path, "{}");
            }

            try
            {
                var json = File.ReadAllText(path);
                Data = JsonSerializer.Deserialize<Dictionary<string, Record>>(json, _serializerOptions) ?? [];
            }
            catch (Exception ex)
            {
                Log.Exception("Load failed: " + path, ex, GetType());
            }
        }

        /// <inheritdoc/>
        public void Save()
        {
            var path = GetPath();

            try
            {
                var json = JsonSerializer.Serialize(Data, _serializerOptions);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Log.Exception("Save failed: " + path, ex, GetType());
            }
        }

        private string GetPath() => Path.Combine(Settings.StorageDirectoryPath, Settings.StorageFileName);
    }
}
