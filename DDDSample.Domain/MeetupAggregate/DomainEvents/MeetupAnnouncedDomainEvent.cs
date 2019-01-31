namespace DDDSample.Domain.MeetupAggregate.DomainEvents
{
    using MediatR;

    public class MeetupAnnouncedDomainEvent : INotification
    {
        public Meetup Meetup { get; private set; }

        public MeetupAnnouncedDomainEvent(Meetup meetup)
        {
            Meetup = meetup;
        }
    }
}
