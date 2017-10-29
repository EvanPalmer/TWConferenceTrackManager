using System;
using System.Collections.Generic;
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

        public ConferenceFactory(IConferenceSessionBuilder conferenceBuilder)
        {
            _conferenceBuilder = conferenceBuilder;
        }

        //todo should return concrete type
        public IConference Create(IList<ITalk> talks)
        {
            return new Conference(talks, _conferenceBuilder);
        }
    }
}
