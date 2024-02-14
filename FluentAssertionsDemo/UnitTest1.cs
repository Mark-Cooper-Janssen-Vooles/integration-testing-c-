using FluentAssertions;
namespace FluentAssertionsDemo;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var x = 10;
        var y = 9;
        x.Should().BeGreaterThan(y);
    }
}