using System.Reflection;

namespace HaidaiTech.Enumeration;

public abstract class Enumeration : IComparable
{
    public string? Flag { get; private set; }
    public string Name { get; private set; }
    public int Id { get; private set; }

    protected Enumeration(int id, string name, string? flag = default!) => (Id, Name, Flag) = (id, name, flag);

    /// <summary>
    /// Returns a string that represents the current instance.
    /// </summary>
    /// <returns>The name of the enumeration.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Gets all values of an enumeration.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <returns>A collection of all values of the enumeration.</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();

    /// <summary>
    /// Finds an Enumeration by ID.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="id">The ID of the Enumeration.</param>
    /// <returns>The Enumeration corresponding to the ID.</returns>
    /// <exception cref="InvalidOperationException">If no Enumeration with the provided ID is found.</exception>
    public static T FromId<T>(int id) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(item => item.Id == id);
        if (matchingItem is null)
            throw new InvalidOperationException($"No {typeof(T).Name} with ID {id} found.");
        return matchingItem;
    }

    /// <summary>
    /// Finds an Enumeration by name.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="name">The name of the Enumeration.</param>
    /// <returns>The Enumeration corresponding to the name.</returns>
    /// <exception cref="InvalidOperationException">If no Enumeration with the provided name is found.</exception>
    public static T FromName<T>(string name) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (matchingItem is null)
            throw new InvalidOperationException($"No {typeof(T).Name} with name {name} found.");
        return matchingItem;
    }

    /// <summary>
    /// Finds an Enumeration by flag.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="flag">The flag of the Enumeration.</param>
    /// <returns>The Enumeration corresponding to the flag.</returns>
    /// <exception cref="InvalidOperationException">If no Enumeration with the provided flag is found.</exception>
    public static T FromFlag<T>(string flag) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(item => item.Flag != null && item.Flag.Equals(flag, StringComparison.OrdinalIgnoreCase));
        if (matchingItem is null)
            throw new InvalidOperationException($"No {typeof(T).Name} with flag {flag} found.");
        return matchingItem;
    }

    /// <summary>
    /// Compares the current instance with another object to check for equality.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns>True if the objects are equal; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    /// <summary>
    /// Returns a hash code for the current instance.
    /// </summary>
    /// <returns>The hash code generated from the ID.</returns>
    public override int GetHashCode() => Id.GetHashCode();

    /// <summary>
    /// Implements IComparable to allow sorting by ID.
    /// </summary>
    /// <param name="other">The other object to compare with.</param>
    /// <returns>An integer that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(object? other) => Id.CompareTo(((Enumeration)other!).Id);

    /// <summary>
    /// Overloads the equality operator to facilitate comparisons.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>True if the operands are equal; otherwise, false.</returns>
    public static bool operator ==(Enumeration left, Enumeration right) => left.Equals(right);

    /// <summary>
    /// Overloads the inequality operator to facilitate comparisons.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>True if the operands are not equal; otherwise, false.</returns>
    public static bool operator !=(Enumeration left, Enumeration right) => !left.Equals(right);

    /// <summary>
    /// Overloads the less than operator to facilitate comparisons.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>True if the left operand is less than the right operand; otherwise, false.</returns>
    public static bool operator <(Enumeration left, Enumeration right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Overloads the less than or equal to operator to facilitate comparisons.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>True if the left operand is less than or equal to the right operand; otherwise, false.</returns>
    public static bool operator <=(Enumeration left, Enumeration right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Overloads the greater than operator to facilitate comparisons.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>True if the left operand is greater than the right operand; otherwise, false.</returns>
    public static bool operator >(Enumeration left, Enumeration right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Overloads the greater than or equal to operator to facilitate comparisons.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>True if the left operand is greater than or equal to the right operand; otherwise, false.</returns>
    public static bool operator >=(Enumeration left, Enumeration right) => left.CompareTo(right) >= 0;
}