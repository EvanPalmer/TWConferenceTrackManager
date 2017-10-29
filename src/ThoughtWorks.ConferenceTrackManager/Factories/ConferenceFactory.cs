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
        readonly IConferenceSessionBuilder _conferenceBuilder;
        readonly IOutputWriter _outputWriter;

        public ConferenceFactory(IConferenceSessionBuilder conferenceBuilder, IOutputWriter outputWriter)
        {
            _conferenceBuilder = conferenceBuilder;
            _outputWriter = outputWriter;
        }

        //todo should return concrete type
        public IConference Create(IList<ITalk> talks)
        {
            return new Conference(talks, _conferenceBuilder, _outputWriter);
        }
    }
}
