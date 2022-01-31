namespace ModalBuilderUtil;

public class DbComponent
{
	public int? DbComponentId { get; set; }
	public int DbActionRowId { get; set; }
	public DbActionRow DbActionRow { get; set; }

	// Shared
	public ComponentType Type { get; set; }
	public string? CustomId { get; set; }
	public string? Label { get; set; }
	public bool? Disabled { get; set; }
	public string? Placeholder { get; set; }
	public int? Min { get; set; }
	public int? Max { get; set; }

	// Button
	public ButtonStyle ButtonStyle { get; set; }
	public string? Emote { get; set; }
	public string? Url { get; set; }

	// Select Menu
	public List<DbSelectOption> SelectOptions { get; set; } = new();

	// Text Input
	public TextInputStyle TextInputStyle { get; set; }
	public bool? Required { get; set; }
	public string? Value { get; set; }

	public DbComponent FromButton(ButtonComponent button)
	{
		Type = ComponentType.Button;
		CustomId = button.CustomId;
		Label = button.Label;
		Disabled = button.IsDisabled;
		ButtonStyle = button.Style;
		Emote = button.Emote.Name;
		Url = button.Url;

		return this;
	}

	public DbComponent FromSelectMenu(SelectMenuComponent select)
	{
		Type = ComponentType.SelectMenu;
		CustomId = select.CustomId;
		Disabled = select.IsDisabled;
		Placeholder = select.Placeholder;
		Min = select.MaxValues;
		Max = select.MaxValues;
		SelectOptions = select.Options.Select(x => new DbSelectOption().FromSelectOption(x)).ToList();

		return this;
	}

	public DbComponent FromTextInput(TextInputComponent text)
	{
		Type = ComponentType.TextInput;
		CustomId = text.CustomId;
		Label = text.Label;
		Placeholder = text.Placeholder;
		Min = text.MinLength;
		Max = text.MaxLength;
		TextInputStyle = text.Style;
		Required = text.Required;
		Value = text.Value;

		return this;
	}

	public string GenerateCode()
	{
		string code;

		switch (Type)
		{
			case ComponentType.Button:
				{
					code = "    .AddButton(new() ButtonBuilder\n" +
						$"        .WithCustomId(\"{CustomId}\")" +
						$"        .WithLabel(\"{Label}\")" +
						$"        .WithStyle(ButtonStyle.{ButtonStyle})\n";
					if (Disabled != null) code += $"        .WithDisabled({Disabled})\n";
					if (!string.IsNullOrWhiteSpace(Emote)) code += $"        .WithEmote(Emote.Parse{Emote})\n";
					if (!string.IsNullOrWhiteSpace(Url)) code += $"        .WithUrl(\"{Url}\")\n";
				}
				break;
			case ComponentType.SelectMenu:
				{
					code = "    .AddSelectMenu(new SelectMenuBuilder()\n" +
						$"        .WithCustomId(\"{CustomId}\")\n";
					if (!string.IsNullOrWhiteSpace(Placeholder)) code += $"        .WithPlaceholder(\"{Placeholder}\")\n";
					if (Min is not null) code += $"        .WithMinValues({Min})\n";
					if (Max is not null) code += $"        .WithMaxValues({Max})\n";
					if (Disabled != null) code += $"        .WithDisabled({Disabled})\n";
					SelectOptions.ForEach(x => code += x.GenerateBuilder());
				}
				break;
			case ComponentType.TextInput:
				{
					code = "    .AddTextInput(new TextInputBuilder()\n" +
						$"        .WithLabel(\"{Label}\")\n" +
						$"        .WithStyle(TextInputStyle.{TextInputStyle})\n" +
						$"        .WithCustomId(\"{CustomId}\")\n";
					if (Min is not null) code += $"        .WithMinLength({Min})\n";
					if (Max is not null) code += $"        .WithMaxLength({Max})\n";
					if (!string.IsNullOrWhiteSpace(Placeholder)) code += $"        .WithPlaceholder(\"{Placeholder}\")\n";
					if (Required != null) code += $"        .WithRequired({Required});\n";
				}
				break;
			default:
				code = "";
				break;
		}

		return code[..^1] + ")\n";
	}
}