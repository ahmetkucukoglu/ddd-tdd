namespace DDDSample.Domain.MeetupAggregate
{
    using DDDSample.Domain.MeetupAggregate.DomainEvents;
    using DDDSample.Domain.MeetupAggregate.Policies;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Meetup : Entity, IAggregateRoot
    {
        public string Subject { get; private set; }
        public DateTime When { get; private set; }
        public string Description { get; private set; }
        public virtual Location Location { get; private set; }
        public string OrganizerId { get; private set; }
        public bool Completed { get; private set; }
        public bool Cancelled { get; private set; }

        private readonly List<MeetupParticipant> _participants = new List<MeetupParticipant>();
        public virtual IReadOnlyCollection<MeetupParticipant> Participants => _participants;

        private readonly List<MeetupPhoto> _photos = new List<MeetupPhoto>();
        public virtual IReadOnlyCollection<MeetupPhoto> Photos => _photos;

        private readonly List<MeetupComment> _comments = new List<MeetupComment>();
        public virtual IReadOnlyCollection<MeetupComment> Comments => _comments;

        protected Meetup()
        {
            //    _participants = new List<string>();
            //    _photos = new List<string>();
        }

        public Meetup(string organizerId, string subject, DateTime when, string description, Location location, IMeetupPolicy policy)
        {
            if (string.IsNullOrEmpty(organizerId))
            {
                throw new MeetupDomainException("Organizer is required");
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new MeetupDomainException("Subject is required");
            }

            if (when == null)
            {
                throw new MeetupDomainException("When is required");
            }

            if (when <= DateTime.Today)
            {
                throw new MeetupDomainException("When must greater than today");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new MeetupDomainException("Description is required");
            }

            if (location == null)
            {
                throw new MeetupDomainException("Location is required");
            }

            policy.CheckCanDefineMeetup(organizerId, when);

            Id = Guid.NewGuid().ToString();
            OrganizerId = organizerId;
            Subject = subject;
            When = when;
            Description = description;
            Location = location;           

            AddDomainEvent(new MeetupAnnouncedDomainEvent(this));
        }

        public void Join(string participantId)
        {
            if (Completed)
            {
                throw new MeetupDomainException("Meetup is completed");
            }

            if (Cancelled)
            {
                throw new MeetupDomainException("Meetup is cancelled");
            }

            if (string.IsNullOrEmpty(participantId))
            {
                throw new MeetupDomainException("Participant is required");
            }

            var participant = new MeetupParticipant(Id, participantId);

            _participants.Add(participant);
        }

        public void Complete()
        {
            if (Cancelled)
            {
                throw new MeetupDomainException("Meetup is cancelled");
            }

            Completed = true;
        }

        public void AddPhoto(string photo)
        {
            if (!Completed)
            {
                throw new MeetupDomainException("Meetup is not completed");
            }

            if (Cancelled)
            {
                throw new MeetupDomainException("Meetup is cancelled");
            }

            var meetupPhoto = new MeetupPhoto(photo);

            _photos.Add(meetupPhoto);
        }

        public void Cancel()
        {
            if (Completed)
            {
                throw new MeetupDomainException("Completed meetup cannot be cancel");
            }

            Cancelled = true;
        }

        public string AddComment(string commentatorId, string comment)
        {
            if (string.IsNullOrEmpty(commentatorId))
            {
                throw new MeetupDomainException("Commentator is required");
            }

            if (!Completed)
            {
                throw new MeetupDomainException("Meetup is not completed");
            }

            var participant = Participants.FirstOrDefault((x) => x.ParticipantId == commentatorId);

            if (participant == null)
            {
                throw new MeetupDomainException("You are not a participant");
            }
        
            var meetupComment = new MeetupComment(commentatorId, comment);

            _comments.Add(meetupComment);

            return meetupComment.Id;
        }
    }
}
