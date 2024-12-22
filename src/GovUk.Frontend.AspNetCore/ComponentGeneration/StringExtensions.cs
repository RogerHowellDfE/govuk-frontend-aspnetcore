using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal static class StringExtensions
{
    public static string? NormalizeEmptyString(this string? value) => string.IsNullOrEmpty(value) ? null : value;

    public static string Capitalize(this string str) => str[0..1].ToUpper() + str[1..];

    public static IHtmlContent? NormalizeEmptyString(this IHtmlContent? htmlString) =>
        string.IsNullOrEmpty(htmlString?.ToHtmlString()) ? null : htmlString;
}
