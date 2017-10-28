using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Access;
using Moq;
using Xunit;

namespace ThoughtWorks.ConferenceTrackManager.Tests.Models
{
    public class ArgumentTests
    {
        [Fact]
        public void TryParse_ReturnsFalse_WhenArgsNull()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var arguments = new AppArgument(outputWriter.Object);
            var filename = string.Empty;

            // Act
            var result = arguments.TryParse(null, out filename);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryParse_ReturnsFalse_WhenNoArguments()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var arguments = new AppArgument(outputWriter.Object);
            var filename = string.Empty;

            // Act
            var result = arguments.TryParse(null, out filename);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void TryParse_ReturnsTrue_WhenThereIsAnArgument()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var arguments = new AppArgument(outputWriter.Object);
            var filename = string.Empty;

            // Act
            var result = arguments.TryParse(new string[] { "any parameter" }, out filename);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TryParse_SetsFilename_WhenThereIsAnArgument()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var arguments = new AppArgument(outputWriter.Object);
            var filename = string.Empty;

            // Act
            arguments.TryParse(new string[] { "the filename" }, out filename);

            // Assert
            Assert.Equal("the filename", filename);
        }

        [Fact]
        public void TryParse_WritesUsage_WhenAregumentsInvalid()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var arguments = new AppArgument(outputWriter.Object);
            var filename = string.Empty;
            string[] invalidArguments = null;

            // Act
            arguments.TryParse(invalidArguments, out filename);

            // Assert
            outputWriter.Verify(o => o.WriteLine("usage: dotnet run [filename]"), Times.Once());
        }
    }
}
