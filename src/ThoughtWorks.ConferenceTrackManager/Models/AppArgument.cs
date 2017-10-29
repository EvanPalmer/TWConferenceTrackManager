using System;
using ThoughtWorks.ConferenceTrackManager.Access;

namespace ThoughtWorks.ConferenceTrackManager.Models
{
    public interface IAppArgument
    {
        bool TryParse(string[] args, out string filename);
    }

    public class AppArgument : IAppArgument
    {
        readonly IOutputWriter _outputWriter;

        public AppArgument(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public bool TryParse(string[] args, out string filename)
        {
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