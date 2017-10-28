using System;
namespace ThoughtWorks.ConferenceTrackManager.Access
{
    public interface IOutputWriter
    {
        void WriteLine(string outputToWrite);
    }

    public class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteLine(string outputToWrite)
        {
            Console.WriteLine(outputToWrite);
        }
    }
}
