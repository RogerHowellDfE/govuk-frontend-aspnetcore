using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class PaginationOptions
{
    public IReadOnlyCollection<PaginationOptionsItem>? Items { get; set; }
    public PaginationOptionsPrevious? Previous { get; set; }
    public PaginationOptionsNext? Next { get; set; }
    public IHtmlContent? LandmarkLabel { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public class PaginationOptionsItem
{
    public IHtmlContent? Number { get; set; }
    public IHtmlContent? VisuallyHiddenText { get; set; }
    public IHtmlContent? Href { get; set; }
    public bool? Current { get; set; }
    public bool? Ellipsis { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public class PaginationOptionsPrevious
{
    public string? Text { get; set; }
    public string? LabelText { get; set; }
    public IHtmlContent? Href { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}

public class PaginationOptionsNext
{
    public string? Text { get; set; }
    public string? LabelText { get; set; }
    public IHtmlContent? Href { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
}
