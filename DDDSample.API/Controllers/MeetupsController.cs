namespace DDDSample.API.Controllers
{
    using DDDSample.Application.Meetup.Commands.AddComment;
    using DDDSample.Application.Meetup.Commands.CancelMeetup;
    using DDDSample.Application.Meetup.Commands.CompleteMeetup;
    using DDDSample.Application.Meetup.Commands.CreateMeetup;
    using DDDSample.Application.Meetup.Commands.JoinMeetup;
    using DDDSample.Application.Meetup.Queries.GetOpenMeetups;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    public class MeetupsController : Controller
    {
        private readonly IMediator _mediator;

        public MeetupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var meetups = await _mediator.Send(new GetMeetupsQuery(), cancellationToken);

            return Ok(meetups);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateMeetupCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);

            return Ok(id);
        }

        [HttpPatch]
        [Route("{id}/complete")]
        public async Task<IActionResult> Complete(string id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CompleteMeetupCommand { MeetupId = id }, cancellationToken);

            return Ok();
        }

        [HttpPatch]
        [Route("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CancelMeetupCommand { MeetupId = id }, cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route("{id}/join")]
        public async Task<IActionResult> Join(string id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new JoinMeetupCommand { MeetupId = id }, cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/comments")]
        public async Task<IActionResult> AddComment(string id, [FromBody] AddCommentCommand command, CancellationToken cancellationToken)
        {
            command.MeetupId = id;

            var commentId = await _mediator.Send(command, cancellationToken);

            return Ok(commentId);
        }
    }
}