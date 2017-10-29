using System.IO;
using Microsoft.Extensions.Configuration;
using ThoughtWorks.ConferenceTrackManager.Access;

namespace ThoughtWorks.ConferenceTrackManager.Configuration
{
    //todo: unit test this
    //todo strongly type this too
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
        private readonly IOutputWriter _outputWriter;

        public AppConfiguration(IOutputWriter outputWriter)
        {
            const string settingsFilename = "appSettings.json";
            _outputWriter = outputWriter;
            var builder = new ConfigurationBuilder();

            try
            {
                builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFilename, optional: true, reloadOnChange: true);
            }catch{
                outputWriter.WriteLine($"Error loading {settingsFilename}");
            }
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
                if(!success){
                    numberOfTracks = defaultNumberOfTracks;
                }
                return numberOfTracks;
            }  
        }

        public int MorningSessionStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultMorningSessionStartHour = 9;
                var morningSessionTimeHour = defaultMorningSessionStartHour;
                var success = int.TryParse(_appSettings["morningSessionStartHourAsTwentyFourHourInt"], out morningSessionTimeHour);
                if (!success)
                {
                    morningSessionTimeHour = defaultMorningSessionStartHour;
                }

                return morningSessionTimeHour;
            }
        }

        public int LunchTimeStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultLunchTimeStartTimeHour = 12;
                var lunchTimeStartTimeHour = defaultLunchTimeStartTimeHour;
                var success = int.TryParse(_appSettings["lunchTimeStartTimeHourAsTwentyFourHourInt"], out lunchTimeStartTimeHour);
                if (!success)
                {
                    lunchTimeStartTimeHour = defaultLunchTimeStartTimeHour;
                }

                return lunchTimeStartTimeHour;
            }
        }

        public int AfternoonSessionStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultAfternoonSessionStartHour = 13;
                var afternoonSessionStartHour = defaultAfternoonSessionStartHour;
                var success = int.TryParse(_appSettings["afternoonSessionStartHourAsTwentyFourHourInt"], out afternoonSessionStartHour);
                if (!success)
                {
                    afternoonSessionStartHour = defaultAfternoonSessionStartHour;
                }

                return afternoonSessionStartHour;
            }
        }

        public int NetworkingSessionEarliestStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultNetworkingSessionEarliestStartHour = 16;
                var networkingSessionEarliestStartHour = defaultNetworkingSessionEarliestStartHour;
                var success = int.TryParse(_appSettings["networkingSessionEarliestStartHourAsTwentyFourHourInt"], out networkingSessionEarliestStartHour);
                if (!success)
                {
                    networkingSessionEarliestStartHour = defaultNetworkingSessionEarliestStartHour;
                }

                return networkingSessionEarliestStartHour;
            }
        }

        public int NetworkingSessionLatestStartHourAsTwentyFourHourInt
        {
            get
            {
                const int defaultNetworkingSessionLatestStartHour = 17;
                var networkingSessionLatestStartHour = defaultNetworkingSessionLatestStartHour;
                var success = int.TryParse(_appSettings["networkingSessionLatestStartHourAsTwentyFourHourInt"], out networkingSessionLatestStartHour);
                if (!success)
                {
                    networkingSessionLatestStartHour = defaultNetworkingSessionLatestStartHour;
                }

                return networkingSessionLatestStartHour;
            }
        }
    }
}
