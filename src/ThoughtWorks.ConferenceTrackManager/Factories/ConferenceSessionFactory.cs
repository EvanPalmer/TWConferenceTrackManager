using ThoughtWorks.ConferenceTrackManager.Access;
using ThoughtWorks.ConferenceTrackManager.Configuration;
using ThoughtWorks.ConferenceTrackManager.Models;

namespace ThoughtWorks.ConferenceTrackManager.Factories
{
    public interface IConferenceSessionFactory
    {
        IConferenceSession CreateMorningConferenceSession(int trackIndex);
        IConferenceSession CreateAfternoonConferenceSession();
    }

    public class ConferenceSessionFactory : IConferenceSessionFactory
    {
        readonly IOutputWriter _outputWriter;
        readonly IAppConfiguration _appConfiguration;

        public ConferenceSessionFactory(IOutputWriter outputWriter, IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            _outputWriter = outputWriter;
        }

        public IConferenceSession CreateMorningConferenceSession(int trackIndex)
        {
            return new MorningConferenceSession(_appConfiguration.MorningSessionStartHourAsTwentyFourHourInt, 
                                                _appConfiguration.LunchTimeStartHourAsTwentyFourHourInt, 
                                                trackIndex, 
                                                _outputWriter);
        }

        public IConferenceSession CreateAfternoonConferenceSession()
        {
            return new AfternoonConferenceSession(_appConfiguration.AfternoonSessionStartHourAsTwentyFourHourInt, 
                                                  _appConfiguration.NetworkingSessionEarliestStartHourAsTwentyFourHourInt, 
                                                  _appConfiguration.NetworkingSessionLatestStartHourAsTwentyFourHourInt, 
                                                  _outputWriter);
        }
    }
}
