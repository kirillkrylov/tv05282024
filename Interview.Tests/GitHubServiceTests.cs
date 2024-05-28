using System.Net;
using System.Text.Json;
using FluentAssertions;
using Interview.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace Interview.Tests;

[TestFixture(Category = "UnitTests")]
public class GitHubServiceTests
{

	private IGitHubService _sut;
	private TestHandler _testHandler;
	private const string Email = "someuser@creatio.com";
	private const string Login = "random_login";
	
	[TearDown]
	public void TearDown(){
		_testHandler.MockResponse = null;
		_testHandler.Dispose();
	}
	
	[SetUp]
	public void Setup(){
		ServiceCollection services = [];
		services.AddHttpClient("GitHub", client => {
			client.BaseAddress = new Uri("https://api.github.com");
			client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
			client.DefaultRequestHeaders.Add("User-Agent", "InterviewApp/1.0.0");
		}).AddHttpMessageHandler<TestHandler>();
		services.AddSingleton<TestHandler>();
		services.AddTransient<IGitHubService, GitHubService>();
		ServiceProvider provider = services.BuildServiceProvider();
		_sut = provider.GetService<IGitHubService>()!;
		_testHandler = provider.GetService<TestHandler>()!;
	}

	[Test]
	public async Task SearchUserByEmailAsync_ReturnsNull_When_ResponseNot200(){
		//Arrange
		_testHandler.MockResponse = uri => new HttpResponseMessage() {
			StatusCode = HttpStatusCode.Forbidden
		}; 

		//Act
		SerachDto? result = await _sut.SearchUserByEmailAsync(Email);

		//Assert
		result.Should().BeNull();
	}
	
	[Test]
	public async Task SearchUserByEmailAsync_ReturnsDto_When_Response200(){
		//Arrange
		
		_testHandler.MockResponse = uri => {
			SerachDto searchDto = new() {
				items = [new Item(){login = Login}],
				total_count = 1,
				incomplete_results = false
			};
			return new HttpResponseMessage() {
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(JsonSerializer.Serialize(searchDto))
			};
		}; 

		//Act
		SerachDto? result = await _sut.SearchUserByEmailAsync(Email);
		
		//Assert
		result.Should().NotBeNull();
		result!.items.Should().HaveCount(1);
		result.items[0].login.Should().Be(Login);
	}

	//TODO: Implement tests for GetUserProfileByLoginAsync
	[Test]
	public async Task GetUserRepositoriesAsync_Throws(){
		Func<Task> act = async () => {
			await _sut.GetUserRepositoriesAsync("");
		};
		await act.Should().ThrowAsync<NotImplementedException>();
	}
}