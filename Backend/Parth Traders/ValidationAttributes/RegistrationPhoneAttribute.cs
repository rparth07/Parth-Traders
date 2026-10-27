using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace Parth_Traders.ValidationAttributes;

public sealed class RegistrationPhoneAttribute : ValidationAttribute
{
    public const string Guidance = "Enter a 10-digit local phone number, or an international number beginning with + and its country code.";
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 32) return null;
        var compact = Regex.Replace(value, @"[\s().-]", "");
        return Regex.IsMatch(compact, @"^(?:[0-9]{10}|\+[1-9][0-9]{6,14})$") ? compact : null;
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext context) =>
        value == null || Normalize(value as string) != null ? ValidationResult.Success : new ValidationResult(Guidance);
}
