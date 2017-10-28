using System;
using ThoughtWorks.ConferenceTrackManager.Access;

namespace ThoughtWorks.ConferenceTrackManager.Models
{
    public interface IAppArgument
    {
        bool TryParse(string[] args, out string filename);
    }

    // todo unit test this
    public class AppArgument : IAppArgument
    {
        readonly IOutputWriter _outputWriter;

        public AppArgument(IOutputWriter outputWriter = null)
        {
            _outputWriter = outputWriter ??  new ConsoleOutputWriter();
        }

        public bool TryParse(string[] args, out string filename)
        {
            // todo check if file exists, like validation
            filename = string.Empty;

            var argumentsAreValid = ArgumentsAreValid(args);
            if (argumentsAreValid)
            {
                filename = args[0];
            }

            return argumentsAreValid;
        }

        private bool ArgumentsAreValid(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                _outputWriter.WriteLine("usage: dotnet run [filename]");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}