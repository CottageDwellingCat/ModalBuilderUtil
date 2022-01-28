using Discord;
using Microsoft.EntityFrameworkCore;

namespace ModalBuilderUtil;

public class Program
{
	public static async Task Main() => await new Program().MainAsync();

	public async Task MainAsync()
	{
		await Task.Delay(-1);
	}
}