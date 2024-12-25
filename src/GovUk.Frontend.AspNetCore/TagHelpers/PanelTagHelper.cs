using System;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS panel component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.PanelElement)]
[RestrictChildren(PanelTitleTagHelper.TagName, PanelTitleTagHelper.ShortTagName, PanelBodyTagHelper.TagName, PanelBodyTagHelper.ShortTagName)]
public class PanelTagHelper : TagHelper
{
    internal const string TagName = "govuk-panel";

    private const string HeadingLevelAttributeName = "heading-level";

    private readonly IComponentGenerator _componentGenerator;
    private int? _headingLevel;

    /// <summary>
    /// Creates a new <see cref="BackLinkTagHelper"/>.
    /// </summary>
    public PanelTagHelper(IComponentGenerator componentGenerator)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// The heading level.
    /// </summary>
    /// <remarks>
    /// Must be between <c>1</c> and <c>6</c> (inclusive). If not specified, <c>1</c> will be used.
    /// </remarks>
    [HtmlAttributeName(HeadingLevelAttributeName)]
    public int? HeadingLevel
    {
        get => _headingLevel;
        set
        {
            if (value is not null && (
                value < DefaultComponentGenerator.PanelMinHeadingLevel ||
                value > DefaultComponentGenerator.PanelMaxHeadingLevel))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"{nameof(HeadingLevel)} must be between {DefaultComponentGenerator.PanelMinHeadingLevel} and {DefaultComponentGenerator.PanelMaxHeadingLevel}.");
            }

            _headingLevel = value;
        }
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var panelContext = new PanelContext();

        using (context.SetScopedContextItem(panelContext))
        {
            await output.GetChildContentAsync();
        }

        panelContext.ThrowIfNotComplete();

        var attributes = new EncodedAttributesDictionary(output.Attributes);
        attributes.Remove("class", out var classes);

        var component = _componentGenerator.GeneratePanel(new PanelOptions
        {
            TitleHtml = panelContext.Title?.Content,
            TitleAttributes = panelContext.Title?.Attributes,
            HeadingLevel = HeadingLevel,
            Html = panelContext.Body?.Content,
            BodyAttributes = panelContext.Body?.Attributes,
            Classes = classes,
            Attributes = attributes
        });

        component.WriteTo(output);
    }
}
