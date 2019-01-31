namespace DDDSample.Tests.ApplicationTests
{
    using DDDSample.Application.Infrastructure;
    using DDDSample.Application.Meetup.Commands.AddComment;
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
    public class AddMeetupCommentCommandHandlerTests : IClassFixture<MeetupCommandHandlerTestsFixture>
    {
        private readonly MeetupCommandHandlerTestsFixture _fixture;

        public AddMeetupCommentCommandHandlerTests(MeetupCommandHandlerTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, TestPriority(1)]
        public async Task Should_Sucess_When_AddingComment()
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

            meetup.Join("456");
            meetup.Complete();

            _fixture.dbContext.Add(meetup);
        
            var meetupRepository = new MeetupRepository(_fixture.dbContext);
            
            var addMeetupCommentCommandHandler = new AddCommentCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var addMeetupCommentCommand = new AddCommentCommand
            {
                MeetupId = meetup.Id,
                Comment = "Good!"
            };

            await addMeetupCommentCommandHandler.Handle(addMeetupCommentCommand, cancellationToken);

            Assert.Single(_fixture.dbContext.Meetups.FirstOrDefault().Comments);
        }

        [Fact, TestPriority(2)]
        public async Task Should_ThrowException_When_AddingComment_ToNotCompletedMeetup()
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

            var addMeetupCommentCommandHandler = new AddCommentCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var addMeetupCommentCommand = new AddCommentCommand
            {
                MeetupId = meetup.Id,
                Comment = "Good!"
            };

            var exception = await Assert.ThrowsAsync<MeetupDomainException>(async () =>
            {
                await addMeetupCommentCommandHandler.Handle(addMeetupCommentCommand, cancellationToken);
            });

            Assert.Equal("Meetup is not completed", exception.Message);
        }

        [Fact, TestPriority(3)]
        public async Task Should_ThrowException_When_AddingComment_If_ParticipantIsNot()
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

            var addMeetupCommentCommandHandler = new AddCommentCommandHandler(meetupRepository, mockIdentityService.Object);
            var cancellationToken = new CancellationToken();

            var addMeetupCommentCommand = new AddCommentCommand
            {
                MeetupId = meetup.Id,
                Comment = "Good!"
            };

            var exception = await Assert.ThrowsAsync<MeetupDomainException>(async () =>
            {
                await addMeetupCommentCommandHandler.Handle(addMeetupCommentCommand, cancellationToken);
            });

            Assert.Equal("You are not a participant", exception.Message);
        }
    }
}
