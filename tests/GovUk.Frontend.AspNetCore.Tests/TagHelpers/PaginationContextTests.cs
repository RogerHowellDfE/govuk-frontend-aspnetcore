using System;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PaginationContextTests
{
    [Fact]
    public void AddItem_AlreadyGotNext_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PaginationContext();
        context.SetNext(new PaginationOptionsNext(), PaginationNextTagHelper.ShortTagName);

        // Act
        var ex = Record.Exception(() => context.AddItem(new PaginationOptionsItem() { Ellipsis = true }, PaginationEllipsisItemTagHelper.ShortTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<ellipsis> must be specified before <next>.", ex.Message);
    }

    [Fact]
    public void AddItem_WithCurrentItemAndAlreadyGotCurrentItem_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PaginationContext();
        context.AddItem(
            new PaginationOptionsItem()
            {
                Number = new HtmlString("1"),
                Current = true
            },
            tagName: PaginationItemTagHelper.ShortTagName);

        // Act
        var ex = Record.Exception(() => context.AddItem(
            new PaginationOptionsItem()
            {
                Number = new HtmlString("2"),
                Current = true
            },
            tagName: PaginationItemTagHelper.ShortTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one Current <item> is permitted.", ex.Message);
    }

    [Fact]
    public void AddItem_ValidRequest_AddsItemToContext()
    {
        // Arrange
        var context = new PaginationContext();
        var item = new PaginationOptionsItem()
        {
            Number = new HtmlString("1"),
            Href = new HtmlString("#")
        };

        // Act
        context.AddItem(item, tagName: PaginationItemTagHelper.ShortTagName);

        // Assert
        Assert.Collection(context.Items, i => Assert.Same(item, i));
    }

    [Fact]
    public void SetNext_AlreadyGotNext_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PaginationContext();
        context.SetNext(new PaginationOptionsNext(), tagName: PaginationNextTagHelper.ShortTagName);

        // Act
        var ex = Record.Exception(() => context.SetNext(new PaginationOptionsNext(), tagName: PaginationNextTagHelper.ShortTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <next> or <govuk-pagination-next> element is permitted within each <govuk-pagination>.", ex.Message);
    }

    [Fact]
    public void SetNext_ValidRequest_SetsNextOnContext()
    {
        // Arrange
        var context = new PaginationContext();
        var next = new PaginationOptionsNext();

        // Act
        context.SetNext(next, tagName: PaginationNextTagHelper.ShortTagName);

        // Assert
        Assert.Same(next, context.Next);
    }

    [Fact]
    public void SetPrevious_AlreadyGotNext_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PaginationContext();
        context.SetNext(new PaginationOptionsNext(), tagName: PaginationNextTagHelper.ShortTagName);

        // Act
        var ex = Record.Exception(() => context.SetPrevious(new PaginationOptionsPrevious(), tagName: PaginationPreviousTagHelper.ShortTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<previous> must be specified before <next>.", ex.Message);
    }

    [Fact]
    public void SetPrevious_AlreadyGotPrevious_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PaginationContext();
        context.SetPrevious(new PaginationOptionsPrevious(), tagName: PaginationPreviousTagHelper.ShortTagName);

        // Act
        var ex = Record.Exception(() => context.SetPrevious(new PaginationOptionsPrevious(), tagName: PaginationPreviousTagHelper.ShortTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <previous> or <govuk-pagination-previous> element is permitted within each <govuk-pagination>.", ex.Message);
    }

    [Fact]
    public void SetPrevious_AlreadyGotItems_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PaginationContext();
        context.AddItem(
            new PaginationOptionsItem()
            {
                Number = new HtmlString("1"),
                Current = true
            },
            tagName: PaginationItemTagHelper.ShortTagName);

        // Act
        var ex = Record.Exception(() => context.SetPrevious(new PaginationOptionsPrevious(), tagName: PaginationPreviousTagHelper.ShortTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<previous> must be specified before <item>.", ex.Message);
    }

    [Fact]
    public void SetPrevious_ValidRequest_SetsPreviousOnContext()
    {
        // Arrange
        var context = new PaginationContext();
        var previous = new PaginationOptionsPrevious();

        // Act
        context.SetPrevious(previous, tagName: PaginationPreviousTagHelper.ShortTagName);

        // Assert
        Assert.Same(previous, context.Previous);
    }
}
