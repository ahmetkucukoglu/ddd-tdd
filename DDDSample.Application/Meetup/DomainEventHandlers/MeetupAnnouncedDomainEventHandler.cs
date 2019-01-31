namespace DDDSample.Application.Meetup.DomainEventHandlers
{
    using DDDSample.Domain.MeetupAggregate.DomainEvents;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class MeetupAnnouncedDomainEventHandler : INotificationHandler<MeetupAnnouncedDomainEvent>
    {
        private readonly ILogger<MeetupAnnouncedDomainEventHandler> _logger;

        public MeetupAnnouncedDomainEventHandler(ILogger<MeetupAnnouncedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MeetupAnnouncedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("OK");

            return Task.CompletedTask;
        }
    }
}
