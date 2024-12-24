using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorSummaryTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var disableAutoFocus = true;
        var titleHtml = "Title";
        var descriptionHtml = "Description";
        var errorItems = new[]
        {
            new ErrorSummaryOptionsErrorItem()
            {
                Html = new HtmlString("First message"),
                Href = new HtmlString("#Field1")
            },
            new ErrorSummaryOptionsErrorItem()
            {
                Html = new HtmlString("Second message"),
                Href = new HtmlString("#Field2")
            }
        };

        var context = new TagHelperContext(
            tagName: "govuk-error-summary",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

                errorSummaryContext.SetTitle(new EncodedAttributesDictionary(), new HtmlString(titleHtml));
                errorSummaryContext.SetDescription(new EncodedAttributesDictionary(), new HtmlString(descriptionHtml));

                foreach (var error in errorItems)
                {
                    errorSummaryContext.AddItem(error);
                }

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorSummaryOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorSummary(It.IsAny<ErrorSummaryOptions>())).Callback<ErrorSummaryOptions>(o => actualOptions = o);

        var tagHelper = new ErrorSummaryTagHelper(componentGeneratorMock.Object)
        {
            DisableAutoFocus = disableAutoFocus,
            ViewContext = new ViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Null(actualOptions!.TitleText);
        Assert.Equal(titleHtml, actualOptions.TitleHtml?.ToHtmlString());
        Assert.Null(actualOptions.DescriptionText);
        Assert.Equal(descriptionHtml, actualOptions.DescriptionHtml?.ToHtmlString());
        Assert.Equal(actualOptions.ErrorList, errorItems);
        Assert.Null(actualOptions.Classes);
        Assert.NotNull(actualOptions.Attributes);
        Assert.Empty(actualOptions.Attributes);
        Assert.Equal(disableAutoFocus, actualOptions.DisableAutoFocus);
        Assert.NotNull(actualOptions.TitleAttributes);
        Assert.Empty(actualOptions.TitleAttributes);
        Assert.NotNull(actualOptions.DescriptionAttributes);
        Assert.Empty(actualOptions.DescriptionAttributes);
    }
}
