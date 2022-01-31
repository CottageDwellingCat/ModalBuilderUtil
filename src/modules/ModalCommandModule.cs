using Microsoft.EntityFrameworkCore;

namespace ModalBuilderUtil;

[Group("modal", "commands for managing user created modals.")]
public class ModalCommands : InteractionModuleBase<SocketInteractionContext>
{
	public OddlyFluffyDbContext Db { get; set; }

	[SlashCommand("code", "Get the code required to use a modal in your own bot.")]
	public async Task Code
	(
		[Autocomplete(typeof(DbModalAutocompleteProvider))]
		[Summary("modal", "The modal to generate.")]
		DbModal modal
	)
		=> await RespondAsync($"```cs\n{modal.GenerateCode()}```");

	[SlashCommand("create", "Create a new modal,")]
	public async Task Create() => await RespondWithModalAsync<ModalInfoModal>("modal.create");
	[SlashCommand("update", "Updates an already existant modal you created.")]
	public async Task Update
	(
		[Autocomplete(typeof(DbModalAutocompleteProvider))]
		[AutocompleteFromUser]
		[Summary("modal", "The modal to modify.")]
		DbModal modal
	)
	{
		if (modal.UserID != Context.User.Id)
		{
			await RespondAsync("You do not have permission to edit this modal.", ephemeral: true);
			return;
		}

		var mb = new ModalBuilder()
			.AddTextInput("Title", "title", TextInputStyle.Short, "Insert a title.", 4, 100, true, modal.Title)
			.AddTextInput("Custom Id", "customID", TextInputStyle.Short, "Insert a custom Id.",
				1, 100, true, modal.CustomId);
		await RespondWithModalAsync(mb.Build());
	}

	[SlashCommand("test", "Tests a modal.")]
	public async Task Test
	(
		[Autocomplete(typeof(DbModalAutocompleteProvider))]
		[Summary("modal", "The modal to test.")]
		DbModal modal
	)
	{
		if (!modal.ActionRows.SelectMany(x => x.Components).Any())
		{
			await RespondAsync("Your modal does not have any components, add some then try again.", ephemeral: true);
			return;
		}

		await RespondWithModalAsync(modal.GetBuilder().WithCustomId($"usermodal.{modal.DbModalId}").Build());
	}
	[Group("input", "Commands for managing text input in modals.")]
	public class Input : InteractionModuleBase<SocketInteractionContext>
	{
		public OddlyFluffyDbContext Db { get; set; }
		
		[SlashCommand("add", "Adds an input to a modal.")]
		public async Task Add
		(
			[Autocomplete(typeof(DbModalAutocompleteProvider))]
			[AutocompleteFromUser]
			[Summary("modal", "The modal to modify.")]
			DbModal modal,
			[Summary(null, "The style of the text input.")]
			TextInputStyle style,
			[Summary("min-length", "The shortest allowed length of inputted text.")]
			int minLength = 1,
			[Summary("max-length", "The longest allowed length of inputted text.")]
			int maxLength = 4000,
			[Summary("required", "Is the user required to submit a modal?")]
			bool required = true
		)
		{
			if (modal.UserID != Context.User.Id)
			{
				await RespondAsync("You do not have permission to edit this modal.", ephemeral: true);
				return;
			}
			await RespondWithModalAsync<TextInputInfoModal>($"modal.{modal.DbModalId}.components.add${(int)style}:" +
				$"{minLength}:{maxLength}:{required}");
		}
		
		[SlashCommand("remove", "Remove a specified text input.")]
		public async Task Remove
		(
			[Autocomplete(typeof(DbModalAutocompleteProvider))]
			[AutocompleteFromUser]
			[Summary("modal", "The parent modal of the component.")]
			DbModal modal,
			[Autocomplete(typeof(DbModalComponentAutocompleteProvider))]
			[Summary("component", "The component to modify.")]
			DbComponent component
		) 
		{
			if (modal.UserID != Context.User.Id)
			{
				await RespondAsync("You do not have permission to edit this modal.", ephemeral: true);
				return;
			}

			modal.ActionRows.First(x => x.Components.Contains(component)).Components.Remove(component);
			Db.Modals.Update(modal);
			await Db.SaveChangesAsync();
			await RespondAsync("Removed the component.");
		}
		
