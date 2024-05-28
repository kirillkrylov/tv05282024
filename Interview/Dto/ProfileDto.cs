namespace Interview.Dto;

public record ProfileDto
{
	public string login { get; init; }
	public int id { get; init; }
	public string node_id { get; init; }
	public string avatar_url { get; init; }
	public string gravatar_id { get; init; }
	public string url { get; init; }
	public string html_url { get; init; }
	public string followers_url { get; init; }
	public string following_url { get; init; }
	public string gists_url { get; init; }
	public string starred_url { get; init; }
	public string subscriptions_url { get; init; }
	public string organizations_url { get; init; }
	public string repos_url { get; init; }
	public string events_url { get; init; }
	public string received_events_url { get; init; }
	public string type { get; init; }
	public bool site_admin { get; init; }
	public string name { get; init; }
	public string company { get; init; }
	public string blog { get; init; }
	public string location { get; init; }
	public object email { get; init; }
	public bool hireable { get; init; }
	public string bio { get; init; }
	public object twitter_username { get; init; }
	public int public_repos { get; init; }
	public int public_gists { get; init; }
	public int followers { get; init; }
	public int following { get; init; }
	public string created_at { get; init; }
	public string updated_at { get; init; }
};