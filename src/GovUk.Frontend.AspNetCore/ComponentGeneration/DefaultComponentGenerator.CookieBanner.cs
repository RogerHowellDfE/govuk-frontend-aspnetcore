using System;
using System.Linq;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const string CookieBannerElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateCookieBanner(CookieBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        return new HtmlTagBuilder(CookieBannerElement)
            .WithCssClass("govuk-cookie-banner")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithBooleanAttribute("data-nosnippet")
            .WithAttribute("role", "region", encodeValue: false)
            .WithAttribute("aria-label", options.AriaLabel ?? new HtmlString("Cookie banner"))
            .When(options.Hidden == true, b => b.WithBooleanAttribute("hidden"))
            .WithAttributes(options.Attributes)
            .WithAppendedHtml((options.Messages ?? Array.Empty<CookieBannerOptionsMessage>()).Select(message => new HtmlTagBuilder("div")
                .WithCssClasses("govuk-cookie-banner__message", "govuk-width-container")
                .WithCssClasses(ExplodeClasses(message.Classes?.ToHtmlString()))
                .WithAttributeWhenNotNull(message.Role, "role")
                .WithAttributes(message.Attributes)
                .When(message.Hidden == true, b => b.WithBooleanAttribute("hidden"))
                .WithAppendedHtml(new HtmlTagBuilder("div")
                    .WithCssClass("govuk-grid-row")
                    .WithAppendedHtml(new HtmlTagBuilder("div")
                        .WithCssClass("govuk-grid-column-two-thirds")
                        .When(
                            message.HeadingText.NormalizeEmptyString() is not null || message.HeadingHtml.NormalizeEmptyString() is not null,
                            b => b.WithAppendedHtml(new HtmlTagBuilder("h2")
                                .WithCssClasses("govuk-cookie-banner__heading", "govuk-heading-m")
                                .WithAppendedHtml(GetEncodedTextOrHtml(message.HeadingText, message.HeadingHtml)!)))
                        .WithAppendedHtml(new HtmlTagBuilder("div")
                            .WithCssClass("govuk-cookie-banner__content")
                            .When(
                                message.Html.NormalizeEmptyString() is not null || message.Text.NormalizeEmptyString() is not null,
                                b => b.WithAppendedHtml(
                                    message.Html.NormalizeEmptyString() ?? new HtmlTagBuilder("p").WithCssClass("govuk-body").WithAppendedText(message.Text!))))))
                .When(
                    message.Actions is not null,
                    b => b.WithAppendedHtml(new HtmlTagBuilder("div")
                        .WithCssClass("govuk-button-group")
                        .WithAppendedHtml(message.Actions!.Select(action => action.Href.NormalizeEmptyString() is null || action.Type?.ToHtmlString() == "button" ?
                            GenerateButton(new ButtonOptions()
                            {
                                Text = action.Text,
                                Type = action.Type ?? new HtmlString("button"),
                                Name = action.Name,
                                Value = action.Value,
                                Classes = action.Classes,
                                Href = action.Href,
                                Attributes = action.Attributes,
                            }) :
                            new HtmlTagBuilder("a")
                                .WithCssClass("govuk-link")
                                .WithCssClasses(ExplodeClasses(action.Classes?.ToHtmlString()))
                                .WithAttribute("href", action.Href ?? new HtmlString(""))
                                .WithAttributes(action.Attributes)
                                .WithAppendedText(action.Text!)))))));
    }
}
