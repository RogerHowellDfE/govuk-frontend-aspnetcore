using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const int PanelDefaultHeadingLevel = 1;
    internal const string PanelElement = "div";
    internal const int PanelMinHeadingLevel = 1;
    internal const int PanelMaxHeadingLevel = 6;

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GeneratePanel(PanelOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(PanelElement)
            .WithCssClasses("govuk-panel", "govuk-panel--confirmation")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributes(options.Attributes)
            .WithAppendedHtml(new HtmlTagBuilder($"h{options.HeadingLevel ?? PanelDefaultHeadingLevel}")
                .WithCssClass("govuk-panel__title")
                .WithAttributes(options.TitleAttributes)
                .WithAppendedHtml(GetEncodedTextOrHtml(options.TitleText, options.TitleHtml)!))
            .When(
                options.Text.NormalizeEmptyString() is not null || options.Html.NormalizeEmptyString() is not null,
                b => b.WithAppendedHtml(new HtmlTagBuilder("div")
                    .WithCssClass("govuk-panel__body")
                    .WithAttributes(options.BodyAttributes)
                    .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!)));
    }
}
