namespace DDDSample.Application.Meetup.Commands.AddComment
{
    using DDDSample.Application.Infrastructure;
    using DDDSample.Domain.MeetupAggregate;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, string>
    {
        private readonly IMeetupRepository _meetupRepository;
        private readonly IIdentityService _identityService;

        public AddCommentCommandHandler(
            IMeetupRepository meetupRepository,
            IIdentityService identityService)
        {
            _meetupRepository = meetupRepository;
            _identityService = identityService;
        }

        public async Task<string> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var meetup = await _meetupRepository.GetAsync(request.MeetupId);
            
            if (meetup == null)
            {
                throw new NotFoundException("Meetup not found");
            }

            var commentId = meetup.AddComment(_identityService.GetUserId(), request.Comment);

            await _meetupRepository.UnitOfWork.SaveEntitiesAsync();

            return commentId;
        }
    }
}
