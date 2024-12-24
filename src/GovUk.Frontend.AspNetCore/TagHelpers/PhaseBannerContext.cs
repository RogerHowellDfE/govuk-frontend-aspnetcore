using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class PhaseBannerContext
{
    internal record TagInfo(EncodedAttributesDictionary Attributes, IHtmlContent Html);

    // internal for testing
    internal TagInfo? Tag;

    public TagOptions GetTagOptions()
    {
        ThrowIfIncomplete();

        return new TagOptions()
        {
            Text = null,
            Html = Tag.Html,
            Attributes = new EncodedAttributesDictionaryBuilder(Tag.Attributes).Without("class", out var classes),
            Classes = classes
        };
    }

    public void SetTag(EncodedAttributesDictionary attributes, IHtmlContent html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Tag != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                PhaseBannerTagTagHelper.TagName, PhaseBannerTagHelper.TagName);
        }

        Tag = new TagInfo(attributes, html);
    }

    [MemberNotNull(nameof(Tag))]
    private void ThrowIfIncomplete()
    {
        if (Tag == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(PhaseBannerTagTagHelper.TagName);
        }
    }
}
