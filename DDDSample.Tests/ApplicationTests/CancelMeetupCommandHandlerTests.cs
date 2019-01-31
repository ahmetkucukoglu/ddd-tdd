namespace DDDSample.Tests.ApplicationTests
{
    using DDDSample.Application;
    using DDDSample.Application.Infrastructure;
    using DDDSample.Application.Meetup.Commands.CancelMeetup;
    using DDDSample.Domain.MeetupAggregate;
    using DDDSample.Domain.MeetupAggregate.Policies;
    using DDDSample.Infrastructure.MeetupDomain;
    using Moq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    [TestCaseOrderer("DDDSample.Tests.PriorityOrderer", "DDDSample.Tests")]
    public class CancelMeetupCommandHandlerTests : IClassFixture<MeetupCommandHandlerTestsFixture>
    {
        private readonly MeetupCommandHandlerTestsFixture _fixture;

        public CancelMeetupCommandHandlerTests(MeetupCommandHandlerTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, TestPriority(1)]
        public async Task Should_Succes_When_CancelingMeetup()
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

            var cancelMeetupCommandHandler = new CancelMeetupCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var cancelMeetupCommand = new CancelMeetupCommand
            {
                MeetupId = meetup.Id
            };

            await cancelMeetupCommandHandler.Handle(cancelMeetupCommand, cancellationToken);

            var result = await _fixture.dbContext.Meetups.FindAsync(meetup.Id);

            Assert.True(result.Cancelled);
        }

        [Fact, TestPriority(2)]
        public async Task Should_ThrowException_When_CancelingMeetup_ToCompletedMeetup()
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

            meetup.Complete();

            var meetupRepository = new MeetupRepository(_fixture.dbContext);

            var cancelMeetupCommandHandler = new CancelMeetupCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var cancelMeetupCommand = new CancelMeetupCommand
            {
                MeetupId = meetup.Id
            };
            
            var exception = await Assert.ThrowsAsync<MeetupDomainException>(async () =>
            {
                await cancelMeetupCommandHandler.Handle(cancelMeetupCommand, cancellationToken);
            });

            Assert.Equal("Completed meetup cannot be cancel", exception.Message);
        }

        [Fact, TestPriority(3)]
        public async Task Should_ThrowException_When_CancelingMeetup_ToNotFoundMeetup()
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

            meetup.Complete();

            var meetupRepository = new MeetupRepository(_fixture.dbContext);

            var cancelMeetupCommandHandler = new CancelMeetupCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var cancelMeetupCommand = new CancelMeetupCommand
            {
                MeetupId = meetup.Id
            };

            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await cancelMeetupCommandHandler.Handle(cancelMeetupCommand, cancellationToken);
            });

            Assert.Equal("Meetup not found", exception.Message);
        }
    }
}
