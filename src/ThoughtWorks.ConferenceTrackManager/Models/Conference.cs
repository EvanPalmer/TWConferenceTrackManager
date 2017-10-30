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
        private IList<ITalk> _talks;
        public IList<ITalk> Talks
        {
            get
            {
                return _talks;
            }
        }

        public Conference(IList<ITalk> talks, IConferenceSessionBuilder conferenceSessionBuilder, IOutputWriter outputWriter)
        {
            _talks = talks;
            _conferenceSessionBuilder = conferenceSessionBuilder;
            _outputWriter = outputWriter;
        }

        public void Print()
        {
            var conferenceSessions = _conferenceSessionBuilder.CreateSessionsOrEmptyListFromConfig();
            var successfullyDistributedTalks = _conferenceSessionBuilder.DistributeTalksAcrossSessions(conferenceSessions, _talks);

            if(!successfullyDistributedTalks)
            {
                _outputWriter.WriteLine("Didn't add a talk! The conference is full and I don't know what to do about it!\nHere's the best I could do:\n");
            }

            foreach(var session in conferenceSessions)
            {
                session.Print();
            }
        }
    }
}