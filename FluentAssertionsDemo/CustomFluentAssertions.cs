// using FluentAssertions;
// using FluentAssertions.Execution;
// using FluentAssertions.Primitives;

// namespace FluentAssertionsDemo;

// public class Person 
// {
//   public string Name { get; set; }
//   public int Age { get; set; }  
//   public string Email { get; set; }
// }

// public class PersonAssertions:ReferenceTypeAssertions<Person, PersonAssertions>
// {
//   public PersonAssertions(Person instance): base(instance)
//   {
    
//   }
//   protected override string Identifier => "Person";
// }

// public static class PersonAssertionsExtensions
// {
//   public static PersonAssertions Should(this Person person)
//   {
//     return new PersonAssertions(person);
//   }

//   public static AndConstraint<PersonAssertions> HaveValidFirstName(this PersonAssertions assertions)
//   {
//     var firstName = assertions.Subject.Name;
//     Execute.Assertion
//       .ForCondition(!string.IsNullOrEmpty(firstName) && firstName.Length >= 2)
//       .FailWith("Expected a valid first name, but found {0}", firstName);

//     return new AndConstraint<PersonAssertions>(assertions);
//   }

//   public static AndConstraint<PersonAssertions> BeAdult(this PersonAssertions assertions)
//   {
//     var age = assertions.Subject.Age;
//     Execute.Assertion
//       .ForCondition(age >= 18)
//       .FailWith("Expected an adult, but found age {0}", age);

//     return new AndConstraint<PersonAssertions>(assertions);
//   }
// }

// public class PersonTests 
// {
//   [Fact]
//   public void Person_Should_Have_Valid_FirstName()
//   {
//     var person = new Person
//     {
//       Name = "John"
//     };

//     person.Should().HaveValidFirstName();
//   }

//   [Fact]
//   public void Person_Should_Be_An_Adult()
//   {
//     var person = new Person
//     {
//       Age = 19
//     };

//     person.Should().BeAdult();
//   }
// }
