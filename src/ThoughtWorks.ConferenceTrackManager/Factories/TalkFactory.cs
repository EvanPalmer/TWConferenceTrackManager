using System;
using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.Models;

namespace ThoughtWorks.ConferenceTrackManager.Factories
{
    public interface ITalkFactory
    {
        List<ITalk> BuildTalkCollectionFromInput(List<string> input);
    }

    public class TalkFactory : ITalkFactory
    {
        public List<ITalk> BuildTalkCollectionFromInput(List<string> talkDefinitionsList)
        {
            var allTalks = new List<ITalk>();

            foreach (var talkStringDefinition in talkDefinitionsList)
            {
                var talk = new Talk(talkStringDefinition);
                allTalks.Add(talk);
            }

            return allTalks;
        }
    }
}
