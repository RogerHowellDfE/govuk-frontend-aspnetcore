using System;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS pagination component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(
    PaginationPreviousTagHelper.TagName,
    PaginationPreviousTagHelper.ShortTagName,
    PaginationItemTagHelper.TagName,
    PaginationItemTagHelper.ShortTagName,
    PaginationEllipsisItemTagHelper.TagName,
    PaginationEllipsisItemTagHelper.ShortTagName,
    PaginationNextTagHelper.TagName,
    PaginationNextTagHelper.ShortTagName)]
[OutputElementHint(DefaultComponentGenerator.PaginationElement)]
public class PaginationTagHelper : TagHelper
{
    internal const string TagName = "govuk-pagination";

    private const string LandmarkLabelAttributeName = "landmark-label";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="BackLinkTagHelper"/>.
    /// </summary>
    public PaginationTagHelper(IComponentGenerator componentGenerator)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// The label for the navigation landmark that wraps the pagination.
    /// </summary>
    /// <remarks>
    /// If not specified, 'Pagination' will be used.
    /// </remarks>
    [HtmlAttributeName(LandmarkLabelAttributeName)]
    public string? LandmarkLabel { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var paginationContext = new PaginationContext();

        using (context.SetScopedContextItem(paginationContext))
        {
            await output.GetChildContentAsync();
        }

        var attributes = new EncodedAttributesDictionary(output.Attributes);
        attributes.Remove("class", out var classes);

        var component = _componentGenerator.GeneratePagination(new PaginationOptions
        {
            Items = paginationContext.Items,
            Previous = paginationContext.Previous,
            Next = paginationContext.Next,
            LandmarkLabel = LandmarkLabel.ToHtmlContent(),
            Classes = classes,
            Attributes = attributes
        });

        component.WriteTo(output);
    }
}
