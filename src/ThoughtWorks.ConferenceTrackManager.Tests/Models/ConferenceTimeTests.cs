using System;
using ThoughtWorks.ConferenceTrackManager.Models;
using Xunit;

namespace ThoughtWorks.ConferenceTrackManager.Tests.Models
{
    public class ConferenceTimeTests
    {
        [Fact]
        public void HourAsTwentFourHourInt_GetsMidnight_AsZero()
        {
            // Arrange
            var time = new ConferenceTime(0);

            // Act
            var midnight = time.HourAsTwentFourHourInt;

            // Assert
            Assert.Equal(0, midnight);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(6, 6)]
        [InlineData(12, 12)]
        [InlineData(18, 18)]
        [InlineData(0, 0)]
        public void HourAsTwentFourHourInt_GetsHour_As24HourInt(int inputNumber, int expect24Hour)
        {
            // Arrange
            var time = new ConferenceTime(inputNumber    );

            // Act
            var result = time.HourAsTwentFourHourInt;

            // Assert
            Assert.Equal(expect24Hour, result);
        }

        public void Constructor_ThrowsException_WhenOutOfRange()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ConferenceTime(24));
        }

        [Theory]
        [InlineData(1, "01:00AM")]
        [InlineData(6, "06:00AM")]
        [InlineData(12, "12:00PM")]
        [InlineData(18, "06:00PM")]
        [InlineData(0, "12:00AM")]
        public void TimeAsTwelveHourString_GetsHour_As12HourInt(int inputNumber, string expect12HourString)
        {
            // Arrange
            var time = new ConferenceTime(inputNumber);

            // Act
            var result = time.TimeAsTwelveHourString();

            // Assert
            Assert.Equal(expect12HourString, result);
        }

        [Fact]
        public void AddMinutes_Adds_OneHour()
        {
            // Arrange
            var time = new ConferenceTime(0);

            // Act
            time.AddMinutes(60);

            // Assert
            Assert.Equal("01:00AM", time.TimeAsTwelveHourString());
        }

        [Fact]
        public void AddMinutes_Adds_ThirtyMinutes()
        {
            // Arrange
            var time = new ConferenceTime(0);

            // Act
            time.AddMinutes(30);

            // Assert
            Assert.Equal("12:30AM", time.TimeAsTwelveHourString());
        }

        [Fact]
        public void AddMinutes_Adds_OverAnHour()
        {
            // Arrange
            var time = new ConferenceTime(0);

            // Act
            time.AddMinutes(70);

            // Assert
            Assert.Equal("01:10AM", time.TimeAsTwelveHourString());
        }

        [Fact]
        public void RoundToWholeHour_DoesntRound_WhenMinutesAreZero()
        {
            // Arrange
            var time = new ConferenceTime(1);

            // Act
            time.RoundToWholeHour();    

            // Assert
            Assert.Equal(1, time.HourAsTwentFourHourInt);
        }

        [Fact]
        public void RoundToWholeHour_Rounds_WhenMinutesAreGreaterThanZero()
        {
            // Arrange
            var time = new ConferenceTime(1);
            time.AddMinutes(1);

            // Act
            time.RoundToWholeHour();

            // Assert
            Assert.Equal(2, time.HourAsTwentFourHourInt);
        }

        [Fact]
        public void ToDateTime_ReturnsDateTime()
        {
            // Arrange
            var time = new ConferenceTime(1);

            // Act
            var date = time.ToDateTime();

            // Assert
            Assert.Equal(1, date.Hour);
            Assert.Equal(0, date.Minute);
        }

        [Fact]
        public void IsLessThanOrEqualTo_ReturnsTrue_WhenLessThan()
        {
            // Arrange
            var timeLow = new ConferenceTime(1);
            var timeHigh = new ConferenceTime(2);

            // Act
            var isLessThanOrEqualTo = timeLow.IsLessThanOrEqualTo(timeHigh);

            // Assert
            Assert.True(isLessThanOrEqualTo);
        }

        [Fact]
        public void IsLessThanOrEqualTo_ReturnsTrue_WhenLessEqualTo()
        {
            // Arrange
            var time = new ConferenceTime(1);

            // Act
            var isLessThanOrEqualTo = time.IsLessThanOrEqualTo(time);

            // Assert
            Assert.True(isLessThanOrEqualTo);
        }

        [Fact]
        public void IsLessThanOrEqualTo_ReturnsFalse_WhenGreaterThan()
        {
            // Arrange
            var timeLow = new ConferenceTime(1);
            var timeHigh = new ConferenceTime(2);

            // Act
            var isLessThanOrEqualTo = timeHigh.IsLessThanOrEqualTo(timeLow);

            // Assert
            Assert.False(isLessThanOrEqualTo);
        }
    }
}
