using System.Net;
using System.Text.RegularExpressions;

namespace SafeVault.Web.Services;


public static class InputValidationService
{
    private static readonly Regex SafeUsernamePattern = new(@"^[a-zA-Z0-9_.-]{3,50}$", RegexOptions.Compiled);
    private static readonly Regex SafeSearchTermPattern = new(@"^[a-zA-Z0-9 ]{1,100}$", RegexOptions.Compiled);

    public static bool IsValidUsername(string? value) =>
        !string.IsNullOrWhiteSpace(value) && SafeUsernamePattern.IsMatch(value);

    public static bool IsValidSearchTerm(string? value) =>
        !string.IsNullOrWhiteSpace(value) && SafeSearchTermPattern.IsMatch(value);


    public static string EncodeForHtml(string? value) =>
        WebUtility.HtmlEncode(value ?? string.Empty);
}
