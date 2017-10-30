using System.Collections.Generic;
using Xunit;
using Moq;
using ThoughtWorks.ConferenceTrackManager.App;
using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Access;

namespace ThoughtWorks.ConferenceTrackManager.Tests.Models
{
    public class ConferenceTests
    {
        [Fact]
        public void Print_BuildsSessions()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionCreator = new Mock<IConferenceSessionCreator>();
            sessionCreator.Setup(sb => sb.CreateSessionsOrEmptyListFromConfig()).Returns(new List<IConferenceSession>());
            var talkDistributor = new Mock<ITalkDistributor>();
            talkDistributor.Setup(sb => sb.DistributeTalksAcrossSessions(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(true);
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionCreator.Object, outputWriter.Object, talkDistributor.Object);

            // Act
            conference.Print();

            // Assert
            sessionCreator.Verify(sb => sb.CreateSessionsOrEmptyListFromConfig(), Times.Once());
        }

        [Fact]
        public void Print_PopulatesSessions()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionCreator = new Mock<IConferenceSessionCreator>();
            var sessions = new List<IConferenceSession> { new Mock<IConferenceSession>().Object, new Mock<IConferenceSession>().Object };
            sessionCreator.Setup(sb => sb.CreateSessionsOrEmptyListFromConfig()).Returns(sessions);
            var talkDistributor = new Mock<ITalkDistributor>();
            talkDistributor.Setup(sb => sb.DistributeTalksAcrossSessions(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(true);
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionCreator.Object, outputWriter.Object, talkDistributor.Object);

            // Act
            conference.Print();

            // Assert
            talkDistributor.Verify(sb => sb.DistributeTalksAcrossSessions(sessions, It.IsAny<IList<ITalk>>()), Times.Once());
        }

        [Fact]
        public void Print_PrintsErrorMessage_WhenPopulationFails()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionCreator = new Mock<IConferenceSessionCreator>();
            var sessions = new List<IConferenceSession> { new Mock<IConferenceSession>().Object, new Mock<IConferenceSession>().Object };
            sessionCreator.Setup(sb => sb.CreateSessionsOrEmptyListFromConfig()).Returns(sessions);
            var talkDistributor = new Mock<ITalkDistributor>();
            talkDistributor.Setup(sb => sb.DistributeTalksAcrossSessions(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(false);
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionCreator.Object, outputWriter.Object, talkDistributor.Object);

            // Act
            conference.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("Didn't add at least one of the talks!\nHere's the best I could do:\n"), Times.Once());
        }

        [Fact]
        public void Print_DoesNotPrintErrorMessage_WhenPopulationSucceeds()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionCreator = new Mock<IConferenceSessionCreator>();
            var sessions = new List<IConferenceSession> { new Mock<IConferenceSession>().Object, new Mock<IConferenceSession>().Object };
            sessionCreator.Setup(sb => sb.CreateSessionsOrEmptyListFromConfig()).Returns(sessions);
            var talkDistributor = new Mock<ITalkDistributor>();
            talkDistributor.Setup(sb => sb.DistributeTalksAcrossSessions(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(true);
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionCreator.Object, outputWriter.Object, talkDistributor.Object);

            // Act
            conference.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("Didn't add at least one of the talks!\nHere's the best I could do:\n"), Times.Never());
        }

        [Fact]
        public void Print_PrintsEachSession()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionCreator = new Mock<IConferenceSessionCreator>();
            var sessions = new List<IConferenceSession>();
            var firstSession = new Mock<IConferenceSession>();
            sessions.Add(firstSession.Object);
            var lastSession = new Mock<IConferenceSession>();
            sessions.Add(lastSession.Object);
            sessionCreator.Setup(sb => sb.CreateSessionsOrEmptyListFromConfig()).Returns(sessions);
            var talkDistributor = new Mock<ITalkDistributor>();
            talkDistributor.Setup(sb => sb.DistributeTalksAcrossSessions(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(true);
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionCreator.Object, outputWriter.Object, talkDistributor.Object);

            // Act
            conference.Print();

            // Assert
            firstSession.Verify(s => s.Print(), Times.Once());
            lastSession.Verify(s => s.Print(), Times.Once());
        }
    }
}
