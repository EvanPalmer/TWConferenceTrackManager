using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Configuration;
using ThoughtWorks.ConferenceTrackManager.Factories;

namespace ThoughtWorks.ConferenceTrackManager.App
{
    public interface IConferenceSessionCreator
    {
        IList<IConferenceSession> CreateSessionsOrEmptyListFromConfig();
    }

    public class ConferenceSessionCreator : IConferenceSessionCreator
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly IConferenceSessionFactory _conferenceSessionFactory;

        public ConferenceSessionCreator(IAppConfiguration appConfiguration, IConferenceSessionFactory conferenceSessionFactory)
        {
            _conferenceSessionFactory = conferenceSessionFactory;
            _appConfiguration = appConfiguration;
        }

        public IList<IConferenceSession> CreateSessionsOrEmptyListFromConfig()
        {
            var sessions = new List<IConferenceSession>();
            for (var trackIndex = 0; trackIndex < _appConfiguration.NumberOfTracks; trackIndex++)
            {
                var morningSession = _conferenceSessionFactory.CreateMorningConferenceSession(trackIndex);
                var afternoonSession = _conferenceSessionFactory.CreateAfternoonConferenceSession();
                sessions.Add(morningSession);
                sessions.Add(afternoonSession);
            }

            return sessions;
        }   

    }
}
