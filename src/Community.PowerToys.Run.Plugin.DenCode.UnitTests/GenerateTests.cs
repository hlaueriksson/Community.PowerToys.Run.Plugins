using System.Text.Json;
using Community.PowerToys.Run.Plugin.DenCode.Models;
using FluentAssertions;

namespace Community.PowerToys.Run.Plugin.DenCode.UnitTests
{
    [TestClass]
    public class GenerateTests
    {
        [TestMethod]
        [Ignore("Explicit")]
        public async Task Generate_DenCodeMethods()
        {
            var result = new Dictionary<string, DenCodeMethod>();

            // Last commit before: Move dencoder definitions from config file to Dencoder annotation
            const string commit = "bddb750a7f7812bb1d7aa34ababeabb2275eb97f";
            // TODO: read annotations from https://github.com/mozq/dencode-web/tree/master/src/main/java/com/dencode/logic/dencoder

            var client = new HttpClient();
            var response = await client.GetAsync($"https://raw.githubusercontent.com/mozq/dencode-web/{commit}/src/main/resources/config.properties");
            var config = await response.Content.ReadAsStringAsync();

            foreach (var line in config.Split('\n'))
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("locales")) continue;

                var lastDotIndex = line.LastIndexOf('.');
                var equalIndex = line.IndexOf('=');

                var method = line.Substring(0, lastDotIndex);
                var property = line.Substring(lastDotIndex + 1, equalIndex - lastDotIndex - 1);
                var value = Convert.ToBoolean(line.Substring(equalIndex + 1));

                if (!result.ContainsKey(method))
                {
                    result[method] = new DenCodeMethod { Key = method };
                }

                switch (property)
                {
                    case "useOe":
                        result[method].useOe = value;
                        break;
                    case "useNl":
                        result[method].useNl = value;
                        break;
                    case "useTz":
                        result[method].useTz = value;
                        break;
                    case "hasEncoded":
                        result[method].hasEncoded = value;
                        break;
                    case "hasDecoded":
                        result[method].hasDecoded = value;
                        break;
                }
            }

            response = await client.GetAsync("https://raw.githubusercontent.com/mozq/dencode-web/master/src/main/resources/messages_en.properties");
            var messages = await response.Content.ReadAsStringAsync();
            var lines = messages.Split('\n').ToList();

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
                            method.method = value;
                            break;
                        case "title":
                            method.title = value;
                            break;
                        case "desc":
                            method.desc = value;
                            break;
                        case "tooltip":
                            method.tooltip = value;
                            break;
                    }
                }

                var index = lines.FindIndex(x => x.StartsWith(method.Key + ".tooltip"));

                while (index > 0)
                {
                    var line = lines[++index];

                    if (string.IsNullOrEmpty(line)) break;
                    if (!line.StartsWith("label.")) break;

                    var firstDotIndex = "label.".Length;
                    var equalIndex = line.IndexOf('=');

                    var property = line.Substring(firstDotIndex, equalIndex - firstDotIndex);
                    var value = line.Substring(equalIndex + 1);

                    method.label[property] = value;
                }
            }

            Console.WriteLine(JsonSerializer.Serialize(result));
        }

        [TestMethod]
        public void Check_DenCodeMethods()
        {
            var result = Constants.Methods.GetDenCodeMethods();

            Console.WriteLine("# Methods");
            foreach (var method in result.Values)
            {
                if (method.method == null)
                {
                    Console.WriteLine(method.Key + ":");
                    Console.WriteLine("\tnull");
                }
            }

            Console.WriteLine("# Labels");
            foreach (var method in result.Values)
            {
                var labels = method.label.Keys.Where(x => !x.StartsWith("enc") && !x.StartsWith("dec"));

                if (labels.Any())
                {
                    Console.WriteLine(method.Key + ":");
                    foreach (var label in labels)
                    {
                        Console.WriteLine("\t" + label);
                    }
                }

                if (method.label.Count == 0)
                {
                    Console.WriteLine(method.Key + ":");
                    Console.WriteLine("\tempty");
                }
            }
        }
    }
}
