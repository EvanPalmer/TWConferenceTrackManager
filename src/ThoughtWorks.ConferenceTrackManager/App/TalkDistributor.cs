using System.Collections.Generic;
using System.Linq;
using ThoughtWorks.ConferenceTrackManager.Models;

namespace ThoughtWorks.ConferenceTrackManager.App
{
    public interface ITalkDistributor
    {
        bool DistributeTalksAcrossSessions(IList<IConferenceSession> sessions, IList<ITalk> allTalks);
    }

    public class TalkDistributor : ITalkDistributor 
    {
        public bool DistributeTalksAcrossSessions(IList<IConferenceSession> sessions, IList<ITalk> talks)
        {
            var successfullyDistributedTalks = true;
            var sortedTalks = talks.OrderByDescending(t => t.LengthInMinutes).ToList();
            var talkIndex = 0;

            while (talkIndex < sortedTalks.Count())
            {
                var successfullyAddedTalkToSession = false;
                foreach (var session in sessions)
                {
                    successfullyAddedTalkToSession = session.TryIncludeTalkInSession(sortedTalks[talkIndex]);
                    if (successfullyAddedTalkToSession)
                    {
                        talkIndex = talkIndex + 1;
                    }
                }

                if (!successfullyAddedTalkToSession)
                {
                    successfullyDistributedTalks = false;
                    talkIndex = talkIndex + 1;
                }
            }
            return successfullyDistributedTalks;
        }
    }
}
