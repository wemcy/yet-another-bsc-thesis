using FluentAssertions;
using Wemcy.RecipeApp.Backend.Pagination;

namespace Wemcy.RecipeApp.Backend.Tests.Pagination;

public class PaginationOptionsTests
{
    [Fact]
    public void Constructor_ValidArguments_SetsProperties()
    {
        var options = new PaginationOptions(2, 50);

        options.PageNumber.Should().Be(2);
        options.PageSize.Should().Be(50);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_InvalidPageSize_ThrowsArgumentException(int pageSize)
    {
        var act = () => new PaginationOptions(0, pageSize);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("pageSize");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_NegativePageNumber_ThrowsArgumentException(int pageNumber)
    {
        var act = () => new PaginationOptions(pageNumber, 25);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("pageNumber");
    }

    [Fact]
    public void NullableConstructor_BothNull_UsesDefaultsOf0And25()
    {
        var options = new PaginationOptions(null, null);

        options.PageNumber.Should().Be(0);
        options.PageSize.Should().Be(25);
    }

    [Fact]
    public void NullableConstructor_NullPageSize_UsesConfiguredDefault()
    {
        var options = new PaginationOptions(null, null, defaultPageSize: 50);

        options.PageSize.Should().Be(50);
    }

    [Fact]
    public void NullableConstructor_ProvidedPageSize_IgnoresCustomDefault()
    {
        var options = new PaginationOptions(null, 30, defaultPageSize: 50);

        options.PageSize.Should().Be(30);
    }
}
