using System;
using ThoughtWorks.ConferenceTrackManager.Factories;
using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Access;

namespace ThoughtWorks.ConferenceTrackManager.App
{
    public interface IConferenceManager
    {
        void Run(string[] args);
    }

    public class ConferenceManager : IConferenceManager
    {
        private ITalkFactory _talkFactory;
        private IAppArgument _arguments;
        private IFileSystem _fileSystem;
        private IConferenceFactory _conferenceFactory;

        public ConferenceManager(IAppArgument arguments = null, IConferenceFactory conferenceFactory = null, ITalkFactory talkFactory = null, IFileSystem fileSystem = null)
        {
            _arguments = arguments;
            _conferenceFactory = conferenceFactory;
            _talkFactory = talkFactory;
            _fileSystem = fileSystem;
        }

        public void Run(string[] args)
        {
            var filename = string.Empty;
            var argumentsAreValid = _arguments.TryParse(args, out filename);

            if (argumentsAreValid)
            {
                // todo what if there's no input file?
                var talkDefinitionsList =  _fileSystem.ReadFileAsStringListOrEmptyList(filename);
                var allTalks = _talkFactory.CreateTalkCollectionFromInput(talkDefinitionsList);
                var conference = _conferenceFactory.Create(allTalks);
                conference.Print();
            }
        }
    }
}
