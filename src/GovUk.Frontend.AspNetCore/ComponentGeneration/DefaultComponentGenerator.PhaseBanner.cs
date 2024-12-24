using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string PhaseBannerElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GeneratePhaseBanner(PhaseBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(PhaseBannerElement)
            .WithCssClass("govuk-phase-banner")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributes(options.Attributes)
            .WithAppendedHtml(new HtmlTagBuilder("p")
                .WithCssClass("govuk-phase-banner__content")
                .WithAppendedHtml(GenerateTag(options.Tag!).WithCssClass("govuk-phase-banner__content__tag"))
                .WithAppendedHtml(new HtmlTagBuilder("span")
                    .WithCssClass("govuk-phase-banner__text")
                    .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!)));
    }
}
