namespace DDDSample.Infrastructure
{
    using DDDSample.Domain;
    using DDDSample.Domain.MeetupAggregate;
    using DDDSample.Infrastructure.Configuration;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class MeetupDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        
        public DbSet<Meetup> Meetups { get; set; }
        public DbSet<MeetupComment> MeetupComments { get; set; }
        public DbSet<MeetupParticipant> MeetupParticipants { get; set; }
        public DbSet<MeetupPhoto> MeetupPhotos { get; set; }

        public MeetupDbContext(DbContextOptions<MeetupDbContext> options, IMediator mediator) : base(options)
        {
            if (mediator == null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            _mediator = mediator;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MeetupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetupCommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetupParticipantEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetupPhotoEntityTypeConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);

            var result = await base.SaveChangesAsync();

            return true;
        }
    }
}
