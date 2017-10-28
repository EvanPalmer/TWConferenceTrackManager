using System;
namespace ThoughtWorks.ConferenceTrackManager.Exceptions
{
    public class ConferenceSetUpException : Exception
    {
        public ConferenceSetUpException() : base()
        {
        }

        public ConferenceSetUpException(string message)
            : base(message)
        {
        }

        public ConferenceSetUpException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
