using CareFlow.Domain.Exceptions;
using CareFlow.Domain.ValueObjects;
using FluentAssertions;

namespace CareFlow.Tests.Domain;

public class DateRangeTests
{
    [Fact]
    public void Constructor_ShouldThrowException_WhenEndIsBeforeStart()
    {
        // Arrange
        var start = DateTime.UtcNow;
        var end = start.AddHours(-1);

        // Act
        Action act = () => new DateRange(start, end);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("End date must be after start date.");
    }

    [Fact]
    public void Overlaps_ShouldReturnTrue_WhenRangesOverlap()
    {
        // Arrange
        var start1 = DateTime.UtcNow;
        var end1 = start1.AddHours(2);
        var range1 = new DateRange(start1, end1);

        var start2 = start1.AddHours(1);
        var end2 = start2.AddHours(2);
        var range2 = new DateRange(start2, end2);

        // Act
        var result = range1.Overlaps(range2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Overlaps_ShouldReturnFalse_WhenRangesDoNotOverlap()
    {
        // Arrange
        var start1 = DateTime.UtcNow;
        var end1 = start1.AddHours(2);
        var range1 = new DateRange(start1, end1);

        var start2 = end1.AddHours(1);
        var end2 = start2.AddHours(2);
        var range2 = new DateRange(start2, end2);

        // Act
        var result = range1.Overlaps(range2);

        // Assert
        result.Should().BeFalse();
    }
}
