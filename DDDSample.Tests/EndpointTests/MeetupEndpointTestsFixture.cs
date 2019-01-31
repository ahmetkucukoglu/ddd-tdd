namespace DDDSample.Tests.EndpointTests
{
    using DDDSample.API;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using System.Net.Http;

    public class MeetupEndpointTestsFixture
    {
        private readonly TestServer _testServer;
        public readonly HttpClient _client;

        public string CreatedMeetupId { get; set; }

        public MeetupEndpointTestsFixture()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        
            _client = _testServer.CreateClient();
        }
    }
}
