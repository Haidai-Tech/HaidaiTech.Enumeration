using System.Text.Json;
using System.Xml.Serialization;

namespace HaidaiTech.Enumeration.Tests;


public class EnumerationTest
{
    private class SampleEnumeration : Enumeration
    {
        public static readonly SampleEnumeration First = new SampleEnumeration(1, "First");
        public static readonly SampleEnumeration Second = new SampleEnumeration(2, "Second");
        public SampleEnumeration(int id, string name) : base(id, name) { }
    }

    private class SampleEnumerationWithProtectedValue : Enumeration
    {
        public static readonly SampleEnumerationWithProtectedValue First = new SampleEnumerationWithProtectedValue(1, "First", "ProtectedValueOfFirst");
        public static readonly SampleEnumerationWithProtectedValue Second = new SampleEnumerationWithProtectedValue(2, "Second", "ProtectedValueOfSecond");


        public SampleEnumerationWithProtectedValue(int id, string name, string protectedValue) : base(id, name, protectedValue) { }
    }

    [Fact]
    public void ProtectedValue_ShouldBeSetCorrectly()
    {
        // Arrange
        var enumeration = new SampleEnumerationWithProtectedValue(1, "First", "ProtectedValueOfFirst");

        // Act
        var protectedValue = enumeration.ProtectedValue;

        // Assert
        Assert.Equal("ProtectedValueOfFirst", protectedValue);
    }

    [Fact]
    public void ProtectedValue_ShouldNotBeSerialized()
    {
        // Arrange
        var enumeration = new SampleEnumerationWithProtectedValue(1, "First", "ProtectedValueOfFirst");

        // Act
        var json = JsonSerializer.Serialize(enumeration);

        // Assert
        Assert.DoesNotContain("ProtectedValueOfFirst", json);
    }


    [Fact]
    public void ToDto_ShouldReturnCorrectDto()
    {
        // Arrange
        var enumeration = new SampleEnumeration(1, "First");

        // Act
        var dto = enumeration.ToDto();

        // Assert
        Assert.Equal(enumeration.Id, dto.Id);
        Assert.Equal(enumeration.Name, dto.Name);
    }



    [Fact]
    public void GetAll_ShouldReturnAllEnumerationValues()
    {
        // Act
        var allValues = Enumeration.GetAll<SampleEnumeration>().ToList();

        // Assert
        Assert.Equal(2, allValues.Count);
        Assert.Contains(SampleEnumeration.First, allValues);
        Assert.Contains(SampleEnumeration.Second, allValues);
    }
    [Fact]
    public void GetAll_ShouldReturnAllEnumerationWithProtectedValues()
    {
        // Act
        var allValues = Enumeration.GetAll<SampleEnumerationWithProtectedValue>().ToList();

        // Assert
        Assert.Equal(2, allValues.Count);
        Assert.Contains(SampleEnumerationWithProtectedValue.First, allValues);
        Assert.Contains(SampleEnumerationWithProtectedValue.Second, allValues);
    }

    [Fact]
    public void FromId_ShouldReturnCorrectEnumerationWithProtectedValue()
    {
        // Act
        var result = Enumeration.FromId<SampleEnumerationWithProtectedValue>(1);

        // Assert
        Assert.Equal(SampleEnumerationWithProtectedValue.First, result);
    }

    [Fact]
    public void FromName_ShouldReturnCorrectEnumerationWithProtectedValue()
    {
        // Act
        var result = Enumeration.FromName<SampleEnumerationWithProtectedValue>("Second");

        // Assert
        Assert.Equal(SampleEnumerationWithProtectedValue.Second, result);
    }

    [Fact]
    public void Equals_ShouldReturnTrueForSameEnumerationWithProtectedValue()
    {
        // Act
        var result = SampleEnumerationWithProtectedValue.First.Equals(SampleEnumerationWithProtectedValue.First);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalseForDifferentEnumerationWithProtectedValue()
    {
        // Act
        var result = SampleEnumerationWithProtectedValue.First.Equals(SampleEnumerationWithProtectedValue.Second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ShouldReturnTrueForSameEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.First;
        var second = SampleEnumeration.First;

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalseForDifferentEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.First;
        var second = SampleEnumeration.Second;

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_ShouldReturnIdHashCode()
    {
        // Arrange
        var enumeration = new SampleEnumeration(1, "First");

        // Act
        var hashCode = enumeration.GetHashCode();

        // Assert
        Assert.Equal(enumeration.Id.GetHashCode(), hashCode);
    }

    [Fact]
    public void CompareTo_ShouldReturnZeroForSameEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.First;
        var second = SampleEnumeration.First;

        // Act
        var result = first.CompareTo(second);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_ShouldReturnNegativeForSmallerEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.First;
        var second = SampleEnumeration.Second;

        // Act
        var result = first.CompareTo(second);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_ShouldReturnPositiveForLargerEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.Second;
        var second = SampleEnumeration.First;

        // Act
        var result = first.CompareTo(second);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForSameEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.First;
        var second = SampleEnumeration.First;

        // Act
        var result = first == second;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.First;
        var second = SampleEnumeration.Second;

        // Act
        var result = first != second;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorLessThan_ShouldReturnTrueForSmallerEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.First;
        var second = SampleEnumeration.Second;

        // Act
        var result = first < second;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorLessThanOrEqual_ShouldReturnTrueForSmallerOrEqualEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.First;
        var second = SampleEnumeration.Second;

        // Act
        var result = first <= second;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorGreaterThan_ShouldReturnTrueForLargerEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.Second;
        var second = SampleEnumeration.First;

        // Act
        var result = first > second;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorGreaterThanOrEqual_ShouldReturnTrueForLargerOrEqualEnumeration()
    {
        // Arrange
        var first = SampleEnumeration.Second;
        var second = SampleEnumeration.First;

        // Act
        var result = first >= second;

        // Assert
        Assert.True(result);
    }
    [Fact]
    public void Serialize_Enumeration_To_Xml()
    {
        // Arrange
        var enumeration = new SampleEnumerationWithProtectedValue(1, "First", "ProtectedValueOfFirst");
        var dto = enumeration.ToDto(); // Convert To DTO
        var xmlSerializer = new XmlSerializer(typeof(EnumerationDto));
        using var stringWriter = new StringWriter();

        // Act
        xmlSerializer.Serialize(stringWriter, dto);
        var xml = stringWriter.ToString();

        // Assert
        Assert.Contains("<Name>First</Name>", xml); // Name deve aparecer no XML
        Assert.Contains("<Id>1</Id>", xml); // Id deve aparecer no XML
    }
}