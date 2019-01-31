namespace DDDSample.Domain.MeetupAggregate
{
    using System;

    public class MeetupComment : Entity
    {
        public string CommentatorId { get; private set; }
        public string Comment { get; private set; }
        public DateTime CreatedDate { get; private set; }

        protected MeetupComment() { }

        public MeetupComment(string commentatorId, string comment)
        {
            if (string.IsNullOrEmpty(commentatorId))
            {
                throw new MeetupDomainException("Commentator is required");
            }

            if (string.IsNullOrEmpty(comment))
            {
                throw new MeetupDomainException("Comment is required");
            }

            Id = Guid.NewGuid().ToString();
            CommentatorId = commentatorId;
            Comment = comment;
            CreatedDate = DateTime.UtcNow;
        }
    }
}
