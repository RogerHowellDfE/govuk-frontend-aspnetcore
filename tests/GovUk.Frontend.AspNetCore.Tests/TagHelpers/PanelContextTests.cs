using System;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PanelContextTests
{
    [Fact]
    public void SetBody_AlreadyGotBody_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PanelContext();
        context.SetBody(new HtmlString("Body"), new EncodedAttributesDictionary(), PanelBodyTagHelper.ShortTagName);

        // Act
        var ex = Record.Exception(() => context.SetBody(new HtmlString("Body"), new EncodedAttributesDictionary(), PanelBodyTagHelper.ShortTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <panel-body> element is permitted within each <govuk-panel>.", ex.Message);
    }

    [Fact]
    public void SetTitle_AlreadyGotBody_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PanelContext();
        context.SetBody(new HtmlString("Body"), new EncodedAttributesDictionary(), PanelBodyTagHelper.ShortTagName);

        // Act
        var ex = Record.Exception(() => context.SetTitle(new HtmlString("Title"), new EncodedAttributesDictionary(), PanelTitleTagHelper.ShortTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<panel-title> must be specified before <panel-body>.", ex.Message);
    }

    [Fact]
    public void SetTitle_AlreadyGotTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PanelContext();
        context.SetTitle(new HtmlString("Title"), new EncodedAttributesDictionary(), PanelTitleTagHelper.ShortTagName);

        // Act
        var ex = Record.Exception(() => context.SetTitle(new HtmlString("Title"), new EncodedAttributesDictionary(), PanelTitleTagHelper.ShortTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <panel-title> element is permitted within each <govuk-panel>.", ex.Message);
    }
}
