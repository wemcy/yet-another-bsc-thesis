using FluentAssertions;
using Wemcy.RecipeApp.Backend.Utils;

namespace Wemcy.RecipeApp.Backend.Tests.UnitTest.Utils;

public class TextNormalizerTest
{
    [Fact]
    public void Normalize_NullInput_ReturnsNull()
    {
        TextNormalizer.Normalize(null).Should().BeNull();
    }

    [Theory]
    [InlineData("  hello  ", "hello")]
    [InlineData("\t recipe \n", "recipe")]
    public void Normalize_LeadingAndTrailingWhitespace_IsTrimmed(string input, string expected)
    {
        TextNormalizer.Normalize(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("hello   world", "hello world")]
    [InlineData("a  b\t\tc", "a b c")]
    public void Normalize_RepeatedInternalWhitespace_IsCollapsedToSingleSpace(string input, string expected)
    {
        TextNormalizer.Normalize(input).Should().Be(expected);
    }

    [Fact]
    public void Normalize_ControlCharacters_AreRemoved()
    {
        var input = "hello\u0000world\u001F!";
        TextNormalizer.Normalize(input).Should().Be("helloworld!");
    }

    [Fact]
    public void Normalize_UnicodeInput_IsNfcNormalized()
    {
        // 'é' as decomposed (e + combining accent) vs precomposed
        var decomposed = "e\u0301";
        var precomposed = "\u00E9";
        TextNormalizer.Normalize(decomposed).Should().Be(precomposed);
    }

    [Theory]
    [InlineData("Normal title", "Normal title")]
    [InlineData("  Spaced   Out  Title  ", "Spaced Out Title")]
    public void Normalize_ValidInput_ReturnsCleanString(string input, string expected)
    {
        TextNormalizer.Normalize(input).Should().Be(expected);
    }

    [Fact]
    public void NormalizeList_EmptyList_ReturnsEmptyList()
    {
        TextNormalizer.NormalizeList([]).Should().BeEmpty();
    }

    [Fact]
    public void NormalizeList_EntriesThatBecomeEmptyAfterNormalization_AreRemoved()
    {
        var input = new List<string> { "  ", "\t", "valid step" };
        TextNormalizer.NormalizeList(input).Should().ContainSingle()
            .Which.Should().Be("valid step");
    }

    [Fact]
    public void NormalizeList_AllEntries_AreNormalized()
    {
        var input = new List<string> { "  step one  ", "step   two" };
        TextNormalizer.NormalizeList(input).Should().Equal("step one", "step two");
    }
}
