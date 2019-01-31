namespace DDDSample.Tests.ApplicationTests
{
    using DDDSample.Application;
    using DDDSample.Application.Infrastructure;
    using DDDSample.Application.Meetup.Commands.CompleteMeetup;
    using DDDSample.Domain.MeetupAggregate;
    using DDDSample.Domain.MeetupAggregate.Policies;
    using DDDSample.Infrastructure.MeetupDomain;
    using Moq;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    [TestCaseOrderer("DDDSample.Tests.PriorityOrderer", "DDDSample.Tests")]
    public class CompleteMeetupCommandHandlerTests : IClassFixture<MeetupCommandHandlerTestsFixture>
    {
        private readonly MeetupCommandHandlerTestsFixture _fixture;

        public CompleteMeetupCommandHandlerTests(MeetupCommandHandlerTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, TestPriority(1)]
        public async Task Should_Success_When_CompletingMeetup()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup((x) => x.GetUserId()).Returns("123");

            var mockMeetupPolicy = new Mock<IMeetupPolicy>();
            mockMeetupPolicy.Setup((x) => x.CheckCanDefineMeetup(It.IsAny<string>(), It.IsAny<DateTime>())).Callback(() => { });

            var meetup = new Meetup(
               organizerId: "123",
               subject: "DDD",
               when: DateTime.Now.AddDays(1),
               description: "DDD Practices",
               location: new Location("YTÜ Teknopark"),
               policy: mockMeetupPolicy.Object);

            _fixture.dbContext.Add(meetup);

            var meetupRepository = new MeetupRepository(_fixture.dbContext);
            
            var completeMeetupCommandHandler = new CompleteMeetupCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var completeMeetupCommand = new CompleteMeetupCommand
            {
                MeetupId = meetup.Id
            };

            await completeMeetupCommandHandler.Handle(completeMeetupCommand, cancellationToken);

            Assert.True(_fixture.dbContext.Meetups.FirstOrDefault().Completed);
        }

        [Fact, TestPriority(2)]
        public async Task Should_ThrowException_When_CompletingMeetup_ToNotFoundMeetup()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup((x) => x.GetUserId()).Returns("456");

            var mockMeetupPolicy = new Mock<IMeetupPolicy>();
            mockMeetupPolicy.Setup((x) => x.CheckCanDefineMeetup(It.IsAny<string>(), It.IsAny<DateTime>())).Callback(() => { });

            var meetup = new Meetup(
               organizerId: "123",
               subject: "DDD",
               when: DateTime.Now.AddDays(1),
               description: "DDD Practices",
               location: new Location("YTÜ Teknopark"),
               policy: mockMeetupPolicy.Object);

            _fixture.dbContext.Add(meetup);

            var meetupRepository = new MeetupRepository(_fixture.dbContext);

            var completeMeetupCommandHandler = new CompleteMeetupCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var completeMeetupCommand = new CompleteMeetupCommand
            {
                MeetupId = meetup.Id
            };

            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await completeMeetupCommandHandler.Handle(completeMeetupCommand, cancellationToken);
            });

            Assert.Equal("Meetup not found", exception.Message);
        }
    }
}
