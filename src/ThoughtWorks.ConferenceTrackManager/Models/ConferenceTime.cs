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

        public int HourAsTwentFourHourInt
        {
            get
            {
                return _startTime.Hour;
            }
        }   

        public void AddMinutes(int minutesToAdd)
        {
            _startTime = _startTime.AddMinutes(minutesToAdd);
        }


        public void SubtractMinutes(int minutesToAdd)
        {
            _startTime = _startTime.AddMinutes(minutesToAdd * -1);
        }

        public void RoundToWholeHour()
        {
            const int minutesInOneHour = 60;
            if(_startTime.Minute > 0)  
            {
                _startTime = _startTime.AddMinutes(minutesInOneHour - _startTime.Minute);
            }
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
