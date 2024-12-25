using System;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string SkipLinkDefaultHref = "#content";
    internal const string SkipLinkElement = "a";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateSkipLink(SkipLinkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(SkipLinkElement)
            .WithAttribute("href", options.Href ?? new HtmlString(SkipLinkDefaultHref))
            .WithCssClass("govuk-skip-link")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributes(options.Attributes)
            .WithAttribute("data-module", "govuk-skip-link", encodeValue: false)
            .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!);
    }
}
