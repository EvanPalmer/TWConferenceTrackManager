using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.Access;
using ThoughtWorks.ConferenceTrackManager.App;
using ThoughtWorks.ConferenceTrackManager.Models;

namespace ThoughtWorks.ConferenceTrackManager.Factories
{
    public interface IConferenceFactory
    {
        IConference Create(IList<ITalk> talks);
    }
    public class ConferenceFactory : IConferenceFactory
    {
        private readonly IConferenceSessionCreator _sessionCreator;
        private readonly IOutputWriter _outputWriter;
        private readonly ITalkDistributor _talkDistributor;

        public ConferenceFactory(IConferenceSessionCreator sessionCreator, IOutputWriter outputWriter, ITalkDistributor talkDistributor)
        {
            _sessionCreator = sessionCreator;
            _outputWriter = outputWriter;
            _talkDistributor = talkDistributor;
        }

        public IConference Create(IList<ITalk> talks)
        {
            return new Conference(talks, _sessionCreator, _outputWriter, _talkDistributor);
        }
    }
}
