namespace DDDSample.Domain.MeetupAggregate
{
    public class Location : ValueObject
    {
        public string Address { get; private set; }
        public string Latitude { get; private set; }
        public string Longitude { get; private set; }

        protected Location() { }

        public Location(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new MeetupDomainException("Address is required");
            }
            
            Address = address;
        }

        public void SetCoordinate(string latitude, string longitude)
        {
            if (string.IsNullOrEmpty(latitude))
            {
                throw new MeetupDomainException("Latitude is required");
            }

            if (string.IsNullOrEmpty(longitude))
            {
                throw new MeetupDomainException("Longitude is required");
            }

            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
