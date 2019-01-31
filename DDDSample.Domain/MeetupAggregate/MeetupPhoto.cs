namespace DDDSample.Domain.MeetupAggregate
{
    public class MeetupPhoto : Entity
    {
        public string Path { get; private set; }

        protected MeetupPhoto() { }

        public MeetupPhoto(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new MeetupDomainException("Path is required");
            }

            Path = path;
        }
    }
}
