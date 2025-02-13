namespace HaidaiTech.Enumerator.Tests;


public class EnumerationTest
{
    private class SampleEnumeration : Enumeration
    {
        public static readonly SampleEnumeration First = new SampleEnumeration(1, "First");
        public static readonly SampleEnumeration Second = new SampleEnumeration(2, "Second");

        public SampleEnumeration(int id, string name) : base(id, name) { }
    }

    private class SampleEnumerationWithFlag : Enumeration
    {
        public static readonly SampleEnumerationWithFlag First = new SampleEnumerationWithFlag(1, "First", "RealValorOfFirst");
        public static readonly SampleEnumerationWithFlag Second = new SampleEnumerationWithFlag(2, "Second", "RealValorOfSecond");

        public SampleEnumerationWithFlag(int id, string name, string flag) : base(id, name, flag) { }
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
    public void GetAll_ShouldReturnAllEnumerationWithFlagValues()
    {
        // Act
        var allValues = Enumeration.GetAll<SampleEnumerationWithFlag>().ToList();

        // Assert
        Assert.Equal(2, allValues.Count);
        Assert.Contains(SampleEnumerationWithFlag.First, allValues);
        Assert.Contains(SampleEnumerationWithFlag.Second, allValues);
    }

    [Fact]
    public void FromId_ShouldReturnCorrectEnumerationWithFlag()
    {
        // Act
        var result = Enumeration.FromId<SampleEnumerationWithFlag>(1);

        // Assert
        Assert.Equal(SampleEnumerationWithFlag.First, result);
    }

    [Fact]
    public void FromName_ShouldReturnCorrectEnumerationWithFlag()
    {
        // Act
        var result = Enumeration.FromName<SampleEnumerationWithFlag>("Second");

        // Assert
        Assert.Equal(SampleEnumerationWithFlag.Second, result);
    }

    [Fact]
    public void FromFlag_ShouldReturnCorrectEnumerationWithFlag()
    {
        // Act
        var result = Enumeration.FromFlag<SampleEnumerationWithFlag>("RealValorOfFirst");

        // Assert
        Assert.Equal(SampleEnumerationWithFlag.First, result);
    }

    [Fact]
    public void Equals_ShouldReturnTrueForSameEnumerationWithFlag()
    {
        // Act
        var result = SampleEnumerationWithFlag.First.Equals(SampleEnumerationWithFlag.First);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalseForDifferentEnumerationWithFlag()
    {
        // Act
        var result = SampleEnumerationWithFlag.First.Equals(SampleEnumerationWithFlag.Second);

        // Assert
        Assert.False(result);
    }
}

