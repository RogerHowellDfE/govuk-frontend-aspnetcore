using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TabsOptions
{
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? IdPrefix { get; set; }
    public IHtmlContent? Title { get; set; }
    public IReadOnlyCollection<TabsOptionsItem>? Items { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public class TabsOptionsItem
{
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? Label { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public TabsOptionsItemPanel? Panel { get; set; }
}

public class TabsOptionsItemPanel
{
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}
