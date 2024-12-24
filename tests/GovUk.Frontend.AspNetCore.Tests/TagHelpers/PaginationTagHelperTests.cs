using System;
using System.Collections.Generic;
using System.Linq;
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

public class PaginationTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var previous = new PaginationOptionsPrevious
        {
            Html = new HtmlString("Previous page"),
            Href = new HtmlString("/1")
        };

        var items = new[]
        {
            new PaginationOptionsItem()
            {
                Href = new HtmlString("/1"),
                Number = new HtmlString("1")
            },
            new PaginationOptionsItem()
            {
                Href = new HtmlString("/2"),
                Number = new HtmlString("2"),
                Current = true
            },
            new PaginationOptionsItem()
            {
                Href = new HtmlString("/3"),
                Number = new HtmlString("3")
            },
            new PaginationOptionsItem()
            {
                Ellipsis = true
            }
        };

        var next = new PaginationOptionsNext
        {
            Html = new HtmlString("Next page"),
            Href = new HtmlString("/3")
        };

        var context = new TagHelperContext(
            tagName: "govuk-pagination",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-pagination",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var paginationContext = context.GetContextItem<PaginationContext>();

                paginationContext.SetPrevious(previous, tagName: PaginationPreviousTagHelper.ShortTagName);

                foreach (var item in items)
                {
                    paginationContext.AddItem(item, tagName: item.Ellipsis == true ? PaginationEllipsisItemTagHelper.ShortTagName : PaginationItemTagHelper.ShortTagName);
                }

                paginationContext.SetNext(next, tagName: PaginationNextTagHelper.ShortTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        PaginationOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GeneratePagination(It.IsAny<PaginationOptions>())).Callback<PaginationOptions>(o => actualOptions = o);

        var tagHelper = new PaginationTagHelper(componentGeneratorMock.Object);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Same(previous, actualOptions.Previous);
        Assert.NotNull(actualOptions.Items);
        Assert.Collection(actualOptions.Items, items.Select(i => (Action<PaginationOptionsItem>)(item => Assert.Same(i, item))).ToArray());
        Assert.Same(next, actualOptions.Next);
    }
}
