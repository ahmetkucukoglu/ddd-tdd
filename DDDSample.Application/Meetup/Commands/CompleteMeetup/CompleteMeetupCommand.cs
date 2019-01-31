namespace DDDSample.Application.Meetup.Commands.CompleteMeetup
{
    using MediatR;

    public class CompleteMeetupCommand : IRequest<bool>
    {
        public string MeetupId { get; set; }
    }
}
