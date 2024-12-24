using System;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal abstract class FormGroupFieldsetContext2
{
    internal record LegendInfo(bool IsPageHeading, EncodedAttributesDictionary Attributes, IHtmlContent? Html);

    private readonly string _fieldsetTagName;
    private readonly string _legendTagName;
    private readonly ModelExpression? _for;

    // internal for testing
    internal LegendInfo? Legend;
    internal string? DescribedBy;

    private EncodedAttributesDictionary _attributes;

    protected FormGroupFieldsetContext2(
        string fieldsetTagName,
        string legendTagName,
        EncodedAttributesDictionary attributes,
        ModelExpression? @for,
        string? describedBy)
    {
        ArgumentNullException.ThrowIfNull(fieldsetTagName);
        ArgumentNullException.ThrowIfNull(legendTagName);
        ArgumentNullException.ThrowIfNull(attributes);
        _fieldsetTagName = fieldsetTagName;
        _legendTagName = legendTagName;
        _attributes = attributes;
        _for = @for;
        DescribedBy = describedBy;
    }

    public FieldsetOptions GetFieldsetOptions(IModelHelper modelHelper)
    {
        ThrowIfNotComplete();

        var legendHtml = Legend?.Html ??
            new HtmlString(modelHelper.GetDisplayName(_for!.ModelExplorer, _for.Name));

        return new FieldsetOptions()
        {
            DescribedBy = DescribedBy.ToHtmlContent(),
            Legend = new FieldsetOptionsLegend()
            {
                Text = null,
                Html = legendHtml,
                IsPageHeading = Legend?.IsPageHeading,
                Attributes = new EncodedAttributesDictionaryBuilder(Legend?.Attributes.Clone() ?? new()).Without("class", out var legendClasses),
                Classes = legendClasses
            },
            Role = null,
            Html = null,
            Attributes = new EncodedAttributesDictionaryBuilder(_attributes).Without("class", out var classes),
            Classes = classes
        };
    }

    public virtual void SetLegend(bool isPageHeading, EncodedAttributesDictionary attributes, IHtmlContent? html)
    {
        if (Legend != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(_legendTagName, _fieldsetTagName);
        }

        Legend = new LegendInfo(isPageHeading, attributes, html);
    }

    public void ThrowIfNotComplete()
    {
        if (Legend == null && _for is null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(_legendTagName);
        }
    }
}
