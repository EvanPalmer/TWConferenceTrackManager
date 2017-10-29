using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Configuration;
using System.Linq;
using System;
using ThoughtWorks.ConferenceTrackManager.Exceptions;
using ThoughtWorks.ConferenceTrackManager.Factories;

namespace ThoughtWorks.ConferenceTrackManager.App
{
    public interface IConferenceSessionBuilder
    {
        IList<IConferenceSession> CreateSessionsOrEmptyList();
        List<ITalk> SortTalks(IList<ITalk> allTalks);
        void PopulateSessionsWithTalks(IList<IConferenceSession> sessions, IList<ITalk> allTalks);
    }

    public class ConferenceSessionBuilder : IConferenceSessionBuilder
    {
        private readonly IAppConfiguration _appConfiguration;
        readonly IConferenceSessionFactory _conferenceSessionFactory;

        public ConferenceSessionBuilder(IAppConfiguration appConfiguration, IConferenceSessionFactory conferenceSessionFactory)
        {
            _conferenceSessionFactory = conferenceSessionFactory;
            _appConfiguration = appConfiguration;
        }

        public IList<IConferenceSession> CreateSessionsOrEmptyList()
        {
            var sessions = new List<IConferenceSession>();
            for (var trackNumber = 0; trackNumber < _appConfiguration.NumberOfTracks; trackNumber++)
            {
                var morningSession = _conferenceSessionFactory.CreateMorningConferenceSession();
                var afternoonSession = _conferenceSessionFactory.CreateAfternoonConferenceSession();
                sessions.Add(morningSession);
                sessions.Add(afternoonSession);
            }

            return sessions;
        }   

        public List<ITalk> SortTalks(IList<ITalk> allTalks)
        {
            var sortedTalks = allTalks.OrderByDescending(t => t.LengthInMinutes).ToList();
            return sortedTalks;
        }

        public void PopulateSessionsWithTalks(IList<IConferenceSession> sessions, IList<ITalk> allTalks)
        {
            //todo make this smaller And better tested.
            var sessionIndex = 0;
            var failedAttempts = 0;
            var allSessionsFailed = false;
            var successfullyAddedTalkToSession = false;

            foreach (var talk in allTalks)
            {
                do
                {
                    successfullyAddedTalkToSession = sessions[sessionIndex].TryIncludeTalkInSession(talk);
                    if(!successfullyAddedTalkToSession)
                    {
                        failedAttempts = failedAttempts + 1;
                        allSessionsFailed = failedAttempts == sessions.Count();
                    }else{
                        failedAttempts = 0;
                        allSessionsFailed = false;
                    }
                    sessionIndex = sessionIndex + 1;
                    Console.WriteLine("AllSessionsFaild: " + allSessionsFailed);
                    Console.WriteLine("sessionIndex < sessions.Count(): " + (sessionIndex < sessions.Count()));
                } while (!successfullyAddedTalkToSession && !allSessionsFailed && sessionIndex < sessions.Count());

                if (allSessionsFailed)
                {
                    break;
                    // throw new ConferenceSetUpException("Didn't add a talk! The conference is full and I don't know what to do about it!");
                }

                if (sessionIndex == sessions.Count())
                {
                    sessionIndex = 0;
                }
            }
        }
    }
}
