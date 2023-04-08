namespace DDD.Domain.Products;

//SKU =>  Stock Keeping Unit
public record Sku
{
    public string Value { get; init; }

    private Sku(string value)
    {
        Value = value;
    }

    public static Sku? Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        if (value.Length < 10)
            return null;

        return new Sku(value);
    }
}