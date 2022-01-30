namespace ModalBuilderUtil;

public class DbSelectOption
{
	public int? DbSelectOptionId { get; set; }
	public string? Label { get; set; }
	public string? Value { get; set; }
	public string? Description { get; set; }
	public string? Emote { get; set; }
	public bool? Default { get; set; }

	public DbSelectOption FromSelectOption(SelectMenuOption option)
	{
		Label = option.Label;
		Value = option.Value;
		Description = option.Description;
		Emote = option.Emote.Name;
		Description = option.Description;
		Default = option.IsDefault;

		return this;
	}
	
	public string GenerateBuilder()
	{
		string code = "        .AddOption(new SelectMenuOptionBuilder()" +
			$"            .WithLabel(\"{Label}\")\n" +
			$"            .WithValue(\"{Value}\n)\n";
			if (!string.IsNullOrWhiteSpace(Description)) code += $"            .WithDescription(\"{Description}\")\n";
			if (!string.IsNullOrWhiteSpace(Emote)) code += $"            .WithEmote(Emote.Parse(\"{Description}\"))\n";
			if (Default is not null) code += $"            .WithDefault({Default})\n";
		return code[..^1] + ")\n";
	}
}