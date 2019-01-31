namespace DDDSample.API
{
    using DDDSample.API.Infrastructure;
    using DDDSample.Application.Infrastructure;
    using DDDSample.Application.Meetup.Commands.CreateMeetup;
    using DDDSample.Domain.MeetupAggregate;
    using DDDSample.Domain.MeetupAggregate.Policies;
    using DDDSample.Infrastructure;
    using DDDSample.Infrastructure.MeetupDomain;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddMediatR(typeof(CreateMeetupCommand).GetTypeInfo().Assembly);

            services.AddHttpContextAccessor();
            services.AddTransient<IMeetupRepository, MeetupRepository>();
            services.AddTransient<IMeetupPolicy, MeetupPolicy>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddDbContext<MeetupDbContext>((options) => options.UseInMemoryDatabase(Guid.NewGuid().ToString()), ServiceLifetime.Singleton);

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<MeetupDbContext>(options =>
                {
                    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MeetupDbContext;Trusted_Connection=True;MultipleActiveResultSets=true",
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                }, ServiceLifetime.Scoped);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler();
            app.UseMvc();
        }
    }
}
