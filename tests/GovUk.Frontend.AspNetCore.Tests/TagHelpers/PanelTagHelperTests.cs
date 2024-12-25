using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PanelTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var title = "title";
        var body = "body";

        var context = new TagHelperContext(
            tagName: "govuk-panel",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-panel",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var panelContext = (PanelContext)context.Items[typeof(PanelContext)];
                panelContext.SetTitle(new HtmlString(title), new EncodedAttributesDictionary(), PanelTitleTagHelper.ShortTagName);
                panelContext.SetBody(new HtmlString(body), new EncodedAttributesDictionary(), PanelBodyTagHelper.ShortTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        PanelOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GeneratePanel(It.IsAny<PanelOptions>())).Callback<PanelOptions>(o => actualOptions = o);

        var tagHelper = new PanelTagHelper(componentGeneratorMock.Object)
        {
            HeadingLevel = 3
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(3, actualOptions.HeadingLevel);
        Assert.Equal(title, actualOptions.TitleHtml?.ToHtmlString());
        Assert.Equal(body, actualOptions.Html?.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_MissingTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-panel",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-panel",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var panelContext = (PanelContext)context.Items[typeof(PanelContext)];
                panelContext.SetBody(new HtmlString("Body"), new EncodedAttributesDictionary(), PanelBodyTagHelper.ShortTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new PanelTagHelper(new DefaultComponentGenerator())
        {
            HeadingLevel = 3
        };

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("A <panel-title> or <govuk-panel-title> element must be provided.", ex.Message);
    }
}
