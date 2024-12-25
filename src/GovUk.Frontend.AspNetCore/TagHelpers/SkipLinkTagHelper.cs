using System;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS skip link component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.SkipLinkElement)]
public class SkipLinkTagHelper : TagHelper
{
    internal const string TagName = "govuk-skip-link";

    private const string HrefAttributeName = "href";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="BackLinkTagHelper"/>.
    /// </summary>
    public SkipLinkTagHelper(IComponentGenerator componentGenerator)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// The <c>href</c> attribute for the link.
    /// </summary>
    /// <remarks>
    /// If not specified, <c>#content</c> will be used.
    /// </remarks>
    [HtmlAttributeName(HrefAttributeName)]
    public string? Href { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = (await output.GetChildContentAsync()).Snapshot();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var attributes = new EncodedAttributesDictionary(output.Attributes);
        attributes.Remove("class", out var classes);

        var component = _componentGenerator.GenerateSkipLink(new SkipLinkOptions
        {
            Html = childContent,
            Href = Href.ToHtmlContent(),
            Classes = classes,
            Attributes = attributes
        });

        component.WriteTo(output);
    }
}
