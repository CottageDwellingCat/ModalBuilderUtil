namespace ModalBuilderUtil;

public class DbModal
{
	public int DbModalId { get; set; }
	public string? Title { get; set; }
	public string? CustomId { get; set; }
	public ulong UserID { get; set; }

	public List<DbActionRow> ActionRows { get; set; } = new();

	public DbModal() { }
	public DbModal(ModalBuilder builder)
	{
		Title = builder.Title;
		CustomId = builder.CustomId;
		builder.Components?.ActionRows?.ForEach(x =>
		{
			var row = new DbActionRow();
			x.Components.ForEach(x => row.Components.Add(x switch
			{
				ButtonComponent button => new DbComponent().FromButton(button),
				SelectMenuComponent select => new DbComponent().FromSelectMenu(select),
				TextInputComponent textInput => new DbComponent().FromTextInput(textInput),
				_ => throw new NotSupportedException($"{x.Type} componets are unsupported.")
			}));
			ActionRows.Add(row);
		});
	}

	public string GenerateCode()
	{
		string code = "";
		code = "var mb = new ModalBuilder();\n" +
			$"    .WithTitle(\"{Title}\")\n" +
			$"    .WithCustomId(\"{CustomId}\")\n";
		ActionRows.ForEach(x => x.Components.ForEach(x => code += x.GenerateCode()));
		return code[..^1] + ";\n";
	}

	public ModalBuilder GetBuilder()
	{
		ModalBuilder mb = new()
		{
			CustomId = CustomId,
			Title = Title,
		};

		mb.AddComponents(ActionRows
			.SelectMany(x => x.Components)
			.Select(x => x.Type switch
			{
				ComponentType.Button => (IMessageComponent)new ButtonBuilder(x.Label, x.CustomId, x.ButtonStyle, x.Url,
					Emote.Parse(x.Emote), x.Disabled ?? false).Build(),
				ComponentType.SelectMenu => (IMessageComponent)new SelectMenuBuilder(x.CustomId,
					x.SelectOptions.Select(x => new SelectMenuOptionBuilder(x.Label, x.Value, x.Description,
					Emote.Parse(x.Emote), x.Default ?? false)).ToList(), x.Placeholder, x.Max ?? 1, x.Min ?? 1,
					x.Disabled ?? false).Build(),
				ComponentType.TextInput => (IMessageComponent)new TextInputBuilder(x.Label, x.CustomId, x.TextInputStyle,
					x.Placeholder, x.Min, x.Max, x.Required, x.Value).Build(),
				_ => throw new NotSupportedException("That component type is not supported.")
			}).ToList(), 0);

		return mb;
	}
}
