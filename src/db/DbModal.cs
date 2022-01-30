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
	
	public string GenerateBuilder()
	{
		string code = "";
		code = "var mb = new ModalBuilder();\n" +
			$"    .WithTitle(\"{Title}\")\n" +
			$"    .WithCustomId(\"{CustomId}\")\n";
		ActionRows.ForEach(x => x.Components.ForEach(x => code += x.GenerateBuilder()));
		return code[..^1] + ';';
	}
}
