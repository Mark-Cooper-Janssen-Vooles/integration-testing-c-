// using FluentAssertions;
// namespace FluentAssertionsDemo;

// public class CollectionAssertion 
// {
//   private readonly IEnumerable<int> myCollection = new List<int> {1, 2, 3, 4};
//   private readonly int myNumber = 4;

//   [Fact]
//   public void ShouldContain()
//   {
//     myCollection.Should().Contain(2);
//   }

//   [Fact]
//   public void ShouldNotContain()
//   {
//     myCollection.Should().NotContain(5);
//   }

//   [Fact]
//   public void ShouldContainSingle()
//   {
//     myCollection.Should().ContainSingle(x => x == 3);
//   }

//   [Fact]
//   public void ShouldHaveCount()
//   {
//     myCollection.Should().HaveCount(4);
//   }

//   [Fact]
//   public void ShouldHaveCountGreaterThan()
//   {
//     myCollection.Should().HaveCountGreaterThan(2);
//   }

//   [Fact]
//   public void ShouldHaveCountLessThan()
//   {
//     myCollection.Should().HaveCountLessThan(10);
//   }

//   [Fact]
//   public void ShouldHaveCountInRange()
//   {
//     myNumber.Should().BeInRange(1, 4);
//   }
// }