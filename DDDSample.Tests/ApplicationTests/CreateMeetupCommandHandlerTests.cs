namespace DDDSample.Tests.ApplicationTests
{
    using DDDSample.Application.Infrastructure;
    using DDDSample.Application.Meetup.Commands.CreateMeetup;
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
    public class CreateMeetupCommandHandlerTests : IClassFixture<MeetupCommandHandlerTestsFixture>
    {
        private readonly MeetupCommandHandlerTestsFixture _fixture;

        public CreateMeetupCommandHandlerTests(MeetupCommandHandlerTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, TestPriority(1)]
        public async Task Should_Succes_When_CreatingMeetup()
        {
            var meetupRepository = new MeetupRepository(_fixture.dbContext);
            var meetupPolicy = new MeetupPolicy(meetupRepository);

            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup((x) => x.GetUserId()).Returns("123");

            var createMeetupCommandHandler = new CreateMeetupCommandHandler(meetupRepository, meetupPolicy, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var createMeetupCommand = new CreateMeetupCommand
            {
                Subject = "DDD",
                Description = "DDD Practices",
                When = DateTime.Now.AddDays(5),
                Address = "YTÜ Teknopark"
            };

            await createMeetupCommandHandler.Handle(createMeetupCommand, cancellationToken);

            Assert.Equal(1, _fixture.dbContext.Meetups.Count());
            Assert.True(_fixture.IsVerifiedMeetupAnnouncedDomainEventHandler);

            _fixture.IsVerifiedMeetupAnnouncedDomainEventHandler = false;
        }

        [Fact, TestPriority(2)]
        public async Task Should_ThrowException_When_CreatingMeetup_If_HasMeetupInToday()
        {
            var meetupRepository = new MeetupRepository(_fixture.dbContext);
            var meetupPolicy = new MeetupPolicy(meetupRepository);

            var mockIdentityService = new Mock<IIdentityService>();
            mockIdentityService.Setup((x) => x.GetUserId()).Returns("123");

            var createMeetupCommandHandler = new CreateMeetupCommandHandler(meetupRepository, meetupPolicy, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var createMeetupCommand = new CreateMeetupCommand
            {
                Subject = "DDD",
                Description = "DDD Practices",
                When = DateTime.Now.AddDays(5),
                Address = "YTÜ Teknopark"
            };

            var exception = await Assert.ThrowsAsync<MeetupDomainException>(async () =>
            {
                await createMeetupCommandHandler.Handle(createMeetupCommand, cancellationToken);
            });

            Assert.Equal("A maximum of one meetup is defined", exception.Message);
        }
    }
}
