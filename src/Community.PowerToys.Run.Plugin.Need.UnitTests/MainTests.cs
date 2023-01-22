using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Need.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var settings = new NeedSettings
            {
                Data = new Dictionary<string, Record>
                {
                    { "foo", new Record { Key = "foo", Value = "bar" } },
                    { "baz", new Record { Key = "baz", Value = "qux" } },
                }
            };
            subject = new Main(settings);
        }

        [TestMethod]
        public void Query_without_args_should_return_all_records()
        {
            subject.Query(new(""))
                .Should().BeEquivalentTo(new[]
                {
                    new Result { Title = "foo", SubTitle = "bar" },
                    new Result { Title = "baz", SubTitle = "qux" },
                });
        }

        [TestMethod]
        public void Query_with_one_arg_should_return_matching_records()
        {
            subject.Query(new("foo"))
                .Should().BeEquivalentTo(new[]
                {
                    new Result { Title = "foo", SubTitle = "bar" },
                });
        }

        [TestMethod]
        public void Query_with_two_args_should_return_set_record_result()
        {
            subject.Query(new("fizz buzz"))
                .Should().BeEquivalentTo(new[]
                {
                    new Result { Title = "fizz", SubTitle = "buzz" },
                });
        }

        [TestMethod]
        public void LoadContextMenus_with_no_ContextData_should_return_empty_result()
        {
            subject.LoadContextMenus(new Result())
                .Should().BeEmpty();
        }

        [TestMethod]
        public void LoadContextMenus_with_Record_should_return_menu_with_copy_value_and_delete_key()
        {
            var result = new Result { ContextData = new Record() };
            subject.LoadContextMenus(result)
                .Should().HaveCount(2)
                .And.Contain(x => x.Title == "Copy value (Enter)")
                .And.Contain(x => x.Title == "Delete key (Ctrl+Del)");
        }
    }
}
