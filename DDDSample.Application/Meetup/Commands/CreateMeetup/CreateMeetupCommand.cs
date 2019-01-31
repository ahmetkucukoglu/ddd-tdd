namespace DDDSample.Application.Meetup.Commands.CreateMeetup
{
    using MediatR;
    using System;

    public class CreateMeetupCommand : IRequest<string>
    {
        public string Subject { get; set; }
        public DateTime When { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
