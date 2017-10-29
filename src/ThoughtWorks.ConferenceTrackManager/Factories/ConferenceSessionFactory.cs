using ThoughtWorks.ConferenceTrackManager.Access;
using ThoughtWorks.ConferenceTrackManager.Configuration;
using ThoughtWorks.ConferenceTrackManager.Models;

namespace ThoughtWorks.ConferenceTrackManager.Factories
{
    public interface IConferenceSessionFactory
    {
        IConferenceSession CreateMorningConferenceSession();
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

        public IConferenceSession CreateMorningConferenceSession()
        {
            return new MorningConferenceSession(_appConfiguration.MorningSessionStartHourAsTwentyFourHourInt, 
                                                _appConfiguration.LunchTimeStartHourAsTwentyFourHourInt, 
                                                _appConfiguration.NumberOfTracks, 
                                                _outputWriter);
        }

        public IConferenceSession CreateAfternoonConferenceSession()
        {
            return new AfternoonConferenceSession(_appConfiguration.AfternoonSessionStartHourAsTwentyFourHourInt, 
                                                  _appConfiguration.NetworkingSessionEarliestStartHourAsTwentyFourHourInt, 
                                                  _appConfiguration.NetworkingSessionLatestStartHourAsTwentyFourHourInt, 
                                                  _appConfiguration.NumberOfTracks,
                                                  _outputWriter);
        }
    }
}
