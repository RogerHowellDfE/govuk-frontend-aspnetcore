using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class BreadcrumbsOptions
{
    public bool? CollapseOnMobile { get; set; }
    public string? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public IReadOnlyCollection<BreadcrumbsOptionsItem>? Items { get; set; }
}

public class BreadcrumbsOptionsItem
{
    public IHtmlContent? Html { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Href { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}
