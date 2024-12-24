using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the link to the previous page in a GDS pagination component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = PaginationTagHelper.TagName)]
[HtmlTargetElement(ShortTagName, ParentTag = PaginationTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.PaginationPreviousElement)]
public class PaginationPreviousTagHelper : TagHelper
{
    internal const string TagName = "govuk-pagination-previous";
    internal const string ShortTagName = ShortTagNames.Previous;

    private const string LabelTextAttributeName = "label-text";
    private const string LinkAttributesPrefix = "link-";

    /// <summary>
    /// The optional label that goes underneath the link to the previous page, providing further context for the user about where the link goes.
    /// </summary>
    [HtmlAttributeName(LabelTextAttributeName)]
    public string? LabelText { get; set; }

    /// <summary>
    /// Additional attributes to add to the generated <c>a</c> element.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = LinkAttributesPrefix)]
    public IDictionary<string, string?> LinkAttributes { get; set; } = new Dictionary<string, string?>();

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var paginationContext = context.GetContextItem<PaginationContext>();

        var childContent = output.TagMode == TagMode.StartTagAndEndTag ?
            (await output.GetChildContentAsync()).Snapshot() :
            null;

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var attributes = new EncodedAttributesDictionary(output.Attributes);
        attributes.Remove("href", out var href);

        paginationContext.SetPrevious(
            new PaginationOptionsPrevious()
            {
                Attributes = EncodedAttributesDictionary.FromDictionaryWithEncodedValues(LinkAttributes),
                Href = href,
                LabelText = LabelText.ToHtmlContent(),
                ContainerAttributes = attributes,
                Html = childContent
            },
            output.TagName);

        output.SuppressOutput();
    }
}
