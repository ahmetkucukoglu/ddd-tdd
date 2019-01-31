namespace DDDSample.Application.Meetup.Queries.GetOpenMeetups
{
    using System;

    public class GetMeetupQueryItem
    {
        public string Subject { get; set; }
        public DateTime When { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
