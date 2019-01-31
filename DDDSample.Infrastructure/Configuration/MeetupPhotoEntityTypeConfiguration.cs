namespace DDDSample.Infrastructure.Configuration
{
    using DDDSample.Domain.MeetupAggregate;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MeetupPhotoEntityTypeConfiguration : IEntityTypeConfiguration<MeetupPhoto>
    {
        public void Configure(EntityTypeBuilder<MeetupPhoto> builder)
        {
            builder.Ignore((x) => x.DomainEvents);
        }
    }
}
