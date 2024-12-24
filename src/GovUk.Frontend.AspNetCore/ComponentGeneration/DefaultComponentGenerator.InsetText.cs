using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string InsetTextElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateInsetText(InsetTextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(InsetTextElement)
            .WithCssClass("govuk-inset-text")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributeWhenNotNull(options.Id, "id")
            .WithAttributes(options.Attributes)
            .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!);
    }
}
