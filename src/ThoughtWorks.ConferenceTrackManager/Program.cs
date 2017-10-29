using System.Reflection;
using Autofac;
using ThoughtWorks.ConferenceTrackManager.App;

namespace ThoughtWorks.ConferenceTrackManager
{
    public class Program
    {
        private static IContainer Container { get; set; }

        public static void Main(string[] args)
        {
            // todo I think there's a nice way to handle arguments. Oh hangon, I think that's powershell
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            Container = builder.Build();

            args = new[] { "talks.txt" };
            using(var scope = Container.BeginLifetimeScope())
            {
                var conferenceManager = scope.Resolve<IConferenceManager>();
                conferenceManager.Run(args);
            }
        }
    }
}