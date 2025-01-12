using System.Text.Json;
using Community.PowerToys.Run.Plugin.DenCode.Models;
using FluentAssertions;
using Moq;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.DenCode.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var json = JsonDocument.Parse(
                """
                {
                    "encStrHex": "48656c6c6f2c20776f726c6421",
                    "encStrURLEncoding": "Hello%2C%20world%21",
                    "encStrHTMLEscape": "Hello, world!",
                    "encStrHTMLEscapeFully": "&#x48;&#x65;&#x6c;&#x6c;&#x6f;&comma;&#x20;&#x77;&#x6f;&#x72;&#x6c;&#x64;&excl;",
                    "decStrHTMLEscape": "Hello, world!"
                }
                """
            );

            var mock = new Mock<IDenCodeClient>();
            mock.Setup(x => x.DenCodeAsync("Hello, world!")).ReturnsAsync(AllDenCodeResponse());
            mock.Setup(x => x.DenCodeAsync(It.Is<DenCodeMethod>(x => x.Key == "string.hex"), "Hello, world!")).ReturnsAsync(HexDenCodeResponse());
            mock.Setup(x => x.DenCodeAsync(It.Is<DenCodeMethod>(x => x.Key == "string.all"), "Hello, world!")).ReturnsAsync(HtmlEscapeDenCodeResponse());
            mock.Setup(x => x.DenCodeAsync(It.Is<DenCodeMethod>(x => x.Key == "string.html-escape"), "Hello, world!")).ReturnsAsync(HtmlEscapeDenCodeResponse());

            _subject = new Main(mock.Object);

            DenCodeResponse AllDenCodeResponse() => new()
            {
                response = new Dictionary<string, JsonElement>
                {
                    { "encStrHex", json.RootElement.GetProperty("encStrHex") },
                    { "encStrURLEncoding", json.RootElement.GetProperty("encStrURLEncoding") },
                }
            };

            DenCodeResponse HexDenCodeResponse() => new()
            {
                response = new Dictionary<string, JsonElement>
                {
                    { "encStrHex", json.RootElement.GetProperty("encStrHex") },
                }
            };

            DenCodeResponse HtmlEscapeDenCodeResponse() => new()
            {
                response = new Dictionary<string, JsonElement>
                {
                    { "encStrHTMLEscape", json.RootElement.GetProperty("encStrHTMLEscape") },
                    { "encStrHTMLEscapeFully", json.RootElement.GetProperty("encStrHTMLEscapeFully") },
                    { "decStrHTMLEscape", json.RootElement.GetProperty("decStrHTMLEscape") },
                }
            };
        }

        [TestMethod]
        public void Query_without_delayedExecution_should_return_empty_result()
        {
            _subject.Query(new(""))
                .Should().BeEmpty();

            _subject.Query(new(""), false)
                .Should().BeEmpty();
        }

        [TestMethod]
        public void Query_with_empty_args_should_return_all_DenCodeMethods()
        {
            _subject.Query(new(""), true)
                .Should().HaveCount(68);
        }

        [TestMethod]
        public void Query_with_key_args_should_return_matching_DenCodeMethods()
        {
            _subject.Query(new("hex"), true)
                .Should().HaveCount(2);
        }

        [TestMethod]
        public void Query_with_key_value_args_should_return_matching_DenCodeMethods()
        {
            _subject.Query(new("string.hex Hello, world!"), true)
                .Should().HaveCount(1);
        }

        [TestMethod]
        public void Query_with_value_args_should_return_matching_DenCodeMethods()
        {
            _subject.Query(new("Hello, world!"), true)
                .Should().HaveCount(2);
        }

        [TestMethod]
        public void Query_branch_method_removes_unchanged_results()
        {
            _subject.Query(new("string.all Hello, world!"), true)
                .Should().HaveCount(1);
        }

        [TestMethod]
        public void Query_leaf_method_keeps_unchanged_results()
        {
            _subject.Query(new("string.html-escape Hello, world!"), true)
                .Should().HaveCount(3);
        }

        [TestMethod]
        public void LoadContextMenus_with_no_ContextData_should_return_empty_result()
        {
            _subject.LoadContextMenus(new Result())
                .Should().BeEmpty();
        }

        [TestMethod]
        public void LoadContextMenus_with_DenCodeMethod_should_return_menu_with_open_website()
        {
            var result = new Result { ContextData = new DenCodeMethod() };
            _subject.LoadContextMenus(result)
                .Should().HaveCount(1)
                .And.Contain(x => x.Title == "Open website (Ctrl+Enter)");
        }

        [TestMethod]
        public void LoadContextMenus_with_DenCodeContextData_should_return_menu_with_copy_result_and_open_website()
        {
            var result = new Result { ContextData = new DenCodeContextData() };
            _subject.LoadContextMenus(result)
                .Should().HaveCount(2)
                .And.Contain(x => x.Title == "Copy result (Enter)")
                .And.Contain(x => x.Title == "Open website (Ctrl+Enter)");
        }
    }
}
