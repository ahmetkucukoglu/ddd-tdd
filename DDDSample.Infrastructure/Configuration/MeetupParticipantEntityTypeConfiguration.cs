namespace DDDSample.Infrastructure.Configuration
{
    using DDDSample.Domain.MeetupAggregate;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MeetupParticipantEntityTypeConfiguration : IEntityTypeConfiguration<MeetupParticipant>
    {
        public void Configure(EntityTypeBuilder<MeetupParticipant> builder)
        {
            builder.HasKey((x) => new { x.ParticipantId, x.MeetupId });
        }
    }
}
