using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PaginationItemTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsItemToContextItems()
    {
        // Arrange
        var paginationContext = new PaginationContext();

        var context = new TagHelperContext(
            tagName: "govuk-pagination-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(PaginationContext), paginationContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-pagination-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.AppendHtml("Page 42");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new PaginationItemTagHelper()
        {
            Current = true
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            paginationContext.Items,
            item =>
            {
                Assert.True(item.Current);
                Assert.Equal("Page 42", item.Number?.ToHtmlString());
            });
    }
}
