namespace DDDSample.Tests.ApplicationTests
{
    using DDDSample.Domain.MeetupAggregate.DomainEvents;
    using DDDSample.Infrastructure;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class MeetupCommandHandlerTestsFixture
    {
        public readonly MeetupDbContext dbContext;

        public bool IsVerifiedMeetupAnnouncedDomainEventHandler { get; set; }

        public MeetupCommandHandlerTestsFixture()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator.Setup((x) => x.Publish<INotification>(It.IsAny<MeetupAnnouncedDomainEvent>(), default(CancellationToken)))
                .Returns<MeetupAnnouncedDomainEvent, CancellationToken>((x, y) =>
                {
                    IsVerifiedMeetupAnnouncedDomainEventHandler = true;

                    return Task.CompletedTask;
                });

            var options = new DbContextOptionsBuilder<MeetupDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
        
            dbContext = new MeetupDbContext(options, mockMediator.Object);
        }
    }
}
