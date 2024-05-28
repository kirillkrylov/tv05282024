using System.CommandLine;
using System.Data;
using ConsoleTables;
using Interview.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace Interview;

public static class Program
{

	private const string BaseUrl = "https://api.github.com";
	private static readonly ServiceProvider? ServiceProvider = RegisterServices();

	public static async Task<int> Main(string[] args){
		RootCommand rootCommand = RegisterCommands();
		return await rootCommand.InvokeAsync(args);
	}

	/// <summary>
	/// Basic DI container
	/// </summary>
	/// <returns>Instance of initialized <see cref="Microsoft.Extensions.DependencyInjection.ServiceProvider"/></returns>
	private static ServiceProvider RegisterServices(){
		ServiceCollection services = [];
		services.AddHttpClient("GitHub", client => {
			client.BaseAddress = new Uri(BaseUrl);
			client.DefaultRequestHeaders.Add("User-Agent", "InterviewApp/1.0.0");
		});
		services.AddTransient<IGitHubService, GitHubService>();
		services.AddSingleton<ISqliteService, SqliteService>(s=> new SqliteService());
		return services.BuildServiceProvider();
	}

	/// <summary>
	/// Registers Command to be availbel from Terminal
	/// </summary>
	/// <returns>Instance of <see cref="RootCommand"/> </returns>
	/// <remarks>
	/// See <a href="https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial">Tutorial: Get started with System.CommandLine</a>
	/// </remarks>
	private static RootCommand RegisterCommands(){
		RootCommand rootCommand = new("Sample app Interview app");

		Option<string> emailOption = new("email", "email of a user");
		Command getProfile = new("GetProfile", "Get user profile from GitHub by email") {emailOption};
		getProfile.SetHandler(GetProfileFromEmailAsync, emailOption);
		rootCommand.AddCommand(getProfile);

		Option<string> loginOption = new("login", "login of a user");
		Command getRepos = new("GetRepos", "Get Repositories for a login") {loginOption};
		getRepos.SetHandler(GetReposByLoginAsync, loginOption);
		rootCommand.AddCommand(getRepos);
		
		Option<string> sqlOption = new("sql", "sql statement");
		Command sqlCommand = new("ExecuteSql", "Execute sql statement");
		sqlCommand.SetHandler(ExecuteSqlAsync);
		rootCommand.AddCommand(sqlCommand);
		
		return rootCommand;
	}

	/// <summary>
	/// Asynchronously retrieves repositories associated with a given GitHub user login.
	/// </summary>
	/// <param name="login">The GitHub user login.</param>
	/// <returns>A task that represents the asynchronous operation.
	/// The task result contains the operation status code,
	/// where 0 indicates success and any other value indicates an error.</returns>
	private static async Task<int> GetReposByLoginAsync(string login){
		try {
			if (ServiceProvider is null) {
				Console.WriteLine("Error - Could not create service provider");
				return 1;
			}
			IGitHubService gitHubClient = ServiceProvider.GetService<IGitHubService>()!;
			IEnumerable<RepositoryDto?> repositories = await gitHubClient.GetUserRepositoriesAsync(login);
			List<RepositoryDto?> repos = repositories.ToList();
			if (repos.Count == 0) {
				Console.WriteLine("Repositories not found");
				return 0;
			}
			Console.WriteLine($"User {login} has total of: {repos.Count} repositories");
			repos
				.Select(e=> new{e?.name, e?.html_url,e?.id }).ToList()
				.ForEach(i=> Console.WriteLine($"{i.id}: {i.name}\t{i.html_url}"));
			return 0;
		} catch (Exception e) {
			Console.WriteLine(e.Message);
			return 1;
		}
	}

	/// <summary>
	/// Searches user by email, and if found prints user information
	/// </summary>
	/// <param name="email">Github user email</param>
	/// <returns>result of an operation, to indicate an error return anything other than 0</returns>
	private static async Task<int> GetProfileFromEmailAsync(string email){
		if (ServiceProvider is null) {
			Console.WriteLine("Error - Could not create service provider");
			return 1;
		}
		
		if (string.IsNullOrWhiteSpace(email)) {
			Console.WriteLine("Error - Email must be provided");
			return 0;
		}

		IGitHubService gitHubClient = ServiceProvider.GetService<IGitHubService>()!;
		SerachDto? search = await gitHubClient.SearchUserByEmailAsync(email);

		if (search is null || search.items.Length == 0) {
			Console.WriteLine("Warning - User not found");
			return 0;
		}
		string login = search.items[0].login;
		ProfileDto? profile = await gitHubClient.GetUserProfileByLoginAsync(login);

		if (profile is null) {
			Console.WriteLine("Profile not found");
			return 1;
		}

		Console.WriteLine($"Name: {profile.name}");
		Console.WriteLine($"Company: {profile.company}");
		Console.WriteLine($"Url: {profile.html_url}");
		return 0;
	}

	
	private static async Task<int> ExecuteSqlAsync(){
		ISqliteService? sql = ServiceProvider!.GetService<ISqliteService>();
		if(sql is null) {
			Console.WriteLine("Error - Could not create sql service");
			return 1;
		}
		sql.ConnectionStateChanged += OnConnectionStateChanged;
		const string sqlStatement = """
						SELECT 
							GHR.Id as 'Repo Id', 
							User.Name as 'User Name', 
							User.Email as 'User Email', 
							User.Login, 
							GHR.Name as 'Repo Name', 
							GHR.FullName as 'Repo Full Name' 
						FROM User
							LEFT OUTER JOIN 
								GitHubRepository GHR on User.Login = GHR.Owner
						""";
		DataTable table = await sql.ExecuteSqlAsync(sqlStatement);
		ConsoleTable.From(table).Write();
		sql.ConnectionStateChanged -= OnConnectionStateChanged;
		return 0;
	}
	
	private static void OnConnectionStateChanged(object? sender, StateChangeEventArgs args){
		Console.WriteLine($"Connection state changed from {args.OriginalState} to {args.CurrentState}");
	}
}