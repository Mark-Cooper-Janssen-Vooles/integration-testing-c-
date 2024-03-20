# Integration Testing in C#

- Intro
  - [Unit testing vs Integration Testing](#unit-testing-vs-integration-testing)
  - [Introduction to Integration Testing](#introduction-to-integration-testing)
  - [Benefits of Integration Testing](#benefits-of-integration-testing)
  - Types of Integration Testing
  - Integration Testing with .NET Core
- [Creating the System under test (SUT)](#creating-the-system-under-test-sut)
- [Fluent Assertions in Testing](#fluent-assertions-in-testing)
- [Integration testing of DB layer - API Integration testing](#integration-testing-of-database-layer-api-integration-testing)
- [Integration Tests for Data Access Code](#integration-tests-for-data-access-code)
- [Load Testing / Concurrency Testing](#concurrency-testing)
- [CICD using github actions](#github-actions)
- [Containerization with Docker](#containerization-with-docker)

---

Terminal commands for using VSCode: 
- `dotnet new webapi -n TodoApi`
- `cd TodoApi`
- `dotnet add package Microsoft.EntityFrameworkCore --version 7.0.0`
- `dotnet build`
- `dotnet run` - it will tell you what port its on, can see it by visiting: `http://localhost:<port>/swagger/index.html`
- for the test project: 
  - Create a new project `dotnet new xunit -n YourTestProjectName`
  - `dotnet add package FluentAssertions`
  - if required, you can link this to an existing project: `cd YourTestProjectName dotnet add reference ../YourWebApiProjectName/YourWebApiProjectName.csproj`
  - `dotnet test` to run it

---

## Intro

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
  - in the arrange step, requests are set up `var client = _factory.CreateClient()`
    - _factory is an instance of `WebApplicationFactory<TEntry>` where generic TEntry is the entry point to the app which is the program class. the _factory object is injected 
  - in the act step, the client submits the request and receives a response, e.g. `var response = await client.getAsync(url);` 
  - the assert steps are used to validate the response received against the expected response as pass or fail test, e.g. `response.EnsureSuccessStatusCode();`, `Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());`
- It is good practice to use separate test projects for unit tests and integration tests - do not mix them in the same projects
  - so they can be run independently of each other 
  - there is a different naming convention for integration tests 

---

## Creating the System under test (SUT)

- `dotnet new webapi -n TodoApi`
  - comes with Microsoft.AspNetCore.OpenApi and Swashbuckle.AspNetCore
    - OpenApi: provides annotations for using APIs
    - Swashbuckle: Swagger has tools for documenting APIs (can use instead of postman)
- `cd TodoApi`
- `dotnet add package Microsoft.EntityFrameworkCore --version 7.0.0`
- `dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 7.0.0`

- Fill out the project
  - create Models folder to house domain models
    - create the TodoItem.cs in there ,
  - create Data folder
    - add TodoContext.cs 
    - in entityFrameworkCore this represents a 'session' and provides a way for us to access the database 
    - it describes managing the db and the connection
  - Seed the database
    - in the models folder, add a class 'DbInitializer'
    - EnsureCreated() makes sure it exists. if it doesn't then it creates it.
  - update Program.cs
    - because we're using an in-memory db; we need to keep the connection open. usually entityFrameworkCore opens and closes the connection as needed

## Fluent Assertions in Testing
- A .net library that provides a fluent interface for asserting the behaviour of code under test. Reads like natural language
- `actualValue.Should().Be(expectedValue)`

- Create a new project: `dotnet new xunit -n YourTestProjectName`
  - if required, you can link this to an existing project: `cd YourTestProjectName dotnet add reference ../YourWebApiProjectName/YourWebApiProjectName.csproj`
  - run `dotnet test` to run it
- `dotnet add package FluentAssertions`

- work in fluentAssertionsDemo/
  - unitTestExamples.cs
  - OtherAdvancedAssertions.cs
  - CollectionAssertion.cs
  - CustomFluentAssertions.cs
  - BestPracticesFluentAssertions.cs

## Integration Testing of Database Layer (API Integration Testing)

- tests the interaction between the app code and the database - all the way from the API, repositories, and DB.
- helps prevent data corruption and security issues

types of integration tests for the db layer:
- E2E tests: tests whole system from UI to db and back
- API tests: tests the api that the app uses to communicate with the database
- Integration tests for data access code: testing how your repository methods interact with the db context 

setting up the test environment:
- create a test db that is separate from the production db
- seed the test db with test data 
- configure the app to use the test db for testing 

Steps:
- create a new project 'TodoApiDataLayerTesting', add the reference to TodoApi, add it to the solution 
- In the TodoItemsControllerIntegrationTests.cs file, it inherits from `IClassFixture<WebApplicationFactory<Program>>` 
  - need to add `dotnet add package Microsoft.AspNetCore.Mvc.Testing` 
  - Its a class that implements IClassFixture 
- tests found here: TodoApiDataLayerTesting\TodoItemsControllerIntegrationTests.cs

## Integration Tests for Data Access Code 

- This section is about ensuring the correct interaction between database and code. Just from the repositories to the DB.

- Creates ITodoItemRepository.cs class
- Creates MockTodoItemRepository.cs class - essentially mocks the TodoItemRepository (we don't have a repository in our controller, we're just directly calling the in memory db. but if we did it would be like this)

## Concurrency Testing 

- Load testing is a type of performance testing that assesses how a system behaves under a specific load or concurrency. The goal of load testing is to evaluate the system's ability to handle a certain amount of load without degrading performance or causing failures.
- Load refers to the number of concurrent users or transactions that a system is subjected to. It helps identify bottlenecks, uncover issues related to system capacity, and determine the systems behavior under different levels of stress 
- He uses JMeter (binaries download the .zip): https://jmeter.apache.org/download_jmeter.cgi
- Also need java sdk installed, `java --version`

*Example for Edge case GET Request performance with JMeter*
- open JMeter (wherever you extracted it, click into bin file, onto ApacheJmeter.Jar)
- Number of threads is e.g. how many users using it at once 
- Ramp-up period is how long it takes for all of these users to use it at once
- Run the application, `cd TodoApi && dotnet run`, check it at http://localhost:5258/swagger/index.html
- Right click the thread group, add Sampler > Http Request
- add `localhost` to the server name, add whatever port (in my case 5258), set it to GET and put the path in 
- Right click the Http Request, add Listeners > Results Tree
- Click the green arrow button to start running it, it will ask you to save it somewhere

*Add an assertion* 
- right click the thread group, add Assertion > Response Assertion
- name it. for field to test, select 'response only'
- in the 'patterns to test' put '200' (i.e. the response code)
  - if it fails, it will say why only when it fails


- to add an assertion with data, i.e. a POST request, you need to right-click and add > Config Element > Http Header Manager. And then click the plus icon and put name: `content-type` and value: `application/json`


*to simulate high server load*
- Set the thread group to use 100 threads, ramp up time 1 second, and loop count 10
- This causes 500 failures - they seem to increase as it goes on as well. If we look into the dotnet terminal thats running it, we can see:
````
An unhandled exception has occurred while executing the request.
Microsoft.Data.Sqlite.SqliteException (0x80004005): SQLite Error 5: 'database is locked For more information on this error code see https://www.sqlite.org/rescode.html'.
at Microsoft.Data.Sqlite.SqliteException.ThrowExceptionForRC(Int32 rc, sqlite3 db)
````
- the above was using the sqlite localhost db, whereas the instructor shows this vs mysql local db which doesn't have these errors - database selection will affect the performance here. 

---

## CICD using Github Actions and Docker and Azure 

### Github Actions
- go to the actions tab of your repo: https://github.com/Mark-Cooper-Janssen-Vooles/integration-testing-c-/actions/new
- you define your worjflows in YAML files stored in the .github/workflows directory
  - these YAML files follow a structured syntax that specifies the events, jobs, and steps that Github should execute 
- Github's primary usecase is CI.
- Github actions can handle CD as well. We can set up our actions to deploy our application to platforms like Azure Web Apps as well.
- When you go to the 'actions' tab in the github repo, you can search for a .net app and github will commmit that to master
  - just search .net and click 'configure' and it makes a PR with the dotnet.yml 

- understanding the .yml step-by-step: https://github.com/Mark-Cooper-Janssen-Vooles/integration-testing-c-/blob/master/.github/workflows/dotnet.yml
  - Workflow Name is named .NET - the identifier for the workflow displayed in the Actions tab of your Github repository 
  - Trigger Events: workflow configured to trigger on two types of events: push events to the master branch and when pull requests are done to the master branch
  - Execution environment: that it runs on the ubuntu-latest image 
  - Job Definition: within the workflow there is a single job named 'build' - the job is responsible for building and testing the .NET project 
  - Job Steps: each step has a specific purpose 
    - `actions/checkout@v4` uses the github action to clone your repository into the runner's workspace - providing access to your projects source code
    - `actions/setup-dotnet@v4` sets up the .NET runtime environment - it specifies it should use the .net version 8.0.x
    - `restore dependencies` runs `dotnet restore` restoring the projects dependencies based in the .csproj and nuget.config 
    - `Build` runs `dotnet build`, the --no-restore flag indicates that dependency restoration should not be performed again because it was done in the previous step
    - `Test` runs `dotnet test` runs the unit tests using the dotnet test command using the --no-build flag which skips the build step. the --verbosity normal flag sets the verbosity level of the test runner. 

### Containerization with Docker
- benefits of containerization
  - portability: containers run consistently across different environments
  - isolation: applications and dependencies are isolated, reducing conflicts
  - scalability: easy scaling up or down as needed
  - version control: containers can be versioned and tracked
- docker architecture
  - consists of docker engine, which includes the docker daemon and docker client 
  - images areu sed to create containers
  - docker hub is a registry for storing and sharing container images, similar to github for code repositories 
- Visual Studio docker support
  - visual studio makes it very easy to add docker support
  - open the solution in visual studio => right-click 'todoAPI' => new => add docker support => make the docker file Linux
  - it then generates a dockerfile 
- the dockerfile contains instructions on how to build a Docker image for our application. it lives at the root of our project directory. 

- Docker commands:
  - Step 1: building a docker image: `docker build -t mytodo-api:latest .`
    - `docker build` builds an image
    - `-t` specifies the image tag, which is like a version
    - `mytodo-api:latest` is the image name with a tag
    - `.` indicates the current directory where the project is located
  - Step 2: running a docker image: `docker run -d -p 5017:80 --name mytodo-api-container mytodo-api:latest`
    - `docker run` starts a container
    - `-d` runs the container in the background 
    - `-p 5017:80` maps port 5017 on your host to port 80 in the container
    - `--name mytodo-api-container` gives the container a name 
    - `mytodo-api:latest` is the image to use 
  