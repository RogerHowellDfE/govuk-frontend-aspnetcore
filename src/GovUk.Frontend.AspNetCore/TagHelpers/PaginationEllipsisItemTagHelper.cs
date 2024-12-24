using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents an ellipsis item in a GDS pagination component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = PaginationTagHelper.TagName)]
[HtmlTargetElement(ShortTagName, ParentTag = PaginationTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.PaginationItemElement)]
public class PaginationEllipsisItemTagHelper : TagHelper
{
    internal const string TagName = "govuk-pagination-ellipsis";
    internal const string ShortTagName = ShortTagNames.Ellipsis;

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var paginationContext = context.GetContextItem<PaginationContext>();

        paginationContext.AddItem(new PaginationOptionsItem { Ellipsis = true }, output.TagName);

        output.SuppressOutput();
    }
}
