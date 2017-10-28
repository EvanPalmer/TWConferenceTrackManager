using System;
using System.Linq;
using System.Collections.Generic;
using ThoughtWorks.ConferenceTrackManager.App;
using ThoughtWorks.ConferenceTrackManager.Configuration;
using ThoughtWorks.ConferenceTrackManager.Models;
using Moq;
using Xunit;

namespace ThoughtWorks.ConferenceTrackManager.Tests.App
{
    public class ConferenceSessionBuilderTests
    {
        [Fact]
        public void CreateSessionsOrEmptyList_BuildsNoSessions_WhenNoTracksConfigured()
        {
            // Arrange
            var mockConfig = new Mock<IAppConfiguration>();
            mockConfig.Setup(c => c.NumberOfTracks).Returns(0);

            var conferenceSessionBuilder = new ConferenceSessionBuilder(mockConfig.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyList();

            // Assert
            Assert.Empty(sessions);
        }

        [Fact]
        public void CreateSessionsOrEmptyList_BuildsTwoSessions_WhenOneTracksConfigured()
        {
            // Arrange
            var mockConfig = new Mock<IAppConfiguration>();
            mockConfig.Setup(c => c.NumberOfTracks).Returns(1);

            var conferenceSessionBuilder = new ConferenceSessionBuilder(mockConfig.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyList();

            // Assert
            Assert.True(sessions.Count == 2);
        }

        [Fact]
        public void CreateSessionsOrEmptyList_BuildsFourSessions_WhenTwoTracksConfigured()
        {
            // Arrange
            var mockConfig = new Mock<IAppConfiguration>();
            mockConfig.Setup(c => c.NumberOfTracks).Returns(2);

            var conferenceSessionBuilder = new ConferenceSessionBuilder(mockConfig.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyList();

            // Assert
            Assert.True(sessions.Count == 4);
        }
        [Fact]
        public void CreateSessionsOrEmptyList_BuildsAMorningSession_WhenOneTracksConfigured()
        {
            // Arrange
            var mockConfig = new Mock<IAppConfiguration>();
            mockConfig.Setup(c => c.NumberOfTracks).Returns(2);

            var conferenceSessionBuilder = new ConferenceSessionBuilder(mockConfig.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyList();

            // Assert
            Assert.IsType(typeof(MorningConferenceSession), sessions.First());
        }


        [Fact]
        public void CreateSessionsOrEmptyList_BuildsAnAfternoonSession_WhenOneTracksConfigured()
        {
            // Arrange
            var mockConfig = new Mock<IAppConfiguration>();
            mockConfig.Setup(c => c.NumberOfTracks).Returns(2);

            var conferenceSessionBuilder = new ConferenceSessionBuilder(mockConfig.Object);

            // Act
            var sessions = conferenceSessionBuilder.CreateSessionsOrEmptyList();

            // Assert
            Assert.IsType(typeof(AfternoonConferenceSession), sessions.Last());
        }

        [Fact]
        public void SortTalks_OrdersSortsDecendingByTime()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object);
            var talks = new List<ITalk> {
                new Talk("Some short talk 1min"),
                new Talk("Some longer talk 10min"),
            };

            // Act
            var sortedTalks = conferenceSessionBuilder.SortTalks(talks);

            // Assert
            Assert.Equal(10, sortedTalks.First().LengthInMinutes);
        }

        [Fact]
        public void PopulateSessionsWithTalks_()
        {
            // Arrange
            var config = new Mock<IAppConfiguration>();
            config.Setup(c => c.NumberOfTracks).Returns(2);

            var conferenceSessionBuilder = new ConferenceSessionBuilder(config.Object);
            var talks = new List<Talk> {
                new Talk("A talk 10min"),
                new Talk("Some talk 10min"),
                new Talk("Some other talk 10min"),
                new Talk("Yet another talk 10min"),
            };
            var sessions = new List<IConferenceSession>();
            var mockSession1 = new Mock<IConferenceSession>();
            mockSession1.Setup(s => s.TryIncludeTalkInSession(It.IsAny<Talk>())).Returns(true);
            var mockSession2 = new Mock<IConferenceSession>();
            mockSession2.Setup(s => s.TryIncludeTalkInSession(It.IsAny<Talk>())).Returns(true);

            sessions.Add(mockSession1.Object);
            sessions.Add(mockSession2.Object);

            // Act
            //conferenceSessionBuilder.PopulateSessionsWithTalks(sessions, talks);

            // Assert
            //Assert.True(mockSession1.Object.);
            //TODO finish testing this shit. I'm too tired now
        }
    }
}
