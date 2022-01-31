namespace ModalBuilderUtil;

public class DbModalAutocompleteProvider : AutocompleteHandler
{
	public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, 
		IAutocompleteInteraction interaction, IParameterInfo parameter, IServiceProvider services)
	{
		bool fromUser = parameter.Attributes.Any(x => x is AutocompleteFromUserAttribute);
		var db = (OddlyFluffyDbContext)services.GetService(typeof(OddlyFluffyDbContext));

		var modals = (await db.Modals.Where(x => !fromUser || x.UserID == context.User.Id).ToListAsync())
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


public class DbModalComponentAutocompleteProvider : AutocompleteHandler
{
	public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context,
		IAutocompleteInteraction interaction, IParameterInfo parameter, IServiceProvider services)
	{
		bool fromUser = parameter.Attributes.Any(x => x is AutocompleteFromUserAttribute);
		var db = (OddlyFluffyDbContext)services.GetService(typeof(OddlyFluffyDbContext));

		var modalID = int.Parse((string)interaction.Data.Options.First(x => x.Name == "modal").Value);
		var modal = await db.Modals
			.Include(x => x.ActionRows)
			.ThenInclude(x => x.Components)
			.FirstAsync(x => x.DbModalId == modalID);

		var components = modal.ActionRows
			.SelectMany(x => x.Components)
			.Where(x => (x.Label + x.Placeholder + x.CustomId + x.DbComponentId)
				.Contains((string)interaction.Data.Current.Value, StringComparison.OrdinalIgnoreCase))
			.Select(x => new AutocompleteResult(x.Label ?? x.CustomId, x.DbComponentId.ToString()));

		return AutocompletionResult.FromSuccess(components);
	}
}

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class AutocompleteFromUserAttribute : Attribute { }