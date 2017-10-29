using System;
using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.Access;

namespace ThoughtWorks.ConferenceTrackManager.Models
{
    public interface IConferenceSession
    {
        bool TryIncludeTalkInSession(ITalk talk);
        void Print();
    }

    public abstract class ConferenceSession : IConferenceSession
    {
        // todo mkaes these private or protected fields
        protected int _startHour;
        protected int _endHour;
        protected ConferenceTime _currentSessionEndtime;
        protected List<ITalk> _talks;
        protected IOutputWriter _outputWriter;

        public ConferenceSession(int startHour, int endHour, IOutputWriter outputWriter)
        {
            _endHour = endHour;
            _startHour = startHour;
            _talks = new List<ITalk>();
            _outputWriter = outputWriter;
            _currentSessionEndtime = new ConferenceTime(startHour);
        }

        public abstract void Print();

        public bool TryIncludeTalkInSession(ITalk talk)
        {
            var potentialSessionEndTime = _currentSessionEndtime.AddMinutes(talk.LengthInMinutes);
            var latestSessionEndtime = new ConferenceTime(_endHour);
            var talkCanFitIsSession = potentialSessionEndTime.IsLessThanOrEqualTo(latestSessionEndtime);

            if(talkCanFitIsSession)
            {
                _talks.Add(talk);
                _currentSessionEndtime = potentialSessionEndTime;
            }

            return talkCanFitIsSession;
        }

        protected ConferenceTime PrintTalksWithTime(ConferenceTime nextAvailableTimeToStartTalk)
        {
            foreach (var talk in _talks)
            {
                _outputWriter.WriteLine($"{nextAvailableTimeToStartTalk.TimeAsTwelveHourString()} {talk.TalkDefinition}");
                nextAvailableTimeToStartTalk = nextAvailableTimeToStartTalk.AddMinutes(talk.LengthInMinutes);
            }
            return nextAvailableTimeToStartTalk;
        }
    }

    public class MorningConferenceSession : ConferenceSession
    {
        protected int _trackIndex;

        public MorningConferenceSession(int startHour, int endHour, int trackIndex, IOutputWriter outputWriter = null)
            : base(startHour, endHour, outputWriter)
        {
            _outputWriter = outputWriter ?? new ConsoleOutputWriter();
            _trackIndex = trackIndex;
        }       

        protected void PrintTrackNumber()
        {
            _outputWriter.WriteLine($"Track {_trackIndex + 1}");
        }

        private int ConvertTrackIndexToTrackNumber()
        {
            return _trackIndex + 1;
        }

        public override void Print()
        {
            var startTime = new ConferenceTime(_startHour);
            PrintTrackNumber();
            PrintTalksWithTime(startTime);

            var endTime = new ConferenceTime(_endHour);
            _outputWriter.WriteLine($"{endTime.TimeAsTwelveHourString()} Lunch");
        }
    }

    public class AfternoonConferenceSession : ConferenceSession
    {
        readonly private int _networkingSessionEarliestStartHour;
        readonly private int _networkingSessionLatestStartHour;

        public AfternoonConferenceSession(int startHour, int networkingSessionEarliestStartHour, int networkingSessionLatestStartHour, IOutputWriter outputWriter = null)
            : base(startHour, networkingSessionLatestStartHour, outputWriter)
        {
            _networkingSessionEarliestStartHour = networkingSessionEarliestStartHour;
            _networkingSessionLatestStartHour = networkingSessionLatestStartHour;
        }

        public override void Print()
        {
            var nextAvailableTimeToStartTalk = new ConferenceTime(_startHour);
            nextAvailableTimeToStartTalk = PrintTalksWithTime(nextAvailableTimeToStartTalk);
            nextAvailableTimeToStartTalk = nextAvailableTimeToStartTalk.RoundToWholeHour();
            var sessionEndTime = new ConferenceTime(_networkingSessionLatestStartHour);
                   
            if(nextAvailableTimeToStartTalk.HourAsTwentyFourHourInt <= _networkingSessionEarliestStartHour)
            {
                sessionEndTime = new ConferenceTime(_networkingSessionEarliestStartHour);
            }

            _outputWriter.WriteLine($"{sessionEndTime.TimeAsTwelveHourString()} Networking Event");   
            _outputWriter.WriteLine(String.Empty);
        }
    }
}
