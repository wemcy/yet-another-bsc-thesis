using System.Text;
using System.Text.RegularExpressions;

namespace Wemcy.RecipeApp.Backend.Utils;

public static partial class TextNormalizer
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleWhitespaceRegex();

    [GeneratedRegex(@"[\p{C}]")]
    private static partial Regex ControlCharactersRegex();

    /// <summary>
    /// Normalizes a text input by trimming whitespace, collapsing internal repeated whitespace
    /// to a single space, removing control characters, and applying Unicode NFC normalization.
    /// Returns null if the input is null.
    /// </summary>
    public static string? Normalize(string? input)
    {
        if (input is null)
            return null;

        var result = input.Normalize(NormalizationForm.FormC);
        result = MultipleWhitespaceRegex().Replace(result, " ");
        result = ControlCharactersRegex().Replace(result, string.Empty);
        result = result.Trim();

        return result;
    }

    /// <summary>
    /// Normalizes each entry in the list in-place. Entries that become empty after
    /// normalization are removed.
    /// </summary>
    public static IList<string> NormalizeList(IList<string> items)
    {
        var normalized = items
            .Select(Normalize)
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => s!)
            .ToList();

        return normalized;
    }
}
