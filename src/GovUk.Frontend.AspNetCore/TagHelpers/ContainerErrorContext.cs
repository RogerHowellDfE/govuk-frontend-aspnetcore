using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class ContainerErrorContext
{
    private readonly List<(IHtmlContent Html, IHtmlContent? Href)> _errors;

    public ContainerErrorContext()
    {
        _errors = new();
    }

    public IReadOnlyCollection<(IHtmlContent Html, IHtmlContent? Href)> Errors => _errors.AsReadOnly();

    public void AddError(IHtmlContent html, IHtmlContent? href)
    {
        ArgumentNullException.ThrowIfNull(html);
        _errors.Add((html, href));
    }
}
