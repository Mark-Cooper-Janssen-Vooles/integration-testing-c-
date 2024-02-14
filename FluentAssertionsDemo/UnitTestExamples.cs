// using FluentAssertions;
// namespace FluentAssertionsDemo;

// public class UnitTestExamples
// {
//     [Fact]
//     public void TestToCheckOneVariableGreaterThanOther()
//     {
//         var x = 10;
//         var y = 9;
//         x.Should().BeGreaterThan(y);
//     }

//     [Fact]
//     public void TestToCheckOneVariableLessThanOther()
//     {
//         // var x = 5;
//         // var y = 9;
//         // x.Should().BeLessThan(y);

//         // string s = "hello world!";
//         // s.Should().StartWith("hello").And.EndWith("world!");

//         // List<int> numbers = new List<int> {1, 2, 3};
//         // numbers.Should().HaveCount(3).And.Contain(2);

//         // Action action = () => { throw new Exception("test"); };
//         // action.Should().Throw<Exception>().WithMessage("test");

//         int x = 5; 
//         int y = 10;
//         x.Should().BeLessThanOrEqualTo(y);
//     }
// }