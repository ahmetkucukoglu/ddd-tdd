namespace DDDSample.Domain.MeetupAggregate
{
    using System;

    public class MeetupDomainException : DomainException
    {
        public MeetupDomainException(string message) : base(message) { }
    }
}
