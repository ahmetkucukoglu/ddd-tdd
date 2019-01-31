namespace DDDSample.Application.Meetup.Queries.GetOpenMeetups
{
    using MediatR;
    using System.Collections.Generic;

    public class GetMeetupsQuery : IRequest<IEnumerable<GetMeetupQueryItem>> { }
}
