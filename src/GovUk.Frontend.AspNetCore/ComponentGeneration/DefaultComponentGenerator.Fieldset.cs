using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string FieldsetElement = "fieldset";
    internal const bool FieldsetLegendDefaultIsPageHeading = false;
    internal const string FieldsetLegendElement = "legend";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateFieldset(FieldsetOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(FieldsetElement)
            .WithCssClass("govuk-fieldset")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributeWhenNotNull(options.Role.NormalizeEmptyString(), "role")
            .WithAttributeWhenNotNull(options.DescribedBy.NormalizeEmptyString(), "aria-describedby")
            .WithAttributes(options.Attributes)
            .When(
                options.Legend is not null,
                b => b.WithAppendedHtml(new HtmlTagBuilder(FieldsetLegendElement)
                    .WithCssClass("govuk-fieldset__legend")
                    .WithCssClasses(ExplodeClasses(options.Legend?.Classes?.ToHtmlString()))
                    .WithAttributes(options.Legend?.Attributes)
                    .WithAppendedHtml(() =>
                    {
                        var content = GetEncodedTextOrHtml(options.Legend!.Text, options.Legend.Html)!;

                        return options.Legend!.IsPageHeading ?? FieldsetLegendDefaultIsPageHeading
                            ? new HtmlTagBuilder("h1")
                                .WithCssClass("govuk-fieldset__heading")
                                .WithAppendedHtml(content)
                            : content;
                    })))
            .WhenNotNull(options.Html, (html, b) => b.WithAppendedHtml(html));
    }
}
