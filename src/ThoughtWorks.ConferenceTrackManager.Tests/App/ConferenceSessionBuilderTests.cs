using System;
using System.Linq;
using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.App;
using ThoughtWorks.ConferenceTrackManager.Configuration;
using ThoughtWorks.ConferenceTrackManager.Models;
using Moq;
using Xunit;
using ThoughtWorks.ConferenceTrackManager.Factories;

namespace ThoughtWorks.ConferenceTrackManager.Tests.App
{
    public class ConferenceSessionBuilderTests
    {
        [Fact]
        public void CreateSessionsOrEmptyList_BuildsNoSessions_WhenNoTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(0);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
			var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyListFromConfig();

            // Assert
            Assert.Empty(sessions);
        }

        [Fact]
        public void CreateSessionsOrEmptyList_BuildsTwoSessions_WhenOneTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(1);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyListFromConfig();

            // Assert
            Assert.True(sessions.Count == 2);
        }

        [Fact]
        public void CreateSessionsOrEmptyList_BuildsFourSessions_WhenTwoTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(2);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyListFromConfig();

            // Assert
            Assert.True(sessions.Count == 4);
        }

        [Fact]
        public void CreateSessionsOrEmptyList_BuildsAMorningSession_WhenOneTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(2);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyListFromConfig();

            // Assert
            conferenceSessionFactory.Verify(csf => csf.CreateMorningConferenceSession(It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        public void CreateSessionsOrEmptyList_BuildsAnAfternoonSession_WhenOneTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(2);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyListFromConfig();

            // Assert
            conferenceSessionFactory.Verify(csf => csf.CreateAfternoonConferenceSession(), Times.Exactly(2));
        }

        [Fact]
        public void DistributeTalksAcrossSessions_SortsTalks_DecendingByLength()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object, conferenceSessionFactory.Object);
            var talks = new List<ITalk> {
                new Talk("Some short talk 1min"),
                new Talk("Some longer talk 10min"),
            };
            var sessions = new List<IConferenceSession>();
            var session1 = new Mock<IConferenceSession>();

            ITalk firstTalkPlaced = null;
            session1.Setup(s => s.TryIncludeTalkInSession(It.IsAny<Talk>())).Returns(true).Callback((ITalk t) => firstTalkPlaced = t);;
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
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(2);

            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object, conferenceSessionFactory.Object);
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
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(2);

            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object, conferenceSessionFactory.Object);
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
