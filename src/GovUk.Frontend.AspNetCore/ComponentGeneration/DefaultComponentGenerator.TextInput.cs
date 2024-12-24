using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const bool InputDefaultDisabled = false;
    internal const string InputDefaultType = "text";
    internal const string InputElement = "input";
    internal const string InputPrefixElement = "div";
    internal const string InputSuffixElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateTextInput(TextInputOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var hasError = options.ErrorMessage is not null;
        var hasPrefixOrSuffix = options.Prefix is not null || options.Suffix is not null;
        var describedBy = options.DescribedBy ?? new HtmlString("");

        var formGroup = GenerateFormGroup(options.FormGroup, hasError);

        if (options.Label is not null)
        {
            formGroup.WithAppendedHtml(GenerateLabel(new LabelOptions()
            {
                Html = options.Label.Html,
                Text = options.Label.Text,
                Classes = options.Label.Classes,
                IsPageHeading = options.Label.IsPageHeading,
                Attributes = options.Label.Attributes,
                For = options.Id
            }));
        }

        if (options.Hint is not null)
        {
            var hintId = new HtmlString($"{options.Id}-hint");
            AppendToDescribedBy(ref describedBy, hintId);

            formGroup.WithAppendedHtml(GenerateHint(new HintOptions()
            {
                Id = hintId,
                Classes = options.Hint.Classes,
                Attributes = options.Hint.Attributes,
                Html = options.Hint.Html,
                Text = options.Hint.Text
            }));
        }

        if (options.ErrorMessage is not null)
        {
            Debug.Assert(hasError);

            var errorId = new HtmlString($"{options.Id}-error");
            AppendToDescribedBy(ref describedBy, errorId);

            formGroup.WithAppendedHtml(GenerateErrorMessage(new ErrorMessageOptions()
            {
                Id = errorId,
                Classes = options.ErrorMessage.Classes,
                Attributes = options.ErrorMessage.Attributes,
                Html = options.ErrorMessage.Html,
                Text = options.ErrorMessage.Text,
                VisuallyHiddenText = options.ErrorMessage.VisuallyHiddenText
            }));
        }

        var wrapper = hasPrefixOrSuffix ?
            new HtmlTagBuilder("div").WithCssClass("govuk-input__wrapper") :
            null;

        if (options.Prefix is not null)
        {
            wrapper!.WithAppendedHtml(new HtmlTagBuilder(InputPrefixElement)
                .WithCssClass("govuk-input__prefix")
                .WithCssClasses(ExplodeClasses(options.Prefix.Classes?.ToHtmlString()))
                .WithAttribute("aria-hidden", "true", encodeValue: false)
                .WithAttributes(options.Prefix.Attributes)
                .WithAppendedHtml(GetEncodedTextOrHtml(options.Prefix.Text, options.Prefix.Html)!));
        }

        var input = new HtmlTagBuilder(InputElement)
            .WithCssClass("govuk-input")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .When(hasError, b => b.WithCssClass("govuk-input--error"))
            .WithAttribute("id", options.Id!)
            .WithAttribute("name", options.Name!)
            .WithAttribute("type", options.Type ?? new HtmlString(InputDefaultType))
            .WhenNotNull(options.Spellcheck, (s, b) => b.WithAttribute("spellcheck", s == true ? "true" : "false", encodeValue: false))
            .WithAttributeWhenNotNull(options.Value.NormalizeEmptyString(), "value")
            .When(options.Disabled == true, b => b.WithBooleanAttribute("disabled"))
            .WithAttributeWhenNotNull(describedBy.NormalizeEmptyString(), "aria-describedby")
            .WithAttributeWhenNotNull(options.Autocomplete.NormalizeEmptyString(), "autocomplete")
            .WithAttributeWhenNotNull(options.Pattern.NormalizeEmptyString(), "pattern")
            .WithAttributeWhenNotNull(options.Inputmode.NormalizeEmptyString(), "inputmode")
            .WithAttributes(options.Attributes);

        wrapper?.WithAppendedHtml(input);

        if (options.Suffix is not null)
        {
            wrapper!.WithAppendedHtml(new HtmlTagBuilder(InputSuffixElement)
                .WithCssClass("govuk-input__suffix")
                .WithCssClasses(ExplodeClasses(options.Suffix.Classes?.ToHtmlString()))
                .WithAttribute("aria-hidden", "true", encodeValue: false)
                .WithAttributes(options.Suffix.Attributes)
                .WithAppendedHtml(GetEncodedTextOrHtml(options.Suffix.Text, options.Suffix.Html)!));
        }

        formGroup.WithAppendedHtml(wrapper ?? input);

        return formGroup;
    }
}
