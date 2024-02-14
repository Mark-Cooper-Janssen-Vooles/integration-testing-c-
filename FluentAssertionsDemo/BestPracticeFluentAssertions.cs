using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertionsDemo;

public class TestBestPractices 
{
  public int MyMethod()
  {
    return 42;
  }
  public int MyMethod(string arg)
  {
    if (arg == null)
    {
      throw new ArgumentNullException(nameof(arg));
    }
    return arg.Length;
  }
  public int MyOtherMethod(int x, int y)
  {
    return x + y + 10;
  }
}

public class BestPracticeTests 
{
  private readonly TestBestPractices testBestPractices = new TestBestPractices();

  [Fact]
  public void MyMethod_Should_Return_Expected_Value()
  {
    // Arrange
    var expected = 42; 
    // Act
    var actual = testBestPractices.MyMethod();
    // Assert
    actual.Should().Be(expected);
  }

    [Fact]
  public void MyMethod_Should_Throw_Exception_When_Argument_Null()
  {
    string arg = null;
    Action action = () => testBestPractices.MyMethod(arg);
    action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("arg");
  }

  [Theory]
  [InlineData(0,1, 11)]
  [InlineData(1,0,11)]
  [InlineData(1,1,12)]
  public void My_Other_Method_Should_Return_Correct_Result(int x, int y, int expectedResult)
  {
    var result = testBestPractices.MyOtherMethod(x, y);
    result.Should().Be(expectedResult);
  }
}