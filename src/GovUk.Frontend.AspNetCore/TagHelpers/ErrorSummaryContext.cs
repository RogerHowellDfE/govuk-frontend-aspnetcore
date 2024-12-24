using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class ErrorSummaryContext
{
    internal record DescriptionInfo(EncodedAttributesDictionary Attributes, IHtmlContent Html);

    internal record TitleInfo(EncodedAttributesDictionary Attributes, IHtmlContent Html);

    private readonly List<ErrorSummaryOptionsErrorItem> _items;

    public ErrorSummaryContext()
    {
        _items = new List<ErrorSummaryOptionsErrorItem>();
    }

    // internal for testing
    internal DescriptionInfo? Description;
    internal TitleInfo? Title;

    public IReadOnlyCollection<ErrorSummaryOptionsErrorItem> Items => _items;

    public bool HasContent => Description is not null || Title is not null || Items.Count > 0;

    public void AddItem(ErrorSummaryOptionsErrorItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    public void AddItem(
        IHtmlContent? href,
        IHtmlContent html,
        EncodedAttributesDictionary attributes,
        EncodedAttributesDictionary itemAttributes)
    {
        ArgumentNullException.ThrowIfNull(html);
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(itemAttributes);

        AddItem(new ErrorSummaryOptionsErrorItem()
        {
            Text = null,
            Html = html,
            Href = href,
            Attributes = attributes,
            ItemAttributes = itemAttributes
        });
    }

    public (EncodedAttributesDictionary Attributes, IHtmlContent Html)? GetTitle() =>
        Title is not null ?
        (Title.Attributes, Title.Html) :
        null;

    public (EncodedAttributesDictionary Attributes, IHtmlContent Html)? GetDescription() =>
        Description is not null ?
        (Description.Attributes, Description.Html) :
        null;

    public void SetDescription(EncodedAttributesDictionary attributes, IHtmlContent html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Description != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                ErrorSummaryDescriptionTagHelper.TagName,
                ErrorSummaryTagHelper.TagName);
        }

        Description = new DescriptionInfo(attributes, html);
    }

    public void SetTitle(EncodedAttributesDictionary attributes, IHtmlContent html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Title != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                ErrorSummaryTitleTagHelper.TagName,
                ErrorSummaryTagHelper.TagName);
        }

        Title = new TitleInfo(attributes, html);
    }
}
