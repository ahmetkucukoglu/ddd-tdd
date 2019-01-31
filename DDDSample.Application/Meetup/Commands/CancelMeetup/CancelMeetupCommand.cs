namespace DDDSample.Application.Meetup.Commands.CancelMeetup
{
    using MediatR;

    public class CancelMeetupCommand : IRequest<bool>
    {
        public string MeetupId { get; set; }
    }
}
