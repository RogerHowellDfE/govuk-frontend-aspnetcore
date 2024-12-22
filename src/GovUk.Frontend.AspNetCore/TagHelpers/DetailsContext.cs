using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class DetailsContext
{
    internal record SummaryInfo(EncodedAttributesDictionary Attributes, IHtmlContent Html);

    internal record TextInfo(EncodedAttributesDictionary Attributes, IHtmlContent Html);

    // internal for testing
    internal SummaryInfo? Summary;
    internal TextInfo? Text;

    public (EncodedAttributesDictionary Attributes, IHtmlContent Html) GetSummaryOptions()
    {
        ThrowIfNotComplete();

        return (Summary.Attributes, Summary.Html);
    }

    public (EncodedAttributesDictionary Attributes, IHtmlContent Html) GetTextOptions()
    {
        ThrowIfNotComplete();

        return (Text.Attributes, Text.Html);
    }

    public void SetSummary(EncodedAttributesDictionary attributes, IHtmlContent html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Summary != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(DetailsSummaryTagHelper.TagName, DetailsTagHelper.TagName);
        }

        if (Text != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(DetailsSummaryTagHelper.TagName, DetailsTextTagHelper.TagName);
        }

        Summary = new SummaryInfo(attributes, html);
    }

    public void SetText(EncodedAttributesDictionary attributes, IHtmlContent html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Text != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(DetailsTextTagHelper.TagName, DetailsTagHelper.TagName);
        }

        Text = new TextInfo(attributes, html);
    }

    [MemberNotNull(nameof(Summary), nameof(Text))]
    private void ThrowIfNotComplete()
    {
        if (Summary == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(DetailsSummaryTagHelper.TagName);
        }

        if (Text == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(DetailsTextTagHelper.TagName);
        }
    }
}
