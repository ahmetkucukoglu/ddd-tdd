namespace DDDSample.Infrastructure.Configuration
{
    using DDDSample.Domain.MeetupAggregate;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MeetupCommentEntityTypeConfiguration : IEntityTypeConfiguration<MeetupComment>
    {
        public void Configure(EntityTypeBuilder<MeetupComment> builder)
        {
            builder.Ignore((x) => x.DomainEvents);
        }
    }
}
