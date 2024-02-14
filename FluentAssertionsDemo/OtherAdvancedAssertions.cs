// using FluentAssertions;
// namespace FluentAssertionsDemo;

// public class OtherAdvancedAssertions
// {
//   public class Person 
//   {
//     public string Name { get; set; }
//     public int Age { get; set; }  
//     public string Email { get; set; }
//   }

//   [Fact]
//   public void ObjectAssertionsTest()
//   {
//     var person1 = new Person 
//     {
//       Name = "John",
//       Age = 30,
//       Email = "john@example.com"
//     };

//     var person2 = new Person 
//     {
//       Name = "John",
//       Age = 30,
//       Email = "john@example.com"
//     };

//     person1.Should().BeEquivalentTo(person2);
//     person1.Should().NotBeNull();
//   }

//   [Fact]
//   public void PersonShouldHaveCorrectProperties()
//   {
//     var person1 = new Person 
//     {
//       Name = "John",
//       Age = 30,
//       Email = "john@example.com"
//     };

//     person1.Should().Match<Person>(p => 
//     p.Name == "John" &&
//     p.Age == 30 &&
//     p.Email == "john@example.com");
//   }

//   [Fact]
//   public void NumericAssertionsTest()
//   {
//     var num = 10;
//     num.Should().BePositive();
//     num.Should().BeInRange(0, 20);
//     num.Should().BeGreaterThan(5);
//   }

//   [Fact]
//   public void StringAssertionsTest()
//   {
//     var text = "Hello World";

//     text.Should().Contain("World");
//     text.Should().StartWith("Hello");
//     text.Should().EndWith("ld");
//   }
// }