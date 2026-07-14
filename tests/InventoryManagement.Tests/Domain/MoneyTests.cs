using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Tests.Domain;

public class MoneyTests
{
    [Fact]
    public void Create_WithValidAmount_SetsProperties()
    {
        var money = new Money(100.50m, "USD");

        Assert.Equal(100.50m, money.Amount);
        Assert.Equal("USD", money.Currency);
    }

    [Fact]
    public void Create_WithNegativeAmount_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Money(-10m));
    }

    [Fact]
    public void Create_DefaultsToUSD()
    {
        var money = new Money(50m);

        Assert.Equal("USD", money.Currency);
    }

    [Fact]
    public void Add_SameCurrency_ReturnsSum()
    {
        var a = new Money(100m);
        var b = new Money(50m);

        var result = a.Add(b);

        Assert.Equal(150m, result.Amount);
    }

    [Fact]
    public void Add_DifferentCurrency_Throws()
    {
        var usd = new Money(100m, "USD");
        var eur = new Money(50m, "EUR");

        Assert.Throws<InvalidOperationException>(() => usd.Add(eur));
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var a = new Money(100m, "USD");
        var b = new Money(100m, "USD");

        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var a = new Money(100m);
        var b = new Money(50m);

        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }
}
