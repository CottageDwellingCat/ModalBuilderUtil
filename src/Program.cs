global using Discord;
global using Discord.WebSocket;
global using Discord.Interactions;
global using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace ModalBuilderUtil;

public class Program
{
#if DEBUG
	public const LogSeverity LogLevel = LogSeverity.Debug;
#else
	const LogSeverity DefaultLogServerity = LogSeverity.Info;
#endif
	
	public static async Task Main() => await new Program().MainAsync();

	private ServiceProvider services = new ServiceCollection()
		.AddDbContext<OddlyFluffyDbContext>(ServiceLifetime.Transient)
		.AddSingleton(JsonSerializer.Deserialize<ClientSettings>(File.ReadAllText("ClientSettings.json")))
		.AddSingleton(new LoggingService(LogLevel))
		.AddSingleton(new DiscordSocketClient(new() {LogLevel = LogLevel}))
		.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(),
			new() {LogLevel = LogLevel, UseCompiledLambda = true}))
		.BuildServiceProvider();

	public async Task MainAsync()
	{
		var client 	= services.GetRequiredService<DiscordSocketClient>();
		var commands = services.GetRequiredService<InteractionService>();
		var settings = services.GetRequiredService<ClientSettings>();
		var logger 	= services.GetRequiredService<LoggingService>();

		client.Log += async message => await Task.Run(() => logger.Log(message));
		client.InteractionCreated += async interaction
			=> await commands.ExecuteCommandAsync(new SocketInteractionContext(client, interaction), services);

		await client.LoginAsync(TokenType.Bot, settings.Token);
		await client.StartAsync();

		await Task.Delay(-1);
	}
}