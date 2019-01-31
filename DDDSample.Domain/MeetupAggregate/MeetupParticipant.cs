namespace DDDSample.Domain.MeetupAggregate
{
    public class MeetupParticipant
    {
        public string MeetupId { get; private set; }
        public string ParticipantId { get; private set; }

        protected MeetupParticipant() { }

        public MeetupParticipant(string meetupId, string participantId)
        {
            if (string.IsNullOrEmpty(meetupId))
            {
                throw new MeetupDomainException("Meetup is required");
            }

            if (string.IsNullOrEmpty(participantId))
            {
                throw new MeetupDomainException("Participant is required");
            }

            MeetupId = meetupId;
            ParticipantId = participantId;
        }
    }
}
