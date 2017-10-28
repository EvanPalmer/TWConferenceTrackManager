using System;
using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.Access;
using ThoughtWorks.ConferenceTrackManager.Factories;
using Moq;
using Xunit;

namespace ThoughtWorks.ConferenceTrackManager.Tests.Factories
{
    public class TalkFactoryTests
    {
        private List<string> fileContents = new List<string>{
            "Talkname 10mins",
            "Talkname 10mins",
            "Talkname 10mins"
        };

        [Fact]
        public void BuildTalkCollectionFromFile_BuildsNoTalks_WhenNoInput()
        {
            // Arrange
            var talkFactory = new TalkFactory();

            // Act
            var talks = talkFactory.BuildTalkCollectionFromInput(new List<string>());

            // Assert
            Assert.Equal(0, talks.Count);
        }

        [Fact]
        public void BuildTalkCollectionFromFile_BuildsThreeTalksFromInput()
        {
            // Arrange
            var talkFactory = new TalkFactory();

            // Act
            var talks = talkFactory.BuildTalkCollectionFromInput(fileContents);

            // Assert
            Assert.Equal(3, talks.Count);
        }
    }
}
