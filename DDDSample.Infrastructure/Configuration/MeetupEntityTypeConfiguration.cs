namespace DDDSample.Infrastructure.Configuration
{
    using DDDSample.Domain.MeetupAggregate;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MeetupEntityTypeConfiguration : IEntityTypeConfiguration<Meetup>
    {
        public void Configure(EntityTypeBuilder<Meetup> builder)
        {
            builder.OwnsOne((x) => x.Location);
            builder.Ignore((x) => x.DomainEvents);
        }
    }
}
