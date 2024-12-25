using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the body in a GDS panel component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = PanelTagHelper.TagName)]
[HtmlTargetElement(ShortTagName, ParentTag = PanelTagHelper.TagName)]
public class PanelBodyTagHelper : TagHelper
{
    internal const string TagName = "govuk-panel-body";
    internal const string ShortTagName = "panel-body";

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var panelContext = context.GetContextItem<PanelContext>();

        var childContent = (await output.GetChildContentAsync()).Snapshot();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        panelContext.SetBody(childContent, new EncodedAttributesDictionary(output.Attributes), output.TagName);

        output.SuppressOutput();
    }
}
