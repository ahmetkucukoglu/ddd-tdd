namespace DDDSample.Tests.EndpointTests
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    [TestCaseOrderer("DDDSample.Tests.PriorityOrderer", "DDDSample.Tests")]
    public class MeetupEndpointTests : IClassFixture<MeetupEndpointTestsFixture>
    {
        private readonly MeetupEndpointTestsFixture _fixture;

        public MeetupEndpointTests(MeetupEndpointTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, TestPriority(1)]
        public async Task Should_Success_PostMeetup()
        {
            _fixture._client.DefaultRequestHeaders.Clear();
            _fixture._client.DefaultRequestHeaders.Add("X-User-Id", "123");

            var body = new
            {
                Subject = "DDD",
                Description = "DDD Practices",
                When = DateTime.Now.AddDays(5),
                Address = "YTÜ Teknopark"
            };

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _fixture._client.PostAsync("/api/meetups", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            _fixture.CreatedMeetupId = responseContent;
        }

        [Fact, TestPriority(2)]
        public async Task Should_ThrowException_When_PostMeetup_If_HasMeetupInToday()
        {
            var body = new
            {
                Subject = "DDD",
                Description = "DDD Practices",
                When = DateTime.Now.AddDays(5),
                Address = "YTÜ Teknopark"
            };

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            _fixture._client.DefaultRequestHeaders.Clear();
            _fixture._client.DefaultRequestHeaders.Add("X-User-Id", "123");

            var response = await _fixture._client.PostAsync("/api/meetups", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(responseContent, new ExpandoObjectConverter());

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("A maximum of one meetup is defined", result.Message);
        }

        [Fact, TestPriority(3)]
        public async Task Should_Success_GetMeetups()
        {
            var response = await _fixture._client.GetAsync("/api/meetups");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<List<ExpandoObject>>(responseContent, new ExpandoObjectConverter());

            Assert.Single(result);
        }

        [Fact, TestPriority(4)]
        public async Task Should_Success_PatchCancelMeetup()
        {
            var response = await _fixture._client.PatchAsync($"/api/meetups/{_fixture.CreatedMeetupId}/cancel", null);

            response.EnsureSuccessStatusCode();
        }

        [Fact, TestPriority(5)]
        public async Task Should_Success_PostMeetup1()
        {
            await Should_Success_PostMeetup();
        }

        [Fact, TestPriority(6)]
        public async Task Should_Success_PutJoinMeetup()
        {
            _fixture._client.DefaultRequestHeaders.Clear();
            _fixture._client.DefaultRequestHeaders.Add("X-User-Id", "456");

            var response = await _fixture._client.PutAsync($"/api/meetups/{_fixture.CreatedMeetupId}/join", null);

            response.EnsureSuccessStatusCode();
        }

        [Fact, TestPriority(7)]
        public async Task Should_Success_PatchCompleteMeetup()
        {
            _fixture._client.DefaultRequestHeaders.Clear();
            _fixture._client.DefaultRequestHeaders.Add("X-User-Id", "123");

            var response = await _fixture._client.PatchAsync($"/api/meetups/{_fixture.CreatedMeetupId}/complete", null);

            response.EnsureSuccessStatusCode();
        }

        [Fact, TestPriority(8)]
        public async Task Should_ThrowException_When_PatchCancelMeetup_ToCompletedMeetup()
        {
            var response = await _fixture._client.PatchAsync($"/api/meetups/{_fixture.CreatedMeetupId}/cancel", null);

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(responseContent, new ExpandoObjectConverter());

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("Completed meetup cannot be cancel", result.Message);
        }

        [Fact, TestPriority(9)]
        public async Task Should_ThrowException_When_PostComment_ToMeetup_If_ParticipantIsNot()
        {
            _fixture._client.DefaultRequestHeaders.Clear();
            _fixture._client.DefaultRequestHeaders.Add("X-User-Id", "789");

            var body = new
            {
                Comment = "Good!"
            };
        
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _fixture._client.PostAsync($"/api/meetups/{_fixture.CreatedMeetupId}/comments", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(responseContent, new ExpandoObjectConverter());

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("You are not a participant", result.Message);
        }

        [Fact, TestPriority(10)]
        public async Task Should_Success_PostComment_ToMeetup()
        {
            _fixture._client.DefaultRequestHeaders.Clear();
            _fixture._client.DefaultRequestHeaders.Add("X-User-Id", "456");

            var body = new
            {
                Comment = "Good!"
            };
        
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _fixture._client.PostAsync($"/api/meetups/{_fixture.CreatedMeetupId}/comments", content);

            response.EnsureSuccessStatusCode();
        }
    }
}
