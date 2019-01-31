namespace DDDSample.Infrastructure.MeetupDomain
{
    using DDDSample.Domain;
    using DDDSample.Domain.MeetupAggregate;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class MeetupRepository : IMeetupRepository
    {
        private readonly MeetupDbContext _dbContext;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _dbContext;
            }
        }

        public MeetupRepository(MeetupDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Meetup> AddAsync(Meetup meetup)
        {
            await _dbContext.AddAsync(meetup);

            return meetup;
        }

        public async Task<Meetup> GetAsync(string id)
        {
            var meetup = await _dbContext.Meetups.FindAsync(id);

            return meetup;
        }

        public int GetMeetupCountOfDay(string organizerId, DateTime date)
        {
            var meetupCount = _dbContext.Meetups.Where((x) => !x.Cancelled && x.OrganizerId == organizerId && x.When.Date == date.Date).AsQueryable().Count();

            return meetupCount;
        }

        public IQueryable<Meetup> GetMeetups(string organizerId)
        {
            var meetups = _dbContext.Meetups.Where((x) => x.OrganizerId == organizerId);

            return meetups;
        }
    }
}
