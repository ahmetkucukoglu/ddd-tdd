namespace DDDSample.Tests.ApplicationTests
{
    using DDDSample.Application;
    using DDDSample.Application.Infrastructure;
    using DDDSample.Application.Meetup.Commands.CompleteMeetup;
    using DDDSample.Application.Meetup.Commands.JoinMeetup;
    using DDDSample.Domain;
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
    public class JoinMeetupCommandHandlerTests : IClassFixture<MeetupCommandHandlerTestsFixture>
    {
        private readonly MeetupCommandHandlerTestsFixture _fixture;

        public JoinMeetupCommandHandlerTests(MeetupCommandHandlerTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, TestPriority(1)]
        public async Task Should_Success_Joining_ToMetup()
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

            var joinMeetupCommandHandler = new JoinMeetupCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var joinMeetupCommand = new JoinMeetupCommand
            {
                MeetupId = meetup.Id
            };

            await joinMeetupCommandHandler.Handle(joinMeetupCommand, cancellationToken);

            Assert.Single(_fixture.dbContext.Meetups.FirstOrDefault().Participants);
        }

        [Fact, TestPriority(2)]
        public async Task Should_ThrowException_When_Joining_ToCompletedMeetup()
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

            meetup.Complete();

            _fixture.dbContext.Add(meetup);

            var meetupRepository = new MeetupRepository(_fixture.dbContext);

            var joinMeetupCommandHandler = new JoinMeetupCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var joinMeetupCommand = new JoinMeetupCommand
            {
                MeetupId = meetup.Id
            };

            var exception = await Assert.ThrowsAsync<MeetupDomainException>(async () =>
            {
                await joinMeetupCommandHandler.Handle(joinMeetupCommand, cancellationToken);
            });

            Assert.Equal("Meetup is completed", exception.Message);
        }

        [Fact, TestPriority(3)]
        public async Task Should_ThrowException_When_Joining_ToCancelledMeetup()
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
        
            meetup.Cancel();

            _fixture.dbContext.Add(meetup);

            var meetupRepository = new MeetupRepository(_fixture.dbContext);

            var joinMeetupCommandHandler = new JoinMeetupCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var joinMeetupCommand = new JoinMeetupCommand
            {
                MeetupId = meetup.Id
            };

            var exception = await Assert.ThrowsAsync<MeetupDomainException>(async () =>
            {
                await joinMeetupCommandHandler.Handle(joinMeetupCommand, cancellationToken);
            });

            Assert.Equal("Meetup is cancelled", exception.Message);
        }
    }
}
