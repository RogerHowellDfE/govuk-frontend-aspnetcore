using System;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorSummaryContextTests
{
    [Fact]
    public void AddItem_AddsItemToItems()
    {
        // Arrange
        var context = new ErrorSummaryContext();

        var item = new ErrorSummaryOptionsErrorItem()
        {
            Html = new HtmlString("An error message"),
            Href = new HtmlString("#TheField")
        };

        // Act
        context.AddItem(item);

        // Assert
        Assert.Collection(
            context.Items,
            item =>
            {
                Assert.Equal("An error message", item.Html?.ToHtmlString());
                Assert.Equal("#TheField", item.Href?.ToHtmlString());
            });
    }

    [Fact]
    public void SetDescription_SetsDescriptionOnContext()
    {
        // Arrange
        var context = new ErrorSummaryContext();

        // Act
        context.SetDescription(new EncodedAttributesDictionary(), html: new HtmlString("Description"));

        // Assert
        Assert.Equal("Description", context.Description?.Html?.ToHtmlString());
    }

    [Fact]
    public void SetDescription_AlreadyGotDescription_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new ErrorSummaryContext();
        context.SetDescription(new EncodedAttributesDictionary(), html: new HtmlString("Existing description"));

        // Act
        var ex = Record.Exception(() => context.SetDescription(new EncodedAttributesDictionary(), html: new HtmlString("Description")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-error-summary-description> element is permitted within each <govuk-error-summary>.", ex.Message);
    }

    [Fact]
    public void SetTitle_SetsTitleOnContext()
    {
        // Arrange
        var context = new ErrorSummaryContext();

        // Act
        context.SetTitle(new EncodedAttributesDictionary(), html: new HtmlString("Title"));

        // Assert
        Assert.Equal("Title", context.Title?.Html?.ToHtmlString());
    }

    [Fact]
    public void SetTitle_AlreadyGotTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new ErrorSummaryContext();
        context.SetTitle(new EncodedAttributesDictionary(), html: new HtmlString("Existing title"));

        // Act
        var ex = Record.Exception(() => context.SetTitle(new EncodedAttributesDictionary(), html: new HtmlString("Title")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-error-summary-title> element is permitted within each <govuk-error-summary>.", ex.Message);
    }
}
