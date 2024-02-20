using Community.PowerToys.Run.Plugin.Need.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Need.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            var settings = new NeedSettings
            {
            };

            var mock = new Mock<INeedStorage>();
            mock.Setup(x => x.GetRecords()).Returns(
            [
                new Record { Key = "foo", Value = "bar" },
                new Record { Key = "baz", Value = "qux" },
            ]);
            mock.Setup(x => x.GetRecords("foo")).Returns(
            [
                new Record { Key = "foo", Value = "bar" },
            ]);

            _subject = new Main(settings, mock.Object);
        }

        [TestMethod]
        public void Query_without_args_should_return_all_records()
        {
            _subject.Query(new(""))
                .Should().BeEquivalentTo(new[]
                {
                    new Result { Title = "foo", SubTitle = "bar", IcoPath = @"Images\need.png" },
                    new Result { Title = "baz", SubTitle = "qux", IcoPath = @"Images\need.png" },
                });
        }

        [TestMethod]
        public void Query_with_one_arg_should_return_matching_records()
        {
            _subject.Query(new("foo"))
                .Should().BeEquivalentTo(new[]
                {
                    new Result { Title = "foo", SubTitle = "bar", IcoPath = @"Images\need.png" },
                });
        }

        [TestMethod]
        public void Query_with_two_args_should_return_set_record_result()
        {
            _subject.Query(new("fizz buzz"))
                .Should().BeEquivalentTo(new[]
                {
                    new Result { Title = "fizz", SubTitle = "buzz", IcoPath = @"Images\need.png" },
                });
        }

        [TestMethod]
        public void LoadContextMenus_with_no_ContextData_should_return_empty_result()
        {
            _subject.LoadContextMenus(new Result())
                .Should().BeEmpty();
        }

        [TestMethod]
        public void LoadContextMenus_with_Record_should_return_menu_with_copy_value_copy_details_and_delete_key()
        {
            var result = new Result { ContextData = new Record() };
            _subject.LoadContextMenus(result)
                .Should().HaveCount(3)
                .And.Contain(x => x.Title == "Copy value (Enter)")
                .And.Contain(x => x.Title == "Copy details (Ctrl+C)")
                .And.Contain(x => x.Title == "Delete key (Ctrl+Del)");
        }

        [TestMethod]
        public void LoadContextMenus_with_string_tuple_should_return_menu_with_add_value()
        {
            var result = new Result { ContextData = ("key", "value") };
            _subject.LoadContextMenus(result)
                .Should().HaveCount(1)
                .And.Contain(x => x.Title == "Add value (Enter)");
        }
    }
}
