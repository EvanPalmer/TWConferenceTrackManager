using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Configuration;
using System.Linq;
using System;
using ThoughtWorks.ConferenceTrackManager.Exceptions;

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

        public ConferenceSessionBuilder(IAppConfiguration appConfiguration = null)
        {
            _appConfiguration = appConfiguration ?? new AppConfiguration();
        }

        public IList<IConferenceSession> CreateSessionsOrEmptyList()
        {
            var sessions = new List<IConferenceSession>();
            for (var trackNumber = 0; trackNumber < _appConfiguration.NumberOfTracks; trackNumber++)
            {
                var morningSession = new MorningConferenceSession(_appConfiguration.MorningSessionStartHourAsTwentyFourHourInt, _appConfiguration.LunchTimeStartHourAsTwentyFourHourInt, trackNumber);
                var lateSession = new AfternoonConferenceSession(_appConfiguration.AfternoonSessionStartHourAsTwentyFourHourInt, _appConfiguration.NetworkingSessionEarliestStartHourAsTwentyFourHourInt, _appConfiguration.NetworkingSessionLatestStartHourAsTwentyFourHourInt, trackNumber);
                sessions.Add(morningSession);
                sessions.Add(lateSession);
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
            //todo make this smaller
            var sessionIndex = 0;
            var failedAttempts = 0;
            var allSessionsFailed = false;

            foreach (var talk in allTalks)
            {
                do
                {
                    var successfullyAddedTalkToSession = sessions[sessionIndex].TryIncludeTalkInSession(talk);
                    if(!successfullyAddedTalkToSession)
                    {
                        failedAttempts = failedAttempts + 1;
                        allSessionsFailed = failedAttempts == sessions.Count();
                    }else{
                        failedAttempts = 0;
                    }
                    sessionIndex = sessionIndex + 1;
                } while (allSessionsFailed && sessionIndex < sessions.Count());

                if (allSessionsFailed)
                {
                    throw new ConferenceSetUpException("Didn't add a talk! The conference is full and I don't know what to do about it!");
                }

                if (sessionIndex == sessions.Count())
                {
                    sessionIndex = 0;
                }
            }
        }
    }
}
