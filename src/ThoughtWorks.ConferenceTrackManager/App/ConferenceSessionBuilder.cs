using System.Collections.Generic;
using System.Linq;
using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Configuration;
using ThoughtWorks.ConferenceTrackManager.Factories;

namespace ThoughtWorks.ConferenceTrackManager.App
{
    public interface IConferenceSessionBuilder
    {
        IList<IConferenceSession> CreateSessionsOrEmptyList();
        List<ITalk> SortTalks(IList<ITalk> allTalks);
        PopulateSessionsWithTalksResponse PopulateSessionsWithTalks(IList<IConferenceSession> sessions, IList<ITalk> allTalks);
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

        public IList<IConferenceSession> CreateSessionsOrEmptyList()
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

        public List<ITalk> SortTalks(IList<ITalk> allTalks)
        {
            var sortedTalks = allTalks.OrderByDescending(t => t.LengthInMinutes).ToList();
            return sortedTalks;
        }

        public PopulateSessionsWithTalksResponse PopulateSessionsWithTalks(IList<IConferenceSession> sessions, IList<ITalk> allTalks)
        {
            // TODO Can improve
            var sessionIndex = 0;
            var failedAttempts = 0;
            var allSessionsFailed = false;
            var successfullyAddedTalkToSession = false;
            var response = new PopulateSessionsWithTalksResponse { SuccessfullyAddedAllTalksToSession = true };
            foreach (var talk in allTalks)
            {
                do
                {
                    successfullyAddedTalkToSession = sessions[sessionIndex].TryIncludeTalkInSession(talk);
                    if (!successfullyAddedTalkToSession)
                    {
                        failedAttempts = failedAttempts + 1;
                        allSessionsFailed = failedAttempts == sessions.Count();
                    }
                    else
                    {
                        failedAttempts = 0;
                        allSessionsFailed = false;
                    }
                    sessionIndex = sessionIndex + 1;
                } while (!successfullyAddedTalkToSession &&
                         !allSessionsFailed &&
                         sessionIndex < sessions.Count());

                if (allSessionsFailed)
                {
                    response.SuccessfullyAddedAllTalksToSession = false;
                    response.Message = "Didn't add a talk! The conference is full and I don't know what to do about it!\nHere's the best I could do:\n";
                }

                if (sessionIndex == sessions.Count())
                {
                    sessionIndex = 0;
                }
            }
            return response;
        }
    }

    public class PopulateSessionsWithTalksResponse
    {
        public bool SuccessfullyAddedAllTalksToSession { get; set; }
        public string Message { get; set; }
    }
}
