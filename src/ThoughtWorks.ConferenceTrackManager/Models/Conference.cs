using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.Access;
using ThoughtWorks.ConferenceTrackManager.App;

namespace ThoughtWorks.ConferenceTrackManager.Models
{
    public interface IConference
    {
        void Print();
    }

    public class Conference : IConference
    {
        private IConferenceSessionBuilder _conferenceSessionBuilder;
        private IOutputWriter _outputWriter;
        private IList<ITalk> _allTalks;
        public IList<ITalk> AllTalks
        {
            get
            {
                return _allTalks;
            }
        }

        public Conference(IList<ITalk> talks, IConferenceSessionBuilder conferenceSessionBuilder, IOutputWriter outputWriter)
        {
            _allTalks = talks;
            _conferenceSessionBuilder = conferenceSessionBuilder;
            _outputWriter = outputWriter;
        }

        public void Print()
        {
            var conferenceSessions = _conferenceSessionBuilder.CreateSessionsOrEmptyList();
            var sortedTalks = _conferenceSessionBuilder.SortTalks(_allTalks);
            var response = _conferenceSessionBuilder.PopulateSessionsWithTalks(conferenceSessions, sortedTalks );

            if(!response.SuccessfullyAddedAllTalksToSession)
            {
                _outputWriter.WriteLine(response.Message);
            }

            foreach(var session in conferenceSessions)
            {
                session.Print();
            }
        }
    }
}