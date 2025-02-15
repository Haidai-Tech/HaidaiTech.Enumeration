# HaidaiTech.Enumeration

Enumeration classes are a powerful alternative to traditional `enum` types in C#. They provide a more flexible and expressive way to represent sets of constant values, especially in complex systems like microservices or applications following Domain-Driven Design (DDD).

---

## **Why Use Enumeration Classes?**

Traditional `enum` types in C# are useful for defining sets of constant values, but they have several limitations:

1. **Lack of Flexibility**:
   - Enums are essentially integer-based and cannot store additional information or behaviors associated with each value.

2. **Difficulty in Extending**:
   - Enums are closed for extension. Adding new values requires modifying the enum definition directly.

3. **Limited Semantics**:
   - Enums are limited to integer values, making it hard to associate rich data (e.g., descriptions, codes, or behaviors).

4. **Serialization Issues**:
   - Enums are serialized as integers, which can cause compatibility issues when the order of values changes or when interoperating with other systems.

---

## **What Problems Do Enumeration Classes Solve?**

Enumeration classes address these limitations by providing a more robust and flexible way to represent constant values. Here’s how:

1. **Additional Properties and Behaviors**:
   - Each value in an enumeration class can have properties and methods associated with it.
   - Example:
     ```csharp
     public class OrderStatus : Enumeration
     {
         public static readonly OrderStatus Pending = new OrderStatus(1, "Pending");
         public static readonly OrderStatus Approved = new OrderStatus(2, "Approved");
         public static readonly OrderStatus Shipped = new OrderStatus(3, "Shipped");

         public string Description { get; }

         private OrderStatus(int value, string description)
             : base(value, description)
         {
             Description = description;
         }
     }
     ```

2. **Extensibility**:
   - Enumeration classes can be easily extended by creating new classes or adding new values without modifying the base class.

3. **Rich Semantics**:
   - You can add custom logic or behaviors directly to the enumeration class.
   - Example:
     ```csharp
     public bool CanBeCancelled()
     {
         return this == Pending || this == Approved;
     }
     ```

4. **Better Serialization**:
   - Enumeration classes can be serialized as rich objects (e.g., JSON with descriptive properties).

5. **Alignment with DDD**:
   - Enumeration classes are ideal for representing domain concepts in a more expressive and domain-aligned way.

---

## **Example of an Enumeration Class**

Here’s a complete example of an enumeration class:

```csharp
public class OrderStatus : Enumeration
{
    public static readonly OrderStatus Pending = new OrderStatus(1, "Pending");
    public static readonly OrderStatus Approved = new OrderStatus(2, "Approved");
    public static readonly OrderStatus Shipped = new OrderStatus(3, "Shipped");

    private OrderStatus(int value, string name)
        : base(value, name) { }

    public bool CanBeCancelled()
    {
        return this == Pending || this == Approved;
    }
}
```

# Enumeration Implementation

The implementation of `Enumeration` is wonderful. In the past, when we wanted to use a `string` field to represent a value, we used a class like this:

```csharp
public static class SelectedValues
{
    public const string Value1 = "Value 1";
    public const string Value2 = "Value 2";
}
```

And when we wanted to expose it in a `Command` in the `Controller`, we had to perform a lot of workarounds to do a `CAST`.

However, this `Enumeration` implementation has a problem. Some applications use **façade properties**, meaning that what is exposed in a `Command` is completely different from what is used internally. This is useful to avoid prying eyes. The issue is that if you want to use façade properties with `Enumeration`, you will face another level of complexity and more workarounds to perform a `CAST`, stating that the property with a fake name corresponds to the `xpto` property inside your project.

For example, you created an `Enumeration` that has a field `TrafficAware`, and it corresponds to `"TRAFFIC_AWARE"` in a `POST` request you will send to an API. You would have to use `if` or `switch` statements to detect which `Enumeration` is coming from a request. We would be going back to square one with the `enum` problem. That's why we added the `ProtectedValue` field, which makes it easier to protect the actual value of that `Enumeration`. We also added the `[JsonIgnore]` attribute to prevent your `Enumeration` from being deserialized, keeping prying eyes away from your code. Therefore, if you do this:

```csharp
public class TravelModeEnumerator : Enumeration
{
    public static readonly TravelModeEnumerator DrivingByCar = new TravelModeEnumerator(1, nameof(DrivingByCar), "DRIVE");
    public static readonly TravelModeEnumerator Walking = new TravelModeEnumerator(2, nameof(Walking), "WALK");
    public static readonly TravelModeEnumerator Bicycling = new TravelModeEnumerator(3, nameof(Bicycling), "BICYCLE");
    public static readonly TravelModeEnumerator Transit = new TravelModeEnumerator(4, nameof(Transit), "TRANSIT");
    public static readonly TravelModeEnumerator TwoWheeler = new TravelModeEnumerator(5, nameof(TwoWheeler), "TWO_WHEELER");
}
```

And have a method like this, you can get the `ProtectedValue` of `Enumeration`:

```csharp
public async Task<RoutesOutput> GetRoutesAsync(TravelModeEnumerator travelMode)
{
    var routesInput = new RoutesInput(
        TravelModeEnumerator.FromName(travelMode.Name).ProtectedValue!
    );
}
```

In the test `ProtectedValue_ShouldNotBeSerialized()`, we check if the `ProtectedValue` property is protected from deserialization.

There is another important point: you might still work with XML. According to the `Enumeration` class, there is no parameterless constructor, and to avoid violating **S.O.L.I.D**, specifically the **Open/Closed Principle (OCP)** and, to a lesser extent, the **Single Responsibility Principle (SRP)**, we created an `EnumerationDto` class that only receives the `Id` and `Name`. If you want to serialize to XML, do this:

```csharp
var enumeration = new SampleEnumerationWithProtectedValue(1, "First", "ProtectedValueOfFirst");
var dto = enumeration.ToDto(); // Convert to DTO
var xmlSerializer = new XmlSerializer(typeof(EnumerationDto));
using var stringWriter = new StringWriter();

// Act
xmlSerializer.Serialize(stringWriter, dto);
var xml = stringWriter.ToString();

// Assert
Assert.Contains("<Name>First</Name>", xml); // Name should appear in XML
Assert.Contains("<Id>1</Id>", xml); // Id should appear in XML
```

### EnumerationConverter Integration

Another important detail: We have included the `EnumerationConverter` in this project to simplify the construction of your application's payload. To enable it, add the following code to your application:

```csharp
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new EnumerationConverter<TravelModeEnumerator>());
                    options.JsonSerializerOptions.Converters.Add(new EnumerationConverter<RoutingPreferencesEnumerator>());
                });
```

With this converter in place, your application's payload only needs to include the Enumeration property name:

```json
{
  "alternativeRoutes": true,
  "travelMode": "DrivingByCar", 
  "routingPreferences": "TrafficAware"
}
```

If the converter is not configured as described above, you will need to include the Enumeration code, which adds unnecessary complexity to your application:

```json
{
  "alternativeRoutes": true,
  "travelMode": {"id": 1, "name": "DrivingByCar"}, // unnecessary complexity
  "routingPreferences": {"id": 1, "name": "TrafficAware"} // unnecessary complexity
}
```

By using the `EnumerationConverter`, you can streamline your payload and improve the readability of your code. 

