namespace ModalBuilderUtil;

public class DbModalAutocompleteProvider : AutocompleteHandler
{
	public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, 
		IAutocompleteInteraction interaction, IParameterInfo parameter, IServiceProvider services)
	{
		var db = (OddlyFluffyDbContext)services.GetService(typeof(OddlyFluffyDbContext));

		var modals = (await db.Modals.ToListAsync())
			.Where(x => x.Title.Contains((string)interaction.Data.Current.Value, StringComparison.OrdinalIgnoreCase))
			.OrderByDescending(x => x.DbModalId)
			.Select(x => new AutocompleteResult(x.Title, x.DbModalId.ToString()))
			.ToList();

		modals = modals.Count > 20
			? modals.Take(20).ToList()
			: modals;
			
		return AutocompletionResult.FromSuccess(modals);
	}
}
