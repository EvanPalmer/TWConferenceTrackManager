using System;
using ThoughtWorks.ConferenceTrackManager.Factories;
using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Access;

namespace ThoughtWorks.ConferenceTrackManager.App
{
    public class ConferenceManager
    {
        private IConferenceFactory _conferenceFactory;
        private ITalkFactory _talkFactory;
        private IAppArgument _arguments;
        private IFileSystem _fileSystem;

        public ConferenceManager(IAppArgument arguments = null, IConferenceFactory conferenceFactory = null, ITalkFactory talkFactory = null, IFileSystem fileSystem = null)
        {
            _arguments = arguments ?? new AppArgument();
            _conferenceFactory = conferenceFactory ?? new ConferenceFactory();
            _talkFactory = talkFactory ?? new TalkFactory();
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public void Run(string[] args)
        {
            var filename = string.Empty;
            var argumentsAreValid = _arguments.TryParse(args, out filename);

            if (argumentsAreValid)
            {
                // todo what if there's no input file?
                var talkDefinitionsList =  _fileSystem.ReadFileAsStringListOrEmptyList(filename);
                var allTalks = _talkFactory.BuildTalkCollectionFromInput(talkDefinitionsList);
                var conference = _conferenceFactory.Build(allTalks);
                conference.Print();
            }
        }
    }
}
