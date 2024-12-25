using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class SkipLinkTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var href = "#main";
        var content = "Link content";

        var context = new TagHelperContext(
            tagName: "govuk-skip-link",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-skip-link",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(content);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        SkipLinkOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateSkipLink(It.IsAny<SkipLinkOptions>())).Callback<SkipLinkOptions>(o => actualOptions = o);

        var tagHelper = new SkipLinkTagHelper(componentGeneratorMock.Object)
        {
            Href = href
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(href, actualOptions.Href?.ToHtmlString());
        Assert.Equal(content, actualOptions.Html?.ToHtmlString());
    }
}
