namespace DDDSample.Domain.MeetupAggregate
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMeetupRepository : IRepository<Meetup>
    {
        Task<Meetup> AddAsync(Meetup meetup);

        Task<Meetup> GetAsync(string id);

        int GetMeetupCountOfDay(string organizerUserId, DateTime date);

        IQueryable<Meetup> GetMeetups(string userId);
    }
}
