namespace DDDSample.Infrastructure
{
    using DDDSample.Domain;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, MeetupDbContext context)
        {
            var entities = context.ChangeTracker
                .Entries<Entity>()
                .Where((x) => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = entities
                .SelectMany((x) => x.Entity.DomainEvents)
                .ToList();

            entities.ToList()
                .ForEach((x) => x.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent,default(CancellationToken));
                });

            await Task.WhenAll(tasks);
        }
    }
}
