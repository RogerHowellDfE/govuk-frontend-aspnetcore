using System;
using System.Collections.Generic;
using System.Linq;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class PaginationContext
{
    private readonly List<PaginationOptionsItem> _items = new();
    private string? _firstItemTagName;
    private string? _nextTagName;

    public IReadOnlyCollection<PaginationOptionsItem> Items => _items.AsReadOnly();

    public PaginationOptionsNext? Next { get; private set; }

    public PaginationOptionsPrevious? Previous { get; private set; }

    private static string[] PreviousTagNames = [PaginationPreviousTagHelper.ShortTagName, PaginationPreviousTagHelper.TagName];

    private static string[] NextTagNames = [PaginationNextTagHelper.ShortTagName, PaginationNextTagHelper.TagName];

    public void AddItem(PaginationOptionsItem item, string tagName)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (Next is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(tagName, _nextTagName!);
        }

        // Only one 'current' item is allowed.
        if (item.Current == true && _items.Any(i => i.Current == true))
        {
            throw new InvalidOperationException($"Only one Current <{tagName}> is permitted.");
        }

        _items.Add(item);
        _firstItemTagName ??= tagName;
    }

    public void SetNext(PaginationOptionsNext next, string tagName)
    {
        ArgumentNullException.ThrowIfNull(next);

        if (Next is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(NextTagNames, PaginationTagHelper.TagName);
        }

        Next = next;
        _nextTagName = tagName;
    }

    public void SetPrevious(PaginationOptionsPrevious previous, string tagName)
    {
        ArgumentNullException.ThrowIfNull(previous);

        if (_items.Count != 0)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(tagName, _firstItemTagName!);
        }

        if (Next is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(tagName, _nextTagName!);
        }

        if (Previous is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(PreviousTagNames, PaginationTagHelper.TagName);
        }

        Previous = previous;
    }
}
