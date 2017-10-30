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
        private ITalkDistributor _talkDistributor;
        private IOutputWriter _outputWriter;
        private IList<ITalk> _talks;
        public IList<ITalk> Talks
        {
            get
            {
                return _talks;
            }
        }

        public Conference(IList<ITalk> talks, IConferenceSessionBuilder conferenceSessionBuilder, IOutputWriter outputWriter, ITalkDistributor talkDistributor)
        {
            _talks = talks;
            _conferenceSessionBuilder = conferenceSessionBuilder;
            _outputWriter = outputWriter;
            _talkDistributor = talkDistributor;
        }

        public void Print()
        {
            var conferenceSessions = _conferenceSessionBuilder.CreateSessionsOrEmptyListFromConfig();
            var successfullyDistributedTalks = _talkDistributor.DistributeTalksAcrossSessions(conferenceSessions, _talks);

            if(!successfullyDistributedTalks)
            {
                _outputWriter.WriteLine("Didn't add at least one of the talks!\nHere's the best I could do:\n");
            }

            foreach(var session in conferenceSessions)
            {
                session.Print();
            }
        }
    }
}