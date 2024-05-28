using System.Net;

namespace Interview.Tests;

public class TestHandler : DelegatingHandler
{

	public Func<Uri, HttpResponseMessage>? MockResponse { get; set; }
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken){
		return Task.FromResult(MockResponse is null 
			? new HttpResponseMessage(HttpStatusCode.OK) 
			: MockResponse(request.RequestUri!));
	}

}