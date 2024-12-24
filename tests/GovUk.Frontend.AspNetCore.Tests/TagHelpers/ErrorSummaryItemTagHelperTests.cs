using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorSummaryItemTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsItemToContext()
    {
        // Arrange
        var errorHtml = "Error message";

        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(errorHtml);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions());
        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        var tagHelper = new ErrorSummaryItemTagHelper(options, dateInputParseErrorsProvider);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            errorSummaryContext.Items,
            item =>
            {
                Assert.Equal(errorHtml, item.Html?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_NoContentOrAspNetFor_ThrowsInvalidOperationException()
    {
        // Arrange
        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
        output.TagMode = TagMode.SelfClosing;

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions());
        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        var tagHelper = new ErrorSummaryItemTagHelper(options, dateInputParseErrorsProvider);

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Content is required when the 'asp-for' attribute is not specified.", ex.Message);
    }

    [Fact]
    public async Task ProcessAsync_AspForSpecified_UsesModelStateErrorMessageForContent()
    {
        // Arrange
        var modelStateErrorMessage = "ModelState error message";

        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
        output.TagMode = TagMode.SelfClosing;

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.Field));

        var viewContext = new ViewContext();

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions());
        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        viewContext.ModelState.AddModelError(nameof(Model.Field), modelStateErrorMessage);

        var tagHelper = new ErrorSummaryItemTagHelper(options, dateInputParseErrorsProvider)
        {
            AspFor = new ModelExpression(nameof(Model.Field), modelExplorer),
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            errorSummaryContext.Items,
            item =>
            {
                Assert.Equal(modelStateErrorMessage, item.Html?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_BothContentAndAspNetForSpecified_UsesContent()
    {
        // Arrange
        var errorHtml = "Error message";
        var modelStateErrorMessage = "ModelState error message";

        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(errorHtml);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.Field));

        var viewContext = new ViewContext();

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions());
        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        viewContext.ModelState.AddModelError(nameof(Model.Field), modelStateErrorMessage);

        var tagHelper = new ErrorSummaryItemTagHelper(options, dateInputParseErrorsProvider)
        {
            AspFor = new ModelExpression(nameof(Model.Field), modelExplorer),
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            errorSummaryContext.Items,
            item =>
            {
                Assert.Equal(errorHtml, item.Html?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_HrefAttributeSpecifiedOrDerived_SetsHrefOnItem()
    {
        // Arrange
        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-item",
            attributes: new TagHelperAttributeList()
            {
                { "href", "#TheField" }
            },
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Error message");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions());
        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        var tagHelper = new ErrorSummaryItemTagHelper(options, dateInputParseErrorsProvider);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            errorSummaryContext.Items,
            item =>
            {
                Assert.Equal("#TheField", item.Href?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_AspForSpecified_GeneratesHrefFromModelExpression()
    {
        // Arrange
        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
        output.TagMode = TagMode.SelfClosing;

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.Field));

        var viewContext = new ViewContext();

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions());
        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        viewContext.ModelState.AddModelError(nameof(Model.Field), "ModelState error message");

        var tagHelper = new ErrorSummaryItemTagHelper(options, dateInputParseErrorsProvider)
        {
            AspFor = new ModelExpression(nameof(Model.Field), modelExplorer),
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            errorSummaryContext.Items,
            item =>
            {
                Assert.Equal("#Field", item.Href?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_AspForSpecifiedForDateField_GeneratesHrefForDateComponentFromModelExpression()
    {
        // Arrange
        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
        output.TagMode = TagMode.SelfClosing;

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.Date));

        var viewContext = new ViewContext();

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions());
        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        var modelName = nameof(Model.Date);
        viewContext.ModelState.AddModelError(modelName, "ModelState error message");
        dateInputParseErrorsProvider.SetErrorsForModel(modelName, DateInputParseErrors.InvalidMonth | DateInputParseErrors.MissingYear);

        var tagHelper = new ErrorSummaryItemTagHelper(options, dateInputParseErrorsProvider)
        {
            AspFor = new ModelExpression(nameof(Model.Date), modelExplorer),
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            errorSummaryContext.Items,
            item =>
            {
                Assert.Equal("#Date.Month", item.Href?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_BothHrefAndAspForSpecified_UsesHrefAttribute()
    {
        // Arrange
        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-item",
            attributes: new TagHelperAttributeList()
            {
                { "href", "#SomeHref" }
            },
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
        output.TagMode = TagMode.SelfClosing;

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.Field));

        var viewContext = new ViewContext();

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions());
        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        viewContext.ModelState.AddModelError(nameof(Model.Field), "ModelState error message");

        var tagHelper = new ErrorSummaryItemTagHelper(options, dateInputParseErrorsProvider)
        {
            AspFor = new ModelExpression(nameof(Model.Field), modelExplorer),
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            errorSummaryContext.Items,
            item =>
            {
                Assert.Equal("#SomeHref", item.Href?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_NoHrefAttributeOrAspFor_SetsNullHrefOnItem()
    {
        // Arrange
        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Error message");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions());
        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        var tagHelper = new ErrorSummaryItemTagHelper(options, dateInputParseErrorsProvider);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            errorSummaryContext.Items,
            item =>
            {
                Assert.Null(item.Href);
            });
    }

    private class Model
    {
        public DateOnly? Date { get; set; }
        public string? Field { get; set; }
    }
}
