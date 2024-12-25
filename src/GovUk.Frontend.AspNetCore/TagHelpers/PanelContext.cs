using System;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class PanelContext
{
    private string? _bodyTagName;

    public (IHtmlContent Content, EncodedAttributesDictionary Attributes)? Body { get; private set; }
    public (IHtmlContent Content, EncodedAttributesDictionary Attributes)? Title { get; private set; }

    public void SetBody(IHtmlContent content, EncodedAttributesDictionary attributes, string tagName)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(tagName);

        if (Body != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(tagName, PanelTagHelper.TagName);
        }

        Body = (content, attributes);
        _bodyTagName = tagName;
    }

    public void SetTitle(IHtmlContent content, EncodedAttributesDictionary attributes, string tagName)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(tagName);

        if (Title != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(tagName, PanelTagHelper.TagName);
        }

        if (Body != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(tagName, _bodyTagName!);
        }

        Title = (content, attributes);
    }

    public void ThrowIfNotComplete()
    {
        if (Title == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided([PanelTitleTagHelper.ShortTagName, PanelTitleTagHelper.TagName]);
        }
    }
}
