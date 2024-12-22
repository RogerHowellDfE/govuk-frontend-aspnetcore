using System;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string AccordionElement = "div";

    /// <inheritdoc />
    public virtual HtmlTagBuilder GenerateAccordion(AccordionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(AccordionElement)
            .WithCssClass("govuk-accordion")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttribute("data-module", "govuk-accordion", encodeValue: false)
            .WithAttribute("id", options.Id!)
            .WhenNotNull(options.HideAllSectionsText, (text, b) => b.WithAttribute("data-i18n.hide-all-sections", text))
            .WhenNotNull(options.HideSectionText, (text, b) => b.WithAttribute("data-i18n.hide-section", text))
            .WhenNotNull(options.HideSectionAriaLabelText, (text, b) => b.WithAttribute("data-i18n.hide-section-aria-label", text))
            .WhenNotNull(options.ShowAllSectionsText, (text, b) => b.WithAttribute("data-i18n.show-all-sections", text))
            .WhenNotNull(options.ShowSectionText, (text, b) => b.WithAttribute("data-i18n.show-section", text))
            .WhenNotNull(options.ShowSectionAriaLabelText, (text, b) => b.WithAttribute("data-i18n.show-section-aria-label", text))
            .When(
                options.RememberExpanded is not null,
                b => b.WithAttribute("data-remember-expanded", options.RememberExpanded == true ? "true" : "false",
                    encodeValue: false))
            .WithAttributes(options.Attributes ?? [])
            .WithAppendedHtml(options.Items!.Select((item, index) => GenerateItem(item, index + 1)));

        HtmlTagBuilder GenerateItem(AccordionOptionsItem item, int index)
        {
            return new HtmlTagBuilder("div")
                .WithCssClass("govuk-accordion__section")
                .When(item.Expanded == true, b => b.WithCssClass("govuk-accordion__section--expanded"))
                .WithAppendedHtml(new HtmlTagBuilder("div")
                    .WithCssClass("govuk-accordion__section-header")
                    .WithAppendedHtml(new HtmlTagBuilder($"h{options.HeadingLevel ?? 2}")
                        .WithCssClass("govuk-accordion__section-heading")
                        .WithAppendedHtml(new HtmlTagBuilder("span")
                            .WithCssClass("govuk-accordion__section-button")
                            .WithAttribute("id", $"{options.Id}-heading-{index}", encodeValue: false)
                            .WithAppendedHtml(GetEncodedTextOrHtml(item.Heading!.Text, item.Heading.Html)!)))
                    .When(
                        item.Summary?.Html is not null || item.Summary?.Text is not null,
                        b => b.WithAppendedHtml(new HtmlTagBuilder("div")
                            .WithCssClasses(["govuk-accordion__section-summary", "govuk-body"])
                            .WithAttribute("id", $"{options.Id}-summary-{index}", encodeValue: false)
                            .WithAppendedHtml(GetEncodedTextOrHtml(item.Summary!.Text, item.Summary.Html)!))))
                .WithAppendedHtml(new HtmlTagBuilder("div")
                    .WithAttribute("id", $"{options.Id}-content-{index}", encodeValue: false)
                    .WithCssClass("govuk-accordion__section-content")
                    .When(
                        item.Content?.Html is not null,
                        b => b.WithAppendedHtml(item.Content!.Html!))
                    .When(
                        item.Content?.Html is null && item.Content?.Text is not null,
                        b => b.WithAppendedHtml(new HtmlTagBuilder("p")
                            .WithCssClass("govuk-body")
                            .WithAppendedText(item.Content!.Text!))));
        }
    }
}
