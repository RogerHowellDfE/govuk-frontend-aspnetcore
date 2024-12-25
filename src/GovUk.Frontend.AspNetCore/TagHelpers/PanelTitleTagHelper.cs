using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents the title in a GDS panel component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = PanelTagHelper.TagName)]
[HtmlTargetElement(ShortTagName, ParentTag = PanelTagHelper.TagName)]
public class PanelTitleTagHelper : TagHelper
{
    internal const string TagName = "govuk-panel-title";
    internal const string ShortTagName = "panel-title";

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var panelContext = context.GetContextItem<PanelContext>();

        var childContent = (await output.GetChildContentAsync()).Snapshot();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        panelContext.SetTitle(childContent, new EncodedAttributesDictionary(output.Attributes), output.TagName);

        output.SuppressOutput();
    }
}
