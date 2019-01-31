namespace DDDSample.Application.Meetup.Commands.AddComment
{
    using MediatR;

    public class AddCommentCommand : IRequest<string>
    {
        public string MeetupId { get; set; }
        public string Comment { get; set; }
    }
}
