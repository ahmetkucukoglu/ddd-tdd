namespace DDDSample.Application.Meetup.Commands.JoinMeetup
{
    using MediatR;

    public class JoinMeetupCommand : IRequest<bool>
    {
        public string MeetupId { get; set; }
    }
}
