using System;
using ThoughtWorks.ConferenceTrackManager.Exceptions;
using ThoughtWorks.ConferenceTrackManager.Models;
using Xunit;

namespace ThoughtWorks.ConferenceTrackManager.Tests.Models
{
    public class TalkTests
    {
        [Fact]
        public void Name_ReturnsName_WhenInputWellFormedWithNumber()
        {
            // Arrange
            var inputString = "Name of talk 10min";
            var talk = new Talk(inputString);

            // Act
            var result = talk.Name;

            // Assert
            Assert.Equal("Name of talk", result);
        }

        [Fact]
        public void LengthInMinutes_ReturnsLengthInMinutes_WhenInputWellFormedWithNumber()
        {
            // Arrange
            var inputString = "Name of talk 10min";
            var talk = new Talk(inputString);

            // Act
            var result = talk.LengthInMinutes;

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void Name_ReturnsName_WhenInputWellFormedWithLightning()
        {
            // Arrange
            var inputString = "Name of talk lightning";
            var talk = new Talk(inputString);

            // Act
            var result = talk.Name;

            // Assert
            Assert.Equal("Name of talk", result);
        }

        [Fact]
        public void LengthInMinutes_ReturnsLengthInMinutes_WhenInputWellFormedWithLightning()
        {
            // Arrange
            var inputString = "Name of talk lightning";
            var talk = new Talk(inputString);

            // Act
            var result = talk.LengthInMinutes;

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenInputPoorlyFormed()
        {
            // Arrange
            var inputString = "This talk is poorly formed";

            // Act, Assert
            Assert.Throws<ConferenceSetUpException>(() => new Talk(inputString));
        }

        [Fact]
        public void Constructor_ThrowsException_WhenInputEmpty()
        {
            // Arrange
            var inputString = string.Empty;

            // Act, Assert
            Assert.Throws<ConferenceSetUpException>(() => new Talk(inputString));
        }

        [Fact]
        public void Constructor_ThrowsException_WhenInputNull()
        {
            // Arrange
            string inputString = null;

            // Act, Assert
            Assert.Throws<ConferenceSetUpException>(() => new Talk(inputString));
        }
    }
}
