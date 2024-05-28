# Interview


## C# - Implement GitHubService.GetUserRepositoriesAsync
Implement GetUserRepositoriesAsync in [GitHubService] that given a user login 
returns a list of repositories for a given user.
You can get it a list of repositories from https://api.github.com/users/{login}/repos
You can use [RepositoryDto] if you wish to do so


## Unit Test
Update GetUserRepositoriesAsync_Throws in [GitHubServiceTests] to reflect new implementation
of [GitHubService].GetUserRepositoriesAsync

## Integration Test
Discuss GetProfile_WritesError_WhenUserNotFoundByEmail in [ProgramTests]
Describe shortcomings of this test and **suggest** improvements if necessary.

## Basic SQL
Update **ExecuteSqlAsync** in [Program] to return a count of repositories for every user 

User table:
    Id (Pk), Name, Email, Login

GitHubRepository Table:
    Id (Pk), GitHubId, Name, FullName, IsPrivate, Owner

You can join on User.Login = GitHubRepository.Owner


| User Name     | User Email            | Count |
|:--------------|:----------------------|:-----:|
| Kirill Krylov | kirill@creatio.com    |   2   |
| Some User     | some.user@creatio.com |   1   | 


## Docker /Kubernetes



<!-- Link -->
[GitHubService]: ./Interview/GitHubService.cs
[RepositoryDto]: ./Interview/Dto/RepositoryDto.cs
[GitHubServiceTests]: ./Interview.Tests/GitHubServiceTests.cs
[ProgramTests]: ./Interview.Tests/ProgramTests.cs
[Program]: ./Interview/Program.cs



