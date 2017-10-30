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
    public class ConferenceSessionCreatorTests
    {
        [Fact]
        public void CreateSessionsOrEmptyListFromConfig_BuildsNoSessions_WhenNoTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(0);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionCreator = new ConferenceSessionCreator(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionCreator.CreateSessionsOrEmptyListFromConfig();

            // Assert
            Assert.Empty(sessions);
        }

        [Fact]
        public void CreateSessionsOrEmptyListFromConfig_BuildsTwoSessions_WhenOneTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(1);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionCreator = new ConferenceSessionCreator(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionCreator.CreateSessionsOrEmptyListFromConfig();

            // Assert
            Assert.True(sessions.Count == 2);
        }

        [Fact]
        public void CreateSessionsOrEmptyListFromConfig_BuildsFourSessions_WhenTwoTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(2);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionCreator = new ConferenceSessionCreator(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionCreator.CreateSessionsOrEmptyListFromConfig();

            // Assert
            Assert.True(sessions.Count == 4);
        }

        [Fact]
        public void CreateSessionsOrEmptyListFromConfig_BuildsAMorningSession_WhenOneTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(2);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionCreator = new ConferenceSessionCreator(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionCreator.CreateSessionsOrEmptyListFromConfig();

            // Assert
            conferenceSessionFactory.Verify(csf => csf.CreateMorningConferenceSession(It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        public void CreateSessionsOrEmptyListFromConfig_BuildsAnAfternoonSession_WhenOneTracksConfigured()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(2);
            var conferenceSessionFactory = new Mock<IConferenceSessionFactory>();
            var conferenceSessionCreator = new ConferenceSessionCreator(config.Object, conferenceSessionFactory.Object);

            // Act
            var sessions = conferenceSessionCreator.CreateSessionsOrEmptyListFromConfig();

            // Assert
            conferenceSessionFactory.Verify(csf => csf.CreateAfternoonConferenceSession(), Times.Exactly(2));
        }
    }
}
