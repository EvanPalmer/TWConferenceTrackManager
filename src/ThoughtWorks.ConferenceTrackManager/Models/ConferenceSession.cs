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

    // Todo replace inheritacnce with composition
    public abstract class ConferenceSession : IConferenceSession
    {
        // todo mkaes these private or protected fields
        protected int _startHour;
        protected int _endHour;
        protected int _trackIndex;
        protected ConferenceTime _currentSessionEndtime;
        protected List<ITalk> _talks;
        protected IOutputWriter _outputWriter;

        public ConferenceSession(int startHour, int endHour, int trackIndex, IOutputWriter outputWriter)
        {
            _trackIndex = trackIndex;
            _endHour = endHour;
            _startHour = startHour;
            _talks = new List<ITalk>();
            _outputWriter = outputWriter ?? new ConsoleOutputWriter();
            _currentSessionEndtime = new ConferenceTime(startHour);
        }

        public abstract void Print();

        public bool TryIncludeTalkInSession(ITalk talk)
        {
            _currentSessionEndtime.AddMinutes(talk.LengthInMinutes);
            var latestSessionEndtime = new ConferenceTime(_endHour);
            var talkCanFitIsSession = _currentSessionEndtime.IsLessThanOrEqualTo(latestSessionEndtime);

            if(talkCanFitIsSession)
            {
                _talks.Add(talk);
            } else { 
                // todo this sucks
                _currentSessionEndtime.SubtractMinutes(talk.LengthInMinutes);
            }

            return talkCanFitIsSession;
        }
        protected void PrintTrackNumber()
        {
            _outputWriter.WriteLine($"Track {_trackIndex + 1}");
        }

        private int ConvertTrackIndexToTrackNumber()
        {
            return _trackIndex + 1;
        }

        protected void PrintTalksWithTime(ConferenceTime nextAvailableTimeToStartTalk)
        {
            foreach (var talk in _talks)
            {
                _outputWriter.WriteLine($"{nextAvailableTimeToStartTalk.TimeAsTwelveHourString()} {talk.TalkDefinition}");
                nextAvailableTimeToStartTalk.AddMinutes(talk.LengthInMinutes);
            }
        }
    }

    public class MorningConferenceSession : ConferenceSession
    {
        public MorningConferenceSession(int startHour, int endHour, int trackNumber, IOutputWriter outputWriter = null)
            : base(startHour, endHour, trackNumber, outputWriter)
        {
            _outputWriter = outputWriter ?? new ConsoleOutputWriter();
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

        public AfternoonConferenceSession(int startHour, int networkingSessionEarliestStartHour, int networkingSessionLatestStartHour, int trackIndex, IOutputWriter outputWriter = null)
            : base(startHour, networkingSessionLatestStartHour, trackIndex, outputWriter)
        {
            _networkingSessionEarliestStartHour = networkingSessionEarliestStartHour;
            _networkingSessionLatestStartHour = networkingSessionLatestStartHour;
        }

        public override void Print()
        {
            var nextAvailableTimeToStartTalk = new ConferenceTime(_startHour);
            PrintTalksWithTime(nextAvailableTimeToStartTalk);
            nextAvailableTimeToStartTalk.RoundToWholeHour();

            var sessionEndTime = new ConferenceTime(_networkingSessionLatestStartHour);
                   
            if(nextAvailableTimeToStartTalk.HourAsTwentFourHourInt <= _networkingSessionEarliestStartHour)
            {
                sessionEndTime = new ConferenceTime(_networkingSessionEarliestStartHour);
            }

            _outputWriter.WriteLine($"{sessionEndTime.TimeAsTwelveHourString()} Networking Event");   
            _outputWriter.WriteLine(String.Empty);
        }
    }
}
