# Domain Driven Design

- Domain-Driven Design (DDD) approach, which emphasizes designing the software system around the business domain. This approach helps to ensure that the software system is closely aligned with the needs and requirements of the business.

- In a DDD approach, the focus is on creating well-defined bounded contexts that encapsulate a specific domain. Each bounded context is implemented as a separate aggregate in the software system. The aggregates communicate with each other through well-defined interfaces and events. 

- By using a DDD approach, the application is able to model the business domain in a natural and intuitive way. The resulting software system is easier to understand, maintain, and evolve over time.

---
# Domain - Architecture 
> This domain has 3 main Aggregates `Customer`, `Product`, and `Order`.

## Customer
> The Customer aggregate has the following properties:
- `Id` : The unique identifier for the customer.
- `Email` : The email address associated with the customer's account.
- `Name` : The name associated with the customer's account.

```csharp
public class Customer
{
    public CustomerId Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
}
```
```csharp
public record CustomerId(Guid Value);
```
---

## Product
> The Product aggregate represents a product that can be purchased. The Product aggregate has the following properties:
- `Id` : The unique identifier for the product.
- `Name` : The name of the product.
- `Price` : The price of the product.
- `Sku` : The stock keeping unit (SKU) for the product.
```csharp
public class Product
{
    public ProductId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Price { get; private set; }
    public Sku Sku { get; private set; }
}
```
```csharp
public record ProductId(Guid Value);
```
```csharp
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
```
```csharp
public record Money(string Currency, decimal Amount);
```

---
## Order
```csharp
public class Order
{
    private readonly HashSet<LineItem> _lineItems = new();
    public OrderId Id { get; private set; }
    public CustomerId CustomerId { get; private set; }

    private Order(OrderId id, CustomerId customerId)
    {
        Id = id;
        CustomerId = customerId;
    }

    public static Order Create(CustomerId customerId)
    {
        return new Order(new OrderId(Guid.NewGuid()), customerId);
    }

    public void Add(ProductId productId, Money price)
    {
        var lineItem = new LineItem(new LineItemId(Guid.NewGuid()), Id, productId, price);
        _lineItems.Add(lineItem);
    }
}
```
```csharp
public record OrderId(Guid Id);
```


### LineItem
```csharp
public class LineItem
{
    public LineItemId Id { get; private set; }
    public OrderId OrderId { get; private set; }
    public ProductId ProductId { get; private set; }
    public Money Price { get; private set; }

    internal LineItem(LineItemId id, OrderId orderId, ProductId productId, Money price)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Price = price;
    }
}
```
```csharp
public record LineItemId(Guid Value);
```