using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.Models;

namespace ThoughtWorks.ConferenceTrackManager.Factories
{
    public interface IConferenceFactory
    {
        IConference Build(IList<ITalk> allTalks);
    }

    public class ConferenceFactory : IConferenceFactory
    {
        public IConference Build(IList<ITalk> allTalks)
        {
            return new Conference(allTalks);
        }
    }
}