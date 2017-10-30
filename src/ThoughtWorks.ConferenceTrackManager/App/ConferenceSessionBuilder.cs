using System.Collections.Generic;
using System.Linq;
using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Configuration;
using ThoughtWorks.ConferenceTrackManager.Factories;

namespace ThoughtWorks.ConferenceTrackManager.App
{
    public interface IConferenceSessionBuilder
    {
        IList<IConferenceSession> CreateSessionsOrEmptyListFromConfig();
        bool DistributeTalksAcrossSessions(IList<IConferenceSession> sessions, IList<ITalk> allTalks);
    }

    public class ConferenceSessionBuilder : IConferenceSessionBuilder
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly IConferenceSessionFactory _conferenceSessionFactory;

        public ConferenceSessionBuilder(IAppConfiguration appConfiguration, IConferenceSessionFactory conferenceSessionFactory)
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

        public bool DistributeTalksAcrossSessions(IList<IConferenceSession> sessions, IList<ITalk> talks)
        {
            var successfullyDistributedTalks = true;
            var sortedTalks = talks.OrderByDescending(t => t.LengthInMinutes).ToList();
            var talkIndex = 0;

            while (talkIndex < sortedTalks.Count())
            {
                var successfullyAddedTalkToSession = false;
                foreach(var session in sessions)
                {
                    successfullyAddedTalkToSession = session.TryIncludeTalkInSession(sortedTalks[talkIndex]);
                    if (successfullyAddedTalkToSession)
                    {
                        talkIndex = talkIndex + 1;
                    }
                }

                if(!successfullyAddedTalkToSession)
                {
                    successfullyDistributedTalks = false;
                    talkIndex = talkIndex + 1;
                }
            }

            return successfullyDistributedTalks;
        }
    }
}
