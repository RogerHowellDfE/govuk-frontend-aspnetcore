using System;
using System.Linq;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string ErrorSummaryDefaultTitle = "There is a problem";
    internal const string ErrorSummaryDescriptionElement = "p";
    internal const string ErrorSummaryElement = "div";
    internal const string ErrorSummaryItemElement = "li";
    internal const string ErrorSummaryTitleElement = "h2";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateErrorSummary(ErrorSummaryOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(ErrorSummaryElement)
            .WithCssClass("govuk-error-summary")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WhenNotNull(
                options.DisableAutoFocus,
                (daf, b) => b.WithAttribute("data-disable-auto-focus", daf == true ? "true" : "false", encodeValue: false))
            .WithAttributes(options.Attributes)
            .WithAttribute("data-module", "govuk-error-summary", encodeValue: false)
            .WithAppendedHtml(new HtmlTagBuilder("div")
                .WithAttribute("role", "alert", encodeValue: false)
                .WithAppendedHtml(new HtmlTagBuilder(ErrorSummaryTitleElement)
                    .WithCssClass("govuk-error-summary__title")
                    .WithAttributes(options.TitleAttributes)
                    .WithAppendedHtml(GetEncodedTextOrHtml(options.TitleText, options.TitleHtml) ??
                                      new HtmlString(ErrorSummaryDefaultTitle)))
                .WithAppendedHtml(new HtmlTagBuilder("div")
                    .WithCssClass("govuk-error-summary__body")
                    .When(
                        options.DescriptionHtml.NormalizeEmptyString() is not null ||
                        options.DescriptionText.NormalizeEmptyString() is not null,
                        b => b.WithAppendedHtml(new HtmlTagBuilder(ErrorSummaryDescriptionElement)
                            .WithAttributes(options.DescriptionAttributes)
                            .WithAppendedHtml(GetEncodedTextOrHtml(options.DescriptionText, options.DescriptionHtml)!)))
                    .WithAppendedHtml(new HtmlTagBuilder("ul")
                        .WithCssClasses("govuk-list", "govuk-error-summary__list")
                        .WithAppendedHtml(
                            (options.ErrorList ?? Enumerable.Empty<ErrorSummaryOptionsErrorItem>()).Select(item =>
                                new HtmlTagBuilder(ErrorSummaryItemElement)
                                    .WithAttributes(item.ItemAttributes)
                                    .WithAppendedHtml(() =>
                                    {
                                        var content = GetEncodedTextOrHtml(item.Text, item.Html)!;

                                        return item.Href.NormalizeEmptyString() is not null
                                            ? new HtmlTagBuilder("a")
                                                .WithAttributes(item.Attributes)
                                                .WithAttribute("href", item.Href!)
                                                .WithAppendedHtml(content)
                                            : content;
                                    }))))));
    }
}
