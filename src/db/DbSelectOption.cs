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
}