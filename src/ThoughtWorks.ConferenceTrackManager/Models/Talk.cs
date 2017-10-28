using System;
using System.Text.RegularExpressions;
using ThoughtWorks.ConferenceTrackManager.Exceptions;

namespace ThoughtWorks.ConferenceTrackManager.Models
{
    public interface ITalk
    {
        int LengthInMinutes { get; }
        string Name { get; }
        string TalkDefinition { get; }
    }

    public class Talk : ITalk
    {
        private const string _inputStringFormat = "(\\D+) (lightning|\\d+)";
        private readonly Regex _regex = new Regex(_inputStringFormat);
        private const int _lengthOfLightningTalkInMinutes = 5;

        public string Name
        {
            get
            {
                var match = _regex.Match(_talkDefinition);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
                throw new ConferenceSetUpException($"Input formed poorly. Cannot parse {_talkDefinition}");
            }
        }

        public int LengthInMinutes
        {
            get
            {
                var match = _regex.Match(_talkDefinition);
                if (match.Success)
                {
                    var lengthInMinutesString = match.Groups[2].Value;

                    var lengthInMinutes = 0;
                    var inputLengthAsNumber = Int32.TryParse(lengthInMinutesString, out lengthInMinutes);
                    if (inputLengthAsNumber)
                    {
                        return lengthInMinutes;
                    }
                    else
                    {
                        return _lengthOfLightningTalkInMinutes;
                    }
                }
                throw new ConferenceSetUpException($"Input formed poorly. Cannot parse {_talkDefinition}");
            }
        }

        private string _talkDefinition;
        public string TalkDefinition
        {
            get 
            {
                return _talkDefinition;
            }
        }

        public Talk(string talkDefinition)
        {
            ValidateTalkDefinition(talkDefinition);
        }

        private void ValidateTalkDefinition(string talkDefinition)
        {
            if (string.IsNullOrWhiteSpace(talkDefinition))
            {
                throw new ConferenceSetUpException($"Input formed poorly. Cannot parse null or white space.");
            }
            var match = _regex.Match(talkDefinition);
            if (match.Success)
            {
                _talkDefinition = talkDefinition;
            }
            else
            {
                throw new ConferenceSetUpException($"Input formed poorly. Cannot parse {_talkDefinition}");
            }
        }
    }
}