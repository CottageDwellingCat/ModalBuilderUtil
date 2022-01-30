namespace ModalBuilderUtil;

[Group("modal", "commands for managing user created modals.")]
public class ModalCommands : InteractionModuleBase<SocketInteractionContext>
{
	[SlashCommand("code", "Get the code required to use a modal in your own bot.")]
	public async Task Code
	(
		[Autocomplete(typeof(DbModalAutocompleteProvider))]
		[Summary("modal", "The modal to generate.")]
		DbModal modal
	)
	{
		await RespondAsync($"```cs\n{modal.GenerateBuilder()}```");
	}
}