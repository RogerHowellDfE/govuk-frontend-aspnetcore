using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS inset text component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.InsetTextElement)]
public class InsetTextTagHelper : TagHelper
{
    internal const string TagName = "govuk-inset-text";

    private const string IdAttributeName = "id";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="InsetTextTagHelper"/>.
    /// </summary>
    public InsetTextTagHelper(IComponentGenerator componentGenerator)
    {
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// The <c>id</c> attribute.
    /// </summary>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

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

        var component = _componentGenerator.GenerateInsetText(new InsetTextOptions()
        {
            Id = Id.ToHtmlContent(),
            Text = null,
            Html = childContent,
            Classes = classes,
            Attributes = attributes
        });

        component.WriteTo(output);
    }
}
