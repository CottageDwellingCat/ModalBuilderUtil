namespace ModalBuilderUtil;

public class ModalInfoModal : IModal
{
	public string Title => "Magnificent Modal Modification";
	
	[InputLabel("Title")]
	[ModalTextInput("title", TextInputStyle.Short, "Insert a title.", maxLength: 100)]
	public string ModalTitle { get; set; }
	
	[InputLabel("Custom Id")]
	[ModalTextInput("customId", TextInputStyle.Short, "Insert a custom id.", maxLength: 100)]
	public string ModalCustomId { get; set; }
}

public class TextInputInfoModal : IModal
{
	public string Title => "Text Input";

	[InputLabel("Title")]
	[ModalTextInput("title", TextInputStyle.Short, "Insert a title.", 1,100)]
	public string InputTitle { get; set; }

	[InputLabel("Custom Id")]
	[ModalTextInput("customId", TextInputStyle.Short, "Insert a custom id.", maxLength: 100)]
	public string InputCustomId { get; set; }

	[InputLabel("Placeholder")]
	[ModalTextInput("placeholder", TextInputStyle.Paragraph,
		"Insert a placeholder (Text shown when the input is empty).", 0, 400)]
	public string InputPlaceholder { get; set; } = null;

	[InputLabel("Value")]
	[ModalTextInput("value", TextInputStyle.Paragraph, "Insert a value (The default text in the input).", 0, 400)]
	public string InputValue { get; set; } = null;
}