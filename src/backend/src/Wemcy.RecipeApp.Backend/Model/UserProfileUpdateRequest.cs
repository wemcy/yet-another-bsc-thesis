using System.Diagnostics.CodeAnalysis;

namespace Wemcy.RecipeApp.Backend.Model;

public class UserProfileUpdateRequest
{
    public required Stream? ImageStream { get; set; }
    public required string? ImageName { get; set; }
    public required string? DisplayName { get; set; }
    public required string? Password { get; set; }
    public required string? Email { get; set; }

    [MemberNotNullWhen(true, nameof(ImageStream))]
    [MemberNotNullWhen(true, nameof(ImageName))]
    public bool HasImageUpdate => ImageStream is not null && ImageName is not null;
    [MemberNotNullWhen(true, nameof(DisplayName))]
    public bool HasDisplayNameUpdate => !String.IsNullOrWhiteSpace(DisplayName);
    [MemberNotNullWhen(true, nameof(Password))]
    public bool HasPasswordUpdate => !String.IsNullOrWhiteSpace(Password);
    [MemberNotNullWhen(true, nameof(Email))]
    public bool HasEmailUpdate => !String.IsNullOrWhiteSpace(Email);
}
