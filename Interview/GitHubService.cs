using System.Net;
using System.Net.Http.Json;
using Interview.Dto;

namespace Interview;


public interface IGitHubService
{
	Task<SerachDto?> SearchUserByEmailAsync(string email);
	Task<ProfileDto?> GetUserProfileByLoginAsync(string login);
	
	Task<IEnumerable<RepositoryDto?>> GetUserRepositoriesAsync(string login);
}

public class GitHubService(IHttpClientFactory factory) : IGitHubService
{

	private readonly HttpClient _client = factory.CreateClient("GitHub");

	public async Task<SerachDto?> SearchUserByEmailAsync(string email){
		
		HttpResponseMessage httpResponseMessage = await _client.GetAsync($"/search/users?q={email}+type:user");
		if(httpResponseMessage.StatusCode != HttpStatusCode.OK) {
			return null;
		}
		SerachDto? result = await httpResponseMessage.Content.ReadFromJsonAsync<SerachDto>();
		return result;
	}
	
	public async Task<ProfileDto?> GetUserProfileByLoginAsync(string login){
		HttpResponseMessage httpResponseMessage = await _client.GetAsync($"/users/{login}");
		if(httpResponseMessage.StatusCode != HttpStatusCode.OK) {
			return null;
		}
		return await httpResponseMessage.Content.ReadFromJsonAsync<ProfileDto>();
	}
	
	
	/* 
	 * TODO: Implement a method that given a user login returns a list of repositories with 
	 * You can get it a list of repositories from https://api.github.com/users/{login}/repos
	 * You can use Interview.RepositoryDto if you wish to do so
	 */
	public async Task<IEnumerable<RepositoryDto?>> GetUserRepositoriesAsync(string login){
		throw new NotImplementedException();
	}
}