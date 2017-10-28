using Xunit;
using Moq;
using ThoughtWorks.ConferenceTrackManager.App;
using ThoughtWorks.ConferenceTrackManager.Factories;
using ThoughtWorks.ConferenceTrackManager.Access;
using ThoughtWorks.ConferenceTrackManager.Models;
using System.Collections.Generic;

namespace ThoughtWorks.ConferenceTrackManager.Tests.App
{
    public class ConferenceManagerTests
    {
        [Fact]
        public void Run_TriesToParseArguments()
        {
            // Arrange
            var arguments = new Mock<IAppArgument>();
            var conferenceFactory = new Mock<IConferenceFactory>();
            var talkFactory = new Mock<ITalkFactory>();
            var fileSystem = new Mock<IFileSystem>();

            var conferenceManager = new ConferenceManager(arguments.Object, conferenceFactory.Object, talkFactory.Object, fileSystem.Object);

            // Act
            conferenceManager.Run(new string[]{});

            // Assert
            var filename = string.Empty;
            arguments.Verify(a => a.TryParse(It.IsAny<string[]>(), out filename), Times.Once());
        }

        [Fact]
        public void Run_ReadsFromFileSystem_WhenArgumentsAreValid()
        {
            // Arrange
            var arguments = new Mock<IAppArgument>();
            var filename = It.IsAny<string>();
            arguments.Setup(a => a.TryParse(It.IsAny<string[]>(), out filename)).Returns(true);
            var conferenceFactory = new Mock<IConferenceFactory>();
            conferenceFactory.Setup(cf => cf.Build(It.IsAny<IList<ITalk>>())).Returns(new Mock<IConference>().Object);
            var talkFactory = new Mock<ITalkFactory>();
            var fileSystem = new Mock<IFileSystem>();

            var conferenceManager = new ConferenceManager(arguments.Object, conferenceFactory.Object, talkFactory.Object, fileSystem.Object);

            // Act
            conferenceManager.Run(new string[] { });

            // Assert
            fileSystem.Verify(fs => fs.ReadFileAsStringListOrEmptyList(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void Run_DoesNotReadFromFileSystem_WhenArgumentsAreNotValid()
        {
            // Arrange
            var arguments = new Mock<IAppArgument>();
            var filename = It.IsAny<string>();
            arguments.Setup(a => a.TryParse(It.IsAny<string[]>(), out filename)).Returns(false);
            var conferenceFactory = new Mock<IConferenceFactory>();
            var talkFactory = new Mock<ITalkFactory>();
            var fileSystem = new Mock<IFileSystem>();

            var conferenceManager = new ConferenceManager(arguments.Object, conferenceFactory.Object, talkFactory.Object, fileSystem.Object);

            // Act
            conferenceManager.Run(new string[] { });

            // Assert
            fileSystem.Verify(fs => fs.ReadFileAsStringListOrEmptyList(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void Run_BuildsTalks_WhenArgumentsAreValid()
        {
            // Arrange
            var arguments = new Mock<IAppArgument>();
            var filename = It.IsAny<string>();
            arguments.Setup(a => a.TryParse(It.IsAny<string[]>(), out filename)).Returns(true);
            var conferenceFactory = new Mock<IConferenceFactory>();
            conferenceFactory.Setup(cf => cf.Build(It.IsAny<IList<ITalk>>())).Returns(new Mock<IConference>().Object);
            var talkFactory = new Mock<ITalkFactory>();
            var fileSystem = new Mock<IFileSystem>();

            var conferenceManager = new ConferenceManager(arguments.Object, conferenceFactory.Object, talkFactory.Object, fileSystem.Object);

            // Act
            conferenceManager.Run(new string[] { });

            // Assert
            talkFactory.Verify(tf => tf.BuildTalkCollectionFromInput(It.IsAny<List<string>>()), Times.Once);
        }

        [Fact]
        public void Run_DoesNotBuildTalks_WhenArgumentsAreNotValid()
        {
            // Arrange
            var arguments = new Mock<IAppArgument>();
            var filename = It.IsAny<string>();
            arguments.Setup(a => a.TryParse(It.IsAny<string[]>(), out filename)).Returns(false);
            var conferenceFactory = new Mock<IConferenceFactory>();
            var talkFactory = new Mock<ITalkFactory>();
            var fileSystem = new Mock<IFileSystem>();

            var conferenceManager = new ConferenceManager(arguments.Object, conferenceFactory.Object, talkFactory.Object, fileSystem.Object);

            // Act
            conferenceManager.Run(new string[] { });

            // Assert
            talkFactory.Verify(tf => tf.BuildTalkCollectionFromInput(It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public void Run_BuildsConference_WhenArgumentsAreValid()
        {
            // Arrange
            var arguments = new Mock<IAppArgument>();
            var filename = It.IsAny<string>();
            arguments.Setup(a => a.TryParse(It.IsAny<string[]>(), out filename)).Returns(true);
            var conferenceFactory = new Mock<IConferenceFactory>();
            conferenceFactory.Setup(cf => cf.Build(It.IsAny<IList<ITalk>>())).Returns(new Mock<IConference>().Object);
            var talkFactory = new Mock<ITalkFactory>();
            var fileSystem = new Mock<IFileSystem>();

            var conferenceManager = new ConferenceManager(arguments.Object, conferenceFactory.Object, talkFactory.Object, fileSystem.Object);

            // Act
            conferenceManager.Run(new string[] { });

            // Assert
            conferenceFactory.Verify(cf => cf.Build(It.IsAny<IList<ITalk>>()));
        }

        [Fact]
        public void Run_DoesNotBuildConference_WhenArgumentsAreNotValid()
        {
            // Arrange
            var arguments = new Mock<IAppArgument>();
            var filename = It.IsAny<string>();
            arguments.Setup(a => a.TryParse(It.IsAny<string[]>(), out filename)).Returns(false);
            var conferenceFactory = new Mock<IConferenceFactory>();
            var talkFactory = new Mock<ITalkFactory>();
            var fileSystem = new Mock<IFileSystem>();

            var conferenceManager = new ConferenceManager(arguments.Object, conferenceFactory.Object, talkFactory.Object, fileSystem.Object);

            // Act
            conferenceManager.Run(new string[] { });

            // Assert
            talkFactory.Verify(tf => tf.BuildTalkCollectionFromInput(It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public void Run_PrintsConference_WhenArgumentsAreValid()
        {
            // Arrange
            var arguments = new Mock<IAppArgument>();
            var filename = It.IsAny<string>();
            arguments.Setup(a => a.TryParse(It.IsAny<string[]>(), out filename)).Returns(true);
            var conferenceFactory = new Mock<IConferenceFactory>();
            var conference = new Mock<IConference>();
            conferenceFactory.Setup(cf => cf.Build(It.IsAny<IList<ITalk>>())).Returns(conference.Object);
            var talkFactory = new Mock<ITalkFactory>();
            var fileSystem = new Mock<IFileSystem>();

            var conferenceManager = new ConferenceManager(arguments.Object, conferenceFactory.Object, talkFactory.Object, fileSystem.Object);

            // Act
            conferenceManager.Run(new string[] { });

            // Assert
            conference.Verify(c => c.Print(), Times.Once);
        }

        [Fact]
        public void Run_DoesNotPrintConference_WhenArgumentsAreNotValid()
        {
            // Arrange
            var arguments = new Mock<IAppArgument>();
            var filename = It.IsAny<string>();
            arguments.Setup(a => a.TryParse(It.IsAny<string[]>(), out filename)).Returns(false);
            var conferenceFactory = new Mock<IConferenceFactory>();
            var conference = new Mock<IConference>();
            conferenceFactory.Setup(cf => cf.Build(It.IsAny<IList<ITalk>>())).Returns(conference.Object);

            var talkFactory = new Mock<ITalkFactory>();
            var fileSystem = new Mock<IFileSystem>();

            var conferenceManager = new ConferenceManager(arguments.Object, conferenceFactory.Object, talkFactory.Object, fileSystem.Object);

            // Act
            conferenceManager.Run(new string[] { });

            // Assert
            conference.Verify(c => c.Print(), Times.Never);
        }

    }
}
