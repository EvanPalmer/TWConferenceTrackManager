using ThoughtWorks.ConferenceTrackManager.App;

namespace ThoughtWorks.ConferenceTrackManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // todo I think there's a nice way to handle arguments. Oh hangon, I think that's powershell

            args = new[] { "talks.txt" };
            var conferenceManager = new ConferenceManager();
            conferenceManager.Run(args);
        }
    }
}