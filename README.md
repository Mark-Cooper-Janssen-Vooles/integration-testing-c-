# Integration Testing in C#

- Intro
  - [Unit testing vs Integration Testing](#unit-testing-vs-integration-testing)
  - [Introduction to Integration Testing](#introduction-to-integration-testing)
  - [Benefits of Integration Testing](#benefits-of-integration-testing)
  - Types of Integration Testing
  - Integration Testing with .NET Core
- 


---

### Unit Testing vs Integration Testing
Unit testing:
- Unit testing focuses on testing individual components / units in isolation 
- One piece of the puzzle

Integration Testing:
- Tests the interaction between components, or entire systems
- An example: We want to add a new Todo item through an API
  - PostTodoITemAsync_ShouldCreateNewTodoItem is the test name
    - we're posting to the API and seeing that it has created / returned said Todo

### Introduction to Integration Testing 
- Integration testing is a testing process where different parts of a system are combined and the interaction between them is tested 
- Its an essential process to expose faults or defects that may occur when different components interact 
- i.e. a service is making a call to another service and processing the response 
- Integration tests require more code and processing
  - They are typically slower to run than unit tests
  - they use the actual components the app uses in production, providing a more realistic assessment of the apps performance 

### Benefits of Integration Testing 
1. Detecting Interaction issues
  - allows us to detect issues that occur when different components interact with each other.
2. Facilitating early bug detection
  - saves time and resources down the line
3. Supports CI/CD 
  - a corner stone of CICD. gives more confidence software remains in a releasable state
4. Tests real world scenarios 
  - simulate user interactions and workflows
5. Promotes loose coupling and high cohesion 

### Types of Integration Testing
1. Big bang integration
2. Incremental Testing 
3. Sandwich testing 
4. Continous Integration 

Big bang integration:
- integrating all modules once and testing them
- the most straightforward approach 
- since all modules are bundled together, could lead to more complexity in isolating and fixing issues 

Incremental testing: 
- Integrating and testing modules one by one 
- sub-divided into 'bottom-up' testing or 'top-down' testing 
  - bottom-up starts from the lowest or innermost unit and moves up
  - top-down starts from the top or outermost unit 

Sandwich testing:
- also known as hybrid testing, a combo of bottom-up and top-down approaches 
- leverages advantages of both above approaches 
- requires planning and effort to manage 

Continous Inegration testing:
- most common / modern currently used 
- after merging code changes, automated builds tests are run 
- early detection of integration bugs 

### Integration tests with ASP.NET core 
- application to be tested is known as 'System Under Test' (SUT)
- can be anything such as Razor pages, MVC or web API
- the nuget package `Microsoft.AspNetCore.Mvc.Testing` is essential for integration testing 
  - includes a `WebApplicationsFactory` class
    - `Test Web Host` and a `Test Client` are created from the `WebApplicationsFactory` class
    - the test project refers to the SUT project as its dependency
  - has the following functions: 
    - copies the project dependencies (.deps) of SUT project (to the bin folder of the test project)
    - sets the content root directory of the test project to the root of the SUT project (so static images, files and pages can be viewed as tests are executed)
    - WebApplicationsFactory class bootstraps the testServer with the SUT 
- Steps of integration testing:
  - test project is included in the same solution as the SUT 
  - follows usual arrange, act and assert workflow 
  - in the arrage step, requests are set up `var client = _factory.CreateClient()`
    - _factory is an instance of `WebApplicationFactory<TEntry>` where generic TEntry is the entry point to the app which is the program class. the _factory object is injected 
  - in the act step, the client submits the request and receives a response, e.g. `var response = await client.getAsync(url);` 
  - the assert steps are used to validate the response received against the expected response as pass or fail test, e.g. `response.EnsureSuccessStatusCode();`, `Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());`
- It is good practice to use separate test projects for unit tests and integration tests - do not mix them in the same projects
  - so they can be run independently of each other 
  - there is a different naming convention for integration tests 