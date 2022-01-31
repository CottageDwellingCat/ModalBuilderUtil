namespace ModalBuilderUtil;

public class DbModalTypeConverter : TypeConverter<DbModal>
{
	public override ApplicationCommandOptionType GetDiscordType()
		=> ApplicationCommandOptionType.String;

	public override async Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
	{
		var db = (OddlyFluffyDbContext)services.GetService(typeof(OddlyFluffyDbContext));

		int value = int.Parse((string)option.Value);
		
		return TypeConverterResult.FromSuccess(await db.Modals
			.Include(modal => modal.ActionRows)
			.ThenInclude(row => row.Components)
			.FirstAsync(x => x.DbModalId == value));
	}
}

public class DbComponentTypeConverter : TypeConverter<DbComponent>
{
	public override ApplicationCommandOptionType GetDiscordType()
		=> ApplicationCommandOptionType.String;

	public override async Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
	{
		var db = (OddlyFluffyDbContext)services.GetService(typeof(OddlyFluffyDbContext));

		int value = int.Parse((string)option.Value);

		return TypeConverterResult.FromSuccess(await db.Components
			.FirstAsync(x => x.DbComponentId == value));
	}
}