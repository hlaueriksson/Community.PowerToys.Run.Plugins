using System.Text.Json;
using Community.PowerToys.Run.Plugin.DenCode.Models;
using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.DenCode.UnitTests
{
    [TestClass]
    public class GenerateTests
    {
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        [TestMethod]
        [Ignore("Explicit")]
        public async Task Generate_DenCodeMethods()
        {
            var result = new Dictionary<string, DenCodeMethod>();

            var client = new HttpClient();
            var response = await client.GetAsync($"https://raw.githubusercontent.com/mozq/dencode-web/master/src/main/resources/messages.properties");
            var messages = await response.Content.ReadAsStringAsync();
            var lines = messages.Split('\n').ToList();

            foreach (var line in lines.Where(x => x.Contains(".method=")))
            {
                var lastDotIndex = line.LastIndexOf('.');
                var method = line.Substring(0, lastDotIndex);

                result[method] = new DenCodeMethod { Key = method };
            }

            foreach (var method in result.Values)
            {
                foreach (var line in lines.Where(x => x.StartsWith(method.Key)))
                {
                    var lastDotIndex = method.Key.Length;
                    var equalIndex = line.IndexOf('=');
                    var property = line.Substring(lastDotIndex + 1, equalIndex - lastDotIndex - 1);
                    var value = line.Substring(equalIndex + 1);

                    switch (property)
                    {
                        case "method":
                            method.Method = value;
                            break;
                        case "title":
                            method.Title = value;
                            break;
                        case "desc":
                            method.Description = value;
                            break;
                        case "tooltip":
                            method.Tooltip = value;
                            break;
                    }

                    if (property.StartsWith("func."))
                    {
                        method.Label[property.Substring("func.".Length)] = value;
                    }
                }
            }

            var json = JsonSerializer.Serialize(result, _options);

            var path = Directory.GetCurrentDirectory() + @"..\..\..\..\..\..\Community.PowerToys.Run.Plugin.DenCode\Models\Constants.g.Methods.cs";
            var contents = File.ReadAllLines(path).ToList();
            var index = contents.IndexOf("\"\"\"") + 1;
            var count = contents.IndexOf("\"\"\";") - index;
            contents.RemoveRange(index, count);
            contents.InsertRange(index, json.Split(Environment.NewLine));
            File.WriteAllLines(path, contents);
        }

        [TestMethod]
        public void Check_DenCodeMethods()
        {
            var result = Constants.Methods.GetDenCodeMethods();

            Console.WriteLine("# Methods");
            foreach (var method in result.Values)
            {
                if (method.Method == null)
                {
                    Console.WriteLine(method.Key + ":");
                    Console.WriteLine("\tnull");
                }
            }

            Console.WriteLine("# Labels");
            foreach (var method in result.Values)
            {
                var labels = method.Label.Keys.Where(x => !x.StartsWith("enc") && !x.StartsWith("dec"));

                if (labels.Any())
                {
                    Console.WriteLine(method.Key + ":");
                    foreach (var label in labels)
                    {
                        Console.WriteLine("\t" + label);
                    }
                }

                if (method.Label.Count == 0)
                {
                    Console.WriteLine(method.Key + ":");
                    Console.WriteLine("\tempty");
                }
            }
        }
    }
}
