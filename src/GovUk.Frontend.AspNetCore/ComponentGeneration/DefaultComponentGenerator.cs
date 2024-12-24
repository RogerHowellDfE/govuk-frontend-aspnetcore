using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Default implementation of <see cref="IComponentGenerator"/>.
/// </summary>
public partial class DefaultComponentGenerator : IComponentGenerator
{
    internal const string FormGroupElement = "div";

    private protected static void AppendToDescribedBy(ref IHtmlContent describedBy, IHtmlContent value)
    {
        var str = describedBy.ToHtmlString();
        str = (str + (" " + value)).Trim();
        describedBy = new HtmlString(str);
    }

    private protected virtual HtmlTagBuilder GenerateFormGroup(
        FormGroupOptions? options,
        bool haveError)
    {
        return new HtmlTagBuilder(FormGroupElement)
            .WithCssClass("govuk-form-group")
            .When(haveError, b => b.WithCssClass("govuk-form-group--error"))
            .WithCssClasses(ExplodeClasses(options?.Classes?.ToHtmlString()))
            .WithAttributes(options?.Attributes);
    }

    private static string[] ExplodeClasses(string? classes) =>
        classes is null ? Array.Empty<string>() : classes.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    [return: NotNullIfNotNull("text")]
    private static string? HtmlEncode(string? text) =>
        text is not null ? System.Text.Encodings.Web.HtmlEncoder.Default.Encode(text) : null;

    private static string? GetEncodedTextOrHtml(string? text, string? html) =>
        html.NormalizeEmptyString() ?? HtmlEncode(text);

    private static IHtmlContent? GetEncodedTextOrHtml(string? text, IHtmlContent? html) =>
        html.NormalizeEmptyString() ?? (text is not null ? new HtmlString(HtmlEncode(text)) : null);
}