		[SlashCommand("update", "Update a specified text input.")]
		public async Task Update
		(
			[Autocomplete(typeof(DbModalAutocompleteProvider))]
			[AutocompleteFromUser]
			[Summary("modal", "The parent modal of the component.")]
			DbModal modal,
			[Autocomplete(typeof(DbModalComponentAutocompleteProvider))]
			[Summary("component", "The component to modify.")]
			DbComponent component,
			[Summary("min-length", "The shortest allowed length of inputted text.")]
			int? minLength = null,
			[Summary("max-length", "The longest allowed length of inputted text.")]
			int? maxLength = null,
			[Summary("required", "Is the user required to submit a modal?")]
			bool? required = null
		)
		{
			if (modal.UserID != Context.User.Id)
			{
				await RespondAsync("You do not have permission to edit this modal.", ephemeral: true);
				return;
			}

			component.Min = minLength ?? component.Min;
			component.Max = maxLength ?? component.Max;
			component.Required = required ?? component.Required;

			var mb = new ModalBuilder()
				.WithCustomId($"modal.{modal.DbModalId}.component.{component.DbComponentId}.update")
				.WithTitle("Update Component")
				.AddTextInput("Title", "title", TextInputStyle.Short, "Insert a title.", 1, 100, value: component.Label)
				.AddTextInput("Custom Id", "customId", TextInputStyle.Short, "Insert a custom id.", 1, 100,
					value: component.CustomId)
				.AddTextInput("Placeholder", "placeholder", TextInputStyle.Paragraph,
					"Insert a placeholder (Text shown when the input is empty).", 0, 400, value: component.Placeholder)
				.AddTextInput("Value", "value", TextInputStyle.Paragraph,
					"Insert a value (The default text in the input).", 0, 400, value: component.Placeholder);

			await RespondWithModalAsync(mb.Build());
		}
	}

	[ModalInteraction("modal.create", true)]
	public async Task Create(ModalInfoModal modal)
	{
		DbModal dbModal = new()
		{
			CustomId = modal.ModalCustomId,
			Title = modal.ModalTitle,
			UserID = Context.User.Id
		};

		await Db.Modals.AddAsync(dbModal);
		await Db.SaveChangesAsync();
		await RespondAsync("Created your modal!", ephemeral:true);
	}
	
	[ModalInteraction("modal.*.components.add$*:*:*:*", true)]
	public async Task AddComponent(string modalID, string style, string minLength = "0", string maxLength = "4000", 
		string Required = "true", TextInputInfoModal modalInput = null)
	{
		DbModal modal = Db.Modals
			.Include(x => x.ActionRows)
			.ThenInclude(x => x.Components)
			.First(x => x.DbModalId == int.Parse(modalID));
			
		if (modal.UserID != Context.User.Id)
		{
			await RespondAsync("You do not have permission to edit this modal.", ephemeral: true);
			return;
		}

		var tib = new TextInputBuilder()
			.WithLabel(modalInput.Title)
			.WithCustomId(modalInput.InputCustomId)
			.WithPlaceholder(modalInput.InputPlaceholder)
			.WithValue(modalInput.InputValue)
			.WithStyle((TextInputStyle)int.Parse(style))
			.WithMinLength(int.Parse(minLength))
			.WithMaxLength(int.Parse(maxLength))
			.WithRequired(bool.Parse(Required));

		try
		{
			modal.GetBuilder().Build();
		}
		catch (Exception ex)
		{
			var reason = $"Adding the component failed \n>>> ```diff\n-  {ex.ToString().Replace("\n", "\n-  ")}\n```";
			await RespondAsync(reason, ephemeral:true);
			return;
		}
		modal.ActionRows.Add(new DbActionRow() { Components = new() { new DbComponent().FromTextInput(tib.Build()) } });

		Db.Modals.Update(modal);

		await Db.SaveChangesAsync();

		await RespondAsync("Added the component", ephemeral: true);
	}
	
	[ModalInteraction("modal.*.component.*.update", true)]
	public async Task UpdateComponent(string modalID, string componentID, TextInputInfoModal input)
	{
		DbModal modal = Db.Modals
			.Include(x => x.ActionRows)
			.ThenInclude(x => x.Components)
			.First(x => x.DbModalId == int.Parse(modalID));

		DbComponent component = modal.ActionRows
			.SelectMany(x => x.Components)
			.First(x => x.DbComponentId == int.Parse(componentID));

		component.Value = input.InputValue;
		component.Label = input.InputTitle;
		component.CustomId = input.InputCustomId;
		component.Placeholder = input.InputPlaceholder;

		Db.Modals.Update(modal);
		await Db.SaveChangesAsync();

		await RespondAsync("Updated the component.", ephemeral:true);
	}
}