using System;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string WarningTextElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateWarningText(WarningTextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(WarningTextElement)
            .WithCssClass("govuk-warning-text")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributes(options.Attributes)
            .WithAppendedHtml(new HtmlTagBuilder("span")
                .WithCssClass("govuk-warning-text__icon")
                .WithAttribute("aria-hidden", "true", encodeValue: false)
                .WithAppendedText("!"))
            .WithAppendedHtml(new HtmlTagBuilder("strong")
                .WithCssClass("govuk-warning-text__text")
                .WithAppendedHtml(new HtmlTagBuilder("span")
                    .WithCssClass("govuk-visually-hidden")
                    .WithAppendedHtml(options.IconFallbackText.NormalizeEmptyString() ?? new HtmlString("Warning")))
                .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html) ?? new HtmlString("")));
    }
}
