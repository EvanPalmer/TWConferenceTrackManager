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
            SetUpIoc();

            using (var scope = Container.BeginLifetimeScope())
            {
                var conferenceManager = scope.Resolve<IConferenceManager>();
                conferenceManager.Run(args);
            }
        }

        private static void SetUpIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            Container = builder.Build();
        }
    }
}