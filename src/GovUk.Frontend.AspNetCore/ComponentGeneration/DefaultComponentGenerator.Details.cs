using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string DetailsElement = "details";
    internal const string DetailsSummaryElement = "summary";
    internal const string DetailsTextElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateDetails(DetailsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(DetailsElement)
            .WhenNotNull(options.Id, (id, b) => b.WithAttribute("id", id))
            .WithCssClass("govuk-details")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .When(options.Open == true, b => b.WithBooleanAttribute("open"))
            .WithAttributes(options.Attributes)
            .WithAppendedHtml(new HtmlTagBuilder(DetailsSummaryElement)
                .WithCssClass("govuk-details__summary")
                .WithAttributes(options.SummaryAttributes)
                .WithAppendedHtml(new HtmlTagBuilder("span")
                    .WithCssClass("govuk-details__summary-text")
                    .WithAppendedHtml(GetEncodedTextOrHtml(options.SummaryText, options.SummaryHtml)!)))
            .WithAppendedHtml(new HtmlTagBuilder(DetailsTextElement)
                .WithCssClass("govuk-details__text")
                .WithAttributes(options.TextAttributes)
                .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!));
    }
}
