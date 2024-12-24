using System;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TextInputContextTests
{
    [Fact]
    public void SetErrorMessage_AlreadyGotPrefix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var prefixTagName = ShortTagNames.Prefix;
        var errorMessageTagName = ShortTagNames.ErrorMessage;
        context.SetPrefix(new EncodedAttributesDictionary(), new HtmlString("Prefix"), prefixTagName);

        // Act
        var ex = Record.Exception(() => context.SetErrorMessage(null, new EncodedAttributesDictionary(), new HtmlString("Error"), errorMessageTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{errorMessageTagName}> must be specified before <{prefixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetErrorMessage_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var suffixTagName = ShortTagNames.Suffix;
        var errorMessageTagName = ShortTagNames.ErrorMessage;
        context.SetSuffix(new EncodedAttributesDictionary(), new HtmlString("Prefix"), suffixTagName);

        // Act
        var ex = Record.Exception(() => context.SetErrorMessage(null, new EncodedAttributesDictionary(), new HtmlString("Error"), errorMessageTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{errorMessageTagName}> must be specified before <{suffixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetHint_AlreadyGotPrefix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var prefixTagName = ShortTagNames.Prefix;
        var hintTagName = ShortTagNames.Hint;
        context.SetPrefix(new EncodedAttributesDictionary(), new HtmlString("Prefix"), prefixTagName);

        // Act
        var ex = Record.Exception(() => context.SetHint(new EncodedAttributesDictionary(), new HtmlString("Error"), hintTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{hintTagName}> must be specified before <{prefixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetHint_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var suffixTagName = ShortTagNames.Suffix;
        var hintTagName = ShortTagNames.ErrorMessage;
        context.SetSuffix(new EncodedAttributesDictionary(), new HtmlString("Prefix"), suffixTagName);

        // Act
        var ex = Record.Exception(() => context.SetHint(new EncodedAttributesDictionary(), new HtmlString("Error"), hintTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{hintTagName}> must be specified before <{suffixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetLabel_AlreadyGotPrefix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var prefixTagName = ShortTagNames.Prefix;
        var labelTagName = ShortTagNames.Label;
        context.SetPrefix(new EncodedAttributesDictionary(), new HtmlString("Prefix"), prefixTagName);

        // Act
        var ex = Record.Exception(() => context.SetLabel(false, new EncodedAttributesDictionary(), new HtmlString("Error"), labelTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{labelTagName}> must be specified before <{prefixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetLabel_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var suffixTagName = ShortTagNames.Suffix;
        var labelTagName = ShortTagNames.Label;
        context.SetSuffix(new EncodedAttributesDictionary(), new HtmlString("Prefix"), suffixTagName);

        // Act
        var ex = Record.Exception(() => context.SetLabel(false, new EncodedAttributesDictionary(), new HtmlString("Error"), labelTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{labelTagName}> must be specified before <{suffixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetPrefix_AlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var prefixTagName = ShortTagNames.Prefix;
        context.SetPrefix(new EncodedAttributesDictionary(), new HtmlString("Existing prefix"), prefixTagName);

        // Act
        var ex = Record.Exception(() => context.SetPrefix(new EncodedAttributesDictionary(), new HtmlString("Prefix"), prefixTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"Only one <prefix> or <govuk-input-prefix> element is permitted within each <govuk-input>.", ex.Message);
    }

    [Fact]
    public void SetPrefix_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var prefixTagName = ShortTagNames.Prefix;
        var suffixTagName = ShortTagNames.Suffix;
        context.SetSuffix(new EncodedAttributesDictionary(), new HtmlString("Suffix"), suffixTagName);

        // Act
        var ex = Record.Exception(() => context.SetPrefix(new EncodedAttributesDictionary(), new HtmlString("Prefix"), prefixTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{prefixTagName}> must be specified before <{suffixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetSuffix_AlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var suffixTagName = ShortTagNames.Suffix;
        context.SetSuffix(new EncodedAttributesDictionary(), new HtmlString("Existing prefix"), suffixTagName);

        // Act
        var ex = Record.Exception(() => context.SetSuffix(new EncodedAttributesDictionary(), new HtmlString("Prefix"), suffixTagName));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"Only one <suffix> or <govuk-input-suffix> element is permitted within each <govuk-input>.", ex.Message);
    }
}
