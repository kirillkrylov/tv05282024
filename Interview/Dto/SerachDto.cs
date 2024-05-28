namespace Interview.Dto;

public record SerachDto{
	public int total_count { get; init; }
	public bool incomplete_results { get; init; }
	public Item[] items { get; init; }
}

public record Item
{
	public string login { get; init; }
	public int id { get; init; }
	public string nodeId { get; init; }
	public string avatarUrl { get; init; }
	public string gravatarId { get; init; }
	public string url { get; init; }
	public string htmlUrl { get; init; }
	public string followersUrl { get; init; }
	public string followingUrl { get; init; }
	public string gistsUrl { get; init; }
	public string starredUrl { get; init; }
	public string subscriptionsUrl { get; init; }
	public string organizationsUrl { get; init; }
	public string reposUrl { get; init; }
	public string eventsUrl { get; init; }
	public string receivedEventsUrl { get; init; }
	public string type { get; init; }
	public bool siteAdmin { get; init; }
	public double score { get; init; }
};