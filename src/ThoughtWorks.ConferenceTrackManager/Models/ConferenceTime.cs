using System;
namespace ThoughtWorks.ConferenceTrackManager.Models
{
    public class ConferenceTime
    {
        DateTime _startTime;

        public ConferenceTime(int startHour)
        {
            _startTime = new DateTime(2000, 1, 1, startHour, 0, 0);
        }

        public ConferenceTime(DateTime date)
        {
            _startTime = date;
        }

        public int HourAsTwentyFourHourInt
        {
            get
            {
                return _startTime.Hour;
            }
        }   

        public ConferenceTime AddMinutes(int minutesToAdd)
        {
            var minutesAdded = _startTime.AddMinutes(minutesToAdd);
            var conferenceTime = new ConferenceTime(minutesAdded);
            return conferenceTime;
        }

        public ConferenceTime SubtractMinutes(int minutesToAdd)
        {
            var minutesSubtracted = _startTime.AddMinutes(minutesToAdd * -1);
            var conferenceTime = new ConferenceTime(minutesSubtracted);
            return conferenceTime;
        }

        public ConferenceTime RoundToWholeHour()
        {
            const int minutesInOneHour = 60;
            var minutesAdded = _startTime;
            if(_startTime.Minute > 0)  
            {
                minutesAdded = _startTime.AddMinutes(minutesInOneHour - _startTime.Minute);
            }
            var conferenceTime = new ConferenceTime(minutesAdded);
            return conferenceTime;
        }   

        public string TimeAsTwelveHourString()
        {
            //todo format properly, without upper
            return _startTime.ToString("hh:mmtt").ToUpper();
        }

        public DateTime ToDateTime()
        {
            return _startTime;
        }

        public bool IsLessThanOrEqualTo(ConferenceTime otherTime)
        {
            return _startTime <= otherTime.ToDateTime();
        }
    }   
}
