using FluentAssertions;
using Wemcy.RecipeApp.Backend.Extensions;

namespace Wemcy.RecipeApp.Backend.Tests.Extensions;

public class LinqExtensionsTests
{
    // ── ConditionalWhere (bool overload) ─────────────────────────────────────────

    [Fact]
    public void ConditionalWhere_BoolTrue_AppliesPredicate()
    {
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        var result = query.ConditionalWhere(true, x => x > 3).ToList();

        result.Should().BeEquivalentTo([4, 5]);
    }

    [Fact]
    public void ConditionalWhere_BoolFalse_ReturnsSourceUnchanged()
    {
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        var result = query.ConditionalWhere(false, x => x > 3).ToList();

        result.Should().HaveCount(5);
    }

    // ── ConditionalWhere (Func<bool> overload) ───────────────────────────────────

    [Fact]
    public void ConditionalWhere_FuncTrue_AppliesPredicate()
    {
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        var result = query.ConditionalWhere(() => true, x => x > 3).ToList();

        result.Should().BeEquivalentTo([4, 5]);
    }

    [Fact]
    public void ConditionalWhere_FuncFalse_ReturnsSourceUnchanged()
    {
        var query = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

        var result = query.ConditionalWhere(() => false, x => x > 3).ToList();

        result.Should().HaveCount(5);
    }

    // ── MapIfHasElementOrDefault ─────────────────────────────────────────────────

    [Fact]
    public void MapIfHasElementOrDefault_NullList_ReturnsDefault()
    {
        IList<int>? list = null;

        var result = list.MapIfHasElementOrDefault(l => l.Sum());

        result.Should().Be(0);
    }

    [Fact]
    public void MapIfHasElementOrDefault_EmptyList_ReturnsDefault()
    {
        IList<int> list = [];

        var result = list.MapIfHasElementOrDefault(l => l.Sum());

        result.Should().Be(0);
    }

    [Fact]
    public void MapIfHasElementOrDefault_NonEmptyList_ReturnsMappedValue()
    {
        IList<int> list = [1, 2, 3];

        var result = list.MapIfHasElementOrDefault(l => l.Sum());

        result.Should().Be(6);
    }

    [Fact]
    public void MapIfHasElementOrDefault_NonEmptyList_CustomDefaultIsIgnored()
    {
        IList<int> list = [10];

        var result = list.MapIfHasElementOrDefault(l => l.Sum(), defaultValue: -1);

        result.Should().Be(10);
    }
}
