using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class CharacterCountOptions
{
    public IHtmlContent? Id { get; set; }
    public IHtmlContent? Name { get; set; }
    public int? Rows { get; set; }
    public IHtmlContent? Value { get; set; }
    public int? MaxLength { get; set; }
    public int? MaxWords { get; set; }
    public int? Threshold { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public IHtmlContent? Classes { get; set; }
    public bool? Spellcheck { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }
    public CharacterCountCountOptionsMessage? CountMessage { get; set; }
    public IHtmlContent? TextareaDescriptionText { get; set; }
    public CharacterCountOptionsLocalizedText? CharactersUnderLimitText { get; set; }
    public IHtmlContent? CharactersAtLimitText { get; set; }
    public CharacterCountOptionsLocalizedText? CharactersOverLimitText { get; set; }
    public CharacterCountOptionsLocalizedText? WordsUnderLimitText { get; set; }
    public IHtmlContent? WordsAtLimitText { get; set; }
    public CharacterCountOptionsLocalizedText? WordsOverLimitText { get; set; }
}

public class CharacterCountCountOptionsMessage
{
    public IHtmlContent? Classes { get; set; }
}

public class CharacterCountOptionsLocalizedText
{
    public IHtmlContent? One { get; set; }
    public IHtmlContent? Other { get; set; }
}
