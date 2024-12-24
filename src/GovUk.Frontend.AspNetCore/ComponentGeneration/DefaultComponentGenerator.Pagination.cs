using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string PaginationDefaultPreviousText = "Previous";
    internal const string PaginationDefaultNextText = "Next";
    internal const string PaginationElement = "nav";
    internal const string PaginationItemElement = "li";
    internal const string PaginationNextElement = "div";
    internal const string PaginationPreviousElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GeneratePagination(PaginationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var blockLevel = options.Items?.Any() != true && (options.Next is not null || options.Previous is not null);

        var arrowPrevious = new HtmlTagBuilder("svg")
            .WithCssClasses("govuk-pagination__icon", "govuk-pagination__icon--prev")
            .WithAttribute("xmlns", "http://www.w3.org/2000/svg", encodeValue: false)
            .WithAttribute("height", "13", encodeValue: false)
            .WithAttribute("width", "15", encodeValue: false)
            .WithAttribute("aria-hidden", "true", encodeValue: false)
            .WithAttribute("focusable", "false", encodeValue: false)
            .WithAttribute("viewBox", "0 0 15 13", encodeValue: false)
            .WithAppendedHtml(new HtmlTagBuilder("path")
                .WithAttribute("d",
                    "m6.5938-0.0078125-6.7266 6.7266 6.7441 6.4062 1.377-1.449-4.1856-3.9768h12.896v-2h-12.984l4.2931-4.293-1.414-1.414z",
                    encodeValue: false));

        var arrowNext = new HtmlTagBuilder("svg")
            .WithCssClasses("govuk-pagination__icon", "govuk-pagination__icon--next")
            .WithAttribute("xmlns", "http://www.w3.org/2000/svg", encodeValue: false)
            .WithAttribute("height", "13", encodeValue: false)
            .WithAttribute("width", "15", encodeValue: false)
            .WithAttribute("aria-hidden", "true", encodeValue: false)
            .WithAttribute("focusable", "false", encodeValue: false)
            .WithAttribute("viewBox", "0 0 15 13", encodeValue: false)
            .WithAppendedHtml(new HtmlTagBuilder("path")
                .WithAttribute("d",
                    "m8.107-0.0078125-1.4136 1.414 4.2926 4.293h-12.986v2h12.896l-4.1855 3.9766 1.377 1.4492 6.7441-6.4062-6.7246-6.7266z",
                    encodeValue: false));

        HtmlTagBuilder ArrowLink(IPaginationOptionsLink link, string type, string fallbackText)
        {
            var arrowType = type == "prev" ? arrowPrevious : arrowNext;

            return new HtmlTagBuilder("div")
                .WithCssClass($"govuk-pagination__{type}")
                .WithAttributes(link.ContainerAttributes)
                .WithAppendedHtml(new HtmlTagBuilder("a")
                    .WithCssClasses("govuk-link", "govuk-pagination__link")
                    .WithAttribute("href", link.Href!)
                    .WithAttribute("rel", type, encodeValue: false)
                    .WithAttributes(link.Attributes)
                    .When(blockLevel || type == "prev", b => b.WithAppendedHtml(arrowType))
                    .WithAppendedHtml(new HtmlTagBuilder("span")
                        .WithCssClass("govuk-pagination__link-title")
                        .When(blockLevel && link.LabelText.NormalizeEmptyString() is null,
                            b => b.WithCssClass("govuk-pagination__link-title--decorated"))
                        .WithAppendedHtml(() =>
                        {
                            var content = GetEncodedTextOrHtml(link.Text, link.Html);

                            if (content is null)
                            {
                                var builder = new HtmlContentBuilder();
                                builder.Append(fallbackText);
                                builder.AppendHtml(new HtmlTagBuilder("span")
                                    .WithCssClass("govuk-visually-hidden")
                                    .WithAppendedText(" page"));
                                return builder;
                            }
                            else
                            {
                                return content;
                            }
                        }))
                    .When(
                        link.LabelText.NormalizeEmptyString() is not null && blockLevel,
                        b => b
                            .WithAppendedHtml(new HtmlTagBuilder("span")
                                .WithCssClass("govuk-visually-hidden")
                                .WithAppendedText(":"))
                            .WithAppendedHtml(new HtmlTagBuilder("span")
                                .WithCssClass("govuk-pagination__link-label")
                                .WithAppendedHtml(link.LabelText!)))
                    .When(
                        !blockLevel && type == "next",
                        b => b.WithAppendedHtml(arrowType)));
        }

        HtmlTagBuilder PageItem(PaginationOptionsItem item) =>
            new HtmlTagBuilder(PaginationItemElement)
                .WithCssClass("govuk-pagination__item")
                .When(item.Current == true, b => b.WithCssClass("govuk-pagination__item--current"))
                .When(item.Ellipsis == true, b => b.WithCssClass("govuk-pagination__item--ellipses"))
                .WithAppendedHtml(
                    item.Ellipsis == true
                        ? new HtmlString("&ctdot;")
                        : new HtmlTagBuilder("a")
                            .WithCssClasses("govuk-link", "govuk-pagination__link")
                            .WithAttribute("href", item.Href!)
                            .WithAttribute("aria-label",
                                item.VisuallyHiddenText ?? new HtmlString($"Page {item.Number}"))
                            .When(item.Current == true,
                                b => b.WithAttribute("aria-current", "page", encodeValue: false))
                            .WithAttributes(item.Attributes)
                            .WithAppendedHtml(item.Number!));

        return new HtmlTagBuilder(PaginationElement)
            .WithCssClass("govuk-pagination")
            .When(blockLevel, b => b.WithCssClass("govuk-pagination--block"))
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttribute("role", "navigation", encodeValue: false)
            .WithAttribute("aria-label", options.LandmarkLabel ?? new HtmlString("Pagination"))
            .WithAttributes(options.Attributes)
            .When(
                options.Previous is not null && options.Previous.Href.NormalizeEmptyString() is not null,
                b =>
                {
                    Debugger.Break();
                    b.WithAppendedHtml(ArrowLink(options.Previous!, "prev", "Previous"));
                })
            .When(
                options.Items is not null,
                b => b.WithAppendedHtml(new HtmlTagBuilder("ul")
                    .WithCssClass("govuk-pagination__list")
                    .WithAppendedHtml(options.Items!.Select(PageItem))))
            .When(
                options.Next is not null && options.Next.Href.NormalizeEmptyString() is not null,
                b =>
                {
                    Debugger.Break();
                    var x = ArrowLink(options.Next!, "next", "Next");
                    b.WithAppendedHtml(ArrowLink(options.Next!, "next", "Next"));
                });
    }
}
