using System;
using System.Collections.Generic;
using Moq;
using ThoughtWorks.ConferenceTrackManager.App;
using ThoughtWorks.ConferenceTrackManager.Models;
using Xunit;

namespace ThoughtWorks.ConferenceTrackManager.Tests.App
{
    public class TalkDistributionTests
    {
        [Fact]
        public void DistributeTalksAcrossSessions_SortsTalks_DecendingByLength()
        {
            // Arrange
            var conferenceSessionBuilder = new TalkDistributor();
            var talks = new List<ITalk> {
                new Talk("Some short talk 1min"),
                new Talk("Some longer talk 10min"),
            };
            var sessions = new List<IConferenceSession>();
            var session1 = new Mock<IConferenceSession>();

            ITalk firstTalkPlaced = null;
            session1.Setup(s => s.TryIncludeTalkInSession(It.IsAny<Talk>())).Returns(true).Callback((ITalk t) => firstTalkPlaced = t); ;
            sessions.Add(session1.Object);

            ITalk secondTalkPlaced = null;
            var session2 = new Mock<IConferenceSession>();
            session2.Setup(s => s.TryIncludeTalkInSession(It.IsAny<Talk>())).Returns(true).Callback((ITalk t) => secondTalkPlaced = t);
            sessions.Add(session2.Object);


            // Act
            var sortedTalks = conferenceSessionBuilder.DistributeTalksAcrossSessions(sessions, talks);

            // Assert
            Assert.Equal("Some longer talk 10min", firstTalkPlaced.TalkDefinition);
            Assert.Equal("Some short talk 1min", secondTalkPlaced.TalkDefinition);
        }

        [Fact]
        public void DistributeTalksAcrossSessions_IncludesAllTalksInSession_WhenTryIncludeSucceeds()
        {
            // Arrange
            var conferenceSessionBuilder = new TalkDistributor();
            var talks = new List<ITalk> {
                new Talk("A talk 10min"),
                new Talk("Some talk 10min"),
                new Talk("Some other talk 10min"),
                new Talk("Yet another talk 10min"),
            };
            var sessions = new List<IConferenceSession>();
            var mockSession1 = new Mock<IConferenceSession>();
            mockSession1.Setup(s => s.TryIncludeTalkInSession(It.IsAny<Talk>())).Returns(true);

            sessions.Add(mockSession1.Object);

            // Act
            conferenceSessionBuilder.DistributeTalksAcrossSessions(sessions, talks);

            // Assert
            mockSession1.Verify(s => s.TryIncludeTalkInSession(It.IsAny<ITalk>()), Times.Exactly(4));
        }

        [Fact]
        public void DistributeTalksAcrossSessions_IncludesAllTalksInSession_WhenTryIncludeFailsOnFirst()
        {
            // Arrange
            var conferenceSessionBuilder = new TalkDistributor();
            var talks = new List<ITalk> {
                new Talk("A talk 10min"),
                new Talk("Some talk 10min"),
                new Talk("Some other talk 10min"),
                new Talk("Yet another talk 10min"),
            };

            var sessions = new List<IConferenceSession>();
            var mockSession1 = new Mock<IConferenceSession>();
            mockSession1.Setup(s => s.TryIncludeTalkInSession(It.IsAny<Talk>())).Returns(false);
            sessions.Add(mockSession1.Object);

            var mockSession2 = new Mock<IConferenceSession>();
            mockSession2.Setup(s => s.TryIncludeTalkInSession(It.IsAny<Talk>())).Returns(true);
            sessions.Add(mockSession2.Object);

            // Act
            conferenceSessionBuilder.DistributeTalksAcrossSessions(sessions, talks);

            // Assert
            mockSession2.Verify(s => s.TryIncludeTalkInSession(It.IsAny<ITalk>()), Times.Exactly(4));
        }

    }
}
