# Panel

[GDS Panel component](https://design-system.service.gov.uk/components/panel/)

## Example

```razor
<govuk-panel heading-level="2">
    <panel-title>Application complete</panel-title>
    <panel-body>
        Your reference number<br><strong>HDJ2123F</strong>
    </panel-body>
</govuk-panel>
```

![Panel](../images/panel.png)

## API

### `<govuk-panel>`

| Attribute | Type | Description                                                                                     |
| --- | --- |-------------------------------------------------------------------------------------------------|
| `heading-level` | `int` | The heading level. Must be between `1` and `6` (inclusive). If not specified, `1` will be used. |

### `<panel-title>`

*Required*\
The content is the HTML to use within the panel title.\
Must be inside a `<govuk-panel>` element.

### `<panel-body>`

The content is the HTML to use within the panel content.\
Must be inside a `<govuk-panel>` element.
