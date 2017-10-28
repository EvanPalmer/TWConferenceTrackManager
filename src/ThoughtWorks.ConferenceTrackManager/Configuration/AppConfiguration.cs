using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ThoughtWorks.ConferenceTrackManager.Configuration
{
    //todo: unit test this
    //todo strongly type this too
    //todo make the detauls work
    public interface IAppConfiguration
    {
        int NumberOfTracks { get; }
        int MorningSessionStartHourAsTwentyFourHourInt { get; }
        int LunchTimeStartHourAsTwentyFourHourInt { get; }
        int AfternoonSessionStartHourAsTwentyFourHourInt { get; }
        int NetworkingSessionEarliestStartHourAsTwentyFourHourInt { get; }
        int NetworkingSessionLatestStartHourAsTwentyFourHourInt { get; }
    }

    public class AppConfiguration : IAppConfiguration
    {
        private IConfigurationSection _appSettings;

        public AppConfiguration()
        {
            // todo, try catch!
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            _appSettings = configuration.GetSection("AppSettings");
        }

        public int NumberOfTracks
        {
            get
            {
                const int defaultNumberOfTracks = 2;
                var numberOfTracks = defaultNumberOfTracks;
                var success = int.TryParse(_appSettings["numberOfTracks"], out numberOfTracks);
                return numberOfTracks;
            }  
        }

        public int MorningSessionStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultMorningSessionStartHour = 9;
                var morningSessionTimeHour = defaultMorningSessionStartHour;
                int.TryParse(_appSettings["morningSessionStartHourAsTwentyFourHourInt"], out morningSessionTimeHour);
                return morningSessionTimeHour;
            }
        }

        public int LunchTimeStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultLunchTimeStartTimeHour = 12;
                var lunchTimeStartTimeHour = defaultLunchTimeStartTimeHour;
                int.TryParse(_appSettings["lunchTimeStartTimeHourAsTwentyFourHourInt"], out lunchTimeStartTimeHour);
                return lunchTimeStartTimeHour;
            }
        }

        public int AfternoonSessionStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultAfternoonSessionStartHour = 13;
                var afternoonSessionStartHour = defaultAfternoonSessionStartHour;
                int.TryParse(_appSettings["afternoonSessionStartHourAsTwentyFourHourInt"], out afternoonSessionStartHour);
                return afternoonSessionStartHour;
            }
        }

        public int NetworkingSessionEarliestStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultNetworkingSessionEarliestStartHour = 16;
                var networkingSessionEarliestStartHour = defaultNetworkingSessionEarliestStartHour;
                int.TryParse(_appSettings["networkingSessionEarliestStartHourAsTwentyFourHourInt"], out networkingSessionEarliestStartHour);
                return networkingSessionEarliestStartHour;
            }
        }

        public int NetworkingSessionLatestStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultNetworkingSessionLatestStartHour = 17;
                var networkingSessionLatestStartHour = defaultNetworkingSessionLatestStartHour;
                int.TryParse(_appSettings["networkingSessionLatestStartHourAsTwentyFourHourInt"], out networkingSessionLatestStartHour);
                return networkingSessionLatestStartHour;
            }
        }
    }
}
