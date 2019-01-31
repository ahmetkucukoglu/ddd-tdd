namespace DDDSample.Tests.DomainTests
{
    using DDDSample.Domain.MeetupAggregate;
    using DDDSample.Domain.MeetupAggregate.Policies;
    using Moq;
    using System;
    using Xunit;

    public class MeetupAggregateTests
    {
        private readonly IMeetupPolicy meetupPolicy;
        private readonly IMeetupPolicy meetupPolicy1;

        public MeetupAggregateTests()
        {
            var mockMeetupPolicy = new Mock<IMeetupPolicy>();
            mockMeetupPolicy.Setup((x) => x.CheckCanDefineMeetup(It.IsAny<string>(), It.IsAny<DateTime>())).Callback(() => { });

            meetupPolicy = mockMeetupPolicy.Object;

            var mockMeetupPolicy1 = new Mock<IMeetupPolicy>();
            mockMeetupPolicy1.Setup((x) => x.CheckCanDefineMeetup(It.IsAny<string>(), It.IsAny<DateTime>())).Callback(() =>
            {
                throw new MeetupDomainException("A maximum of one meetup is defined");
            });

            meetupPolicy1 = mockMeetupPolicy1.Object;
        }
        
        [Fact]
        public void Should_ThrowException_When_CreatingMeetup_If_HasMeetupInToday()
        {
            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                var meetup = new Meetup(
                    organizerId: "123",
                    subject: "DDD",
                    when: DateTime.Now.AddDays(1),
                    description: "DDD Practices",
                    location: new Location("YTÜ Teknopark"),
                    policy: meetupPolicy1);
            });

            Assert.Equal("A maximum of one meetup is defined", exception.Message);
        }

        [Fact]
        public void Should_ThrowException_When_CreatingMeetup_If_ThereIsNoSubject()
        {
            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                var meetup = new Meetup(
                    organizerId: "123",
                    subject: "",
                    when: DateTime.Now.AddDays(1),
                    description: "DDD Practices",
                    location: new Location("YTÜ Teknopark"),
                    policy: meetupPolicy);
            });

            Assert.Equal("Subject is required", exception.Message);
        }

        [Fact]
        public void Should_ThrowException_When_CreatingMeetup_If_ThereIsNoOrganizer()
        {
            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                var meetup = new Meetup(
                    organizerId: "",
                    subject: "DDD",
                    when: DateTime.Now.AddDays(1),
                    description: "DDD Practices",
                    location: new Location("YTÜ Teknopark"),
                    policy: meetupPolicy);
            });

            Assert.Equal("Organizer is required", exception.Message);
        }

        [Fact]
        public void Should_ThrowException_When_CreatingMeetup_If_DateIsLessThanToday()
        {
            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                var meetup = new Meetup(
                    organizerId: "123",
                    subject: "DDD",
                    when: DateTime.Now.AddDays(-1),
                    description: "DDD Practices",
                    location: new Location("YTÜ Teknopark"),
                    policy: meetupPolicy);
            });

            Assert.Equal("When must greater than today", exception.Message);
        }

        [Fact]
        public void Should_ThrowException_When_CreatingMeetup_If_ThereIsNoDescription()
        {
            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                var meetup = new Meetup(
                    organizerId: "123",
                    subject: "DDD",
                    when: DateTime.Now.AddDays(1),
                    description: "",
                    location: new Location("YTÜ Teknopark"),
                    policy: meetupPolicy);
            });

            Assert.Equal("Description is required", exception.Message);
        }

        [Fact]
        public void Should_ThrowException_When_CreatingMeetup_If_ThereIsNoAddress()
        {
            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                var meetup = new Meetup(
                    organizerId: "123",
                    subject: "DDD",
                    when: DateTime.Now.AddDays(1),
                    description: "DDD Practices",
                    location: null,
                    policy: meetupPolicy);
            });

            Assert.Equal("Location is required", exception.Message);
        }

        [Fact]
        public void Should_Succes_When_CreatingMeetup()
        {
            var meetup = new Meetup(
                organizerId: "123",
                subject: "DDD",
                when: DateTime.Now.AddDays(1),
                description: "DDD Practices",
                location: new Location("YTÜ Teknopark"),
                policy: meetupPolicy);

            Assert.NotNull(meetup);
        }

        [Fact]
        public void Should_RaiseDomainEvent_When_MeetupCreated()
        {
            var meetup = new Meetup(
                organizerId: "123",
                subject: "DDD",
                when: DateTime.Now.AddDays(1),
                description: "DDD Practices",
                location: new Location("YTÜ Teknopark"),
                policy: meetupPolicy);

            Assert.Single(meetup.DomainEvents);
        }

        [Fact]
        public void Should_Success_When_CompletingMeetup()
        {
            var meetup = new Meetup(
                organizerId: "123",
                subject: "DDD",
                when: DateTime.Now.AddDays(1),
                description: "DDD Practices",
                location: new Location("YTÜ Teknopark"),
                policy: meetupPolicy);

            meetup.Complete();

            Assert.True(meetup.Completed);
        }

        [Fact]
        public void Should_ThrowException_When_AddingPhoto_ToNotCompletedMeetup()
        {
            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                var meetup = new Meetup(
                    organizerId: "123",
                    subject: "DDD",
                    when: DateTime.Now.AddDays(1),
                    description: "DDD Practices",
                    location: new Location("YTÜ Teknopark"),
                    policy: meetupPolicy);

                meetup.AddPhoto("photo.jpg");
            });

            Assert.Equal("Meetup is not completed", exception.Message);
        }

        [Fact]
        public void Should_Succes_When_AddingPhoto_ToMeetup()
        {
            var meetup = new Meetup(
                organizerId: "123",
                subject: "DDD",
                when: DateTime.Now.AddDays(1),
                description: "DDD Practices",
                location: new Location("YTÜ Teknopark"),
                policy: meetupPolicy);

            meetup.Complete();

            meetup.AddPhoto("photo.jpg");

            Assert.Single(meetup.Photos);
        }

        [Fact]
        public void Should_ThrowException_When_Joining_ToCompletedMeetup()
        {
            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                var meetup = new Meetup(
                    organizerId: "123",
                    subject: "DDD",
                    when: DateTime.Now.AddDays(1),
                    description: "DDD Practices",
                    location: new Location("YTÜ Teknopark"),
                    policy: meetupPolicy);

                meetup.Complete();

                meetup.Join(Guid.NewGuid().ToString());
            });

            Assert.Equal("Meetup is completed", exception.Message);
        }

        [Fact]
        public void Should_Success_Joining_ToMetup()
        {
            var meetup = new Meetup(
                organizerId: "123",
                subject: "DDD",
                when: DateTime.Now.AddDays(1),
                description: "DDD Practices",
                location: new Location("YTÜ Teknopark"),
                policy: meetupPolicy);

            meetup.Join(Guid.NewGuid().ToString());

            Assert.Single(meetup.Participants);
        }

        [Fact]
        public void Should_ThrowException_When_AddingComment_If_ParticipantIsNot()
        {
            var meetup = new Meetup(
                organizerId: "123",
                subject: "DDD",
                when: DateTime.Now.AddDays(1),
                description: "DDD Practices",
                location: new Location("YTÜ Teknopark"),
                policy: meetupPolicy);

            meetup.Complete();

            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                meetup.AddComment("456", "Good!");
            });

            Assert.Equal("You are not a participant", exception.Message);
        }

        [Fact]
        public void Should_ThrowException_When_AddingComment_ToNotCompletedMeetup()
        {
            var meetup = new Meetup(
                organizerId: "123",
                subject: "DDD",
                when: DateTime.Now.AddDays(1),
                description: "DDD Practices",
                location: new Location("YTÜ Teknopark"),
                policy: meetupPolicy);

            var exception = Assert.Throws<MeetupDomainException>(() =>
            {
                meetup.AddComment("456", "Good!");
            });

            Assert.Equal("Meetup is not completed", exception.Message);
        }

        [Fact]
        public void Should_Sucess_When_AddingComment()
        {
            var meetup = new Meetup(
                organizerId: "123",
                subject: "DDD",
                when: DateTime.Now.AddDays(1),
                description: "DDD Practices",
                location: new Location("YTÜ Teknopark"),
                policy: meetupPolicy);

            meetup.Join("456");

            meetup.Complete();

            meetup.AddComment("456", "Good!");

            Assert.Single(meetup.Comments);
        }
    }
}
