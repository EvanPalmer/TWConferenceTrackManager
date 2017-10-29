using System;
using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.App;
using ThoughtWorks.ConferenceTrackManager.Configuration;

namespace ThoughtWorks.ConferenceTrackManager.Models
{
    public interface IConference
    {
        void Print();
    }

    public class Conference : IConference
    {
        private IConferenceSessionBuilder _conferenceSessionBuilder;

        private IList<ITalk> _allTalks;
        public IList<ITalk> AllTalks
        {
            get
            {
                return _allTalks;
            }
        }

        // TODO: Use proper IOC
        public Conference(IList<ITalk> talks, IConferenceSessionBuilder conferenceSessionBuilder)
        {
            _allTalks = talks;
            _conferenceSessionBuilder = conferenceSessionBuilder;
        }

        public void Print()
        {
            var conferenceSessions = _conferenceSessionBuilder.CreateSessionsOrEmptyList();
            var sortedTalks = _conferenceSessionBuilder.SortTalks(_allTalks);
            _conferenceSessionBuilder.PopulateSessionsWithTalks(conferenceSessions, sortedTalks );

            foreach(var session in conferenceSessions)
            {
                session.Print();
            }
        }
    }
}