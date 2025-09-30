namespace Domain.ValueObjects;


// Encapsulate, strong semantic and immutability
public readonly record struct LadderId(Guid Value)
{
    public static LadderId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}