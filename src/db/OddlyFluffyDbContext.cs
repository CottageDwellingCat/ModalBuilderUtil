using Microsoft.EntityFrameworkCore;

namespace ModalBuilderUtil;

/// <summary>
/// 	Is it moldy? Did it come like this? We will never know.
///		<br />
/// 	EEEP that spots green - thats not normal, right??
/// 	<br />
/// 	It smells bad too, ICH.
/// </summary>
/// <remarks>
/// 	I'm starting to think it didn't come fluffy...
/// </remarks>
public class OddlyFluffyDbContext : DbContext
{
	/// <summary>
	/// 	Just a little fluffy - probably fine if you eat around it.
	/// </summary>
	public DbSet<DBModal> Modals { get; set; }
	// Feeling less jokey
	public DbSet<DbActionRow> ActionRows { get; set; }
	public DbSet<DbComponent> Components { get; set; }
	public DbSet<DbSelectOption> SelectOptions { get; set; }

	/// <summary>
	/// 	EWEWEWEWEW this is the smelly part!!!11!
	/// </summary>
	public string DbPath { get; } = "SuspiciouslySmelly.db";

	/// <summary>
	/// 	OH GODDESS IS THAT A COCKROACH??/??11!!!
	/// </summary>
	protected override void OnConfiguring(DbContextOptionsBuilder options) 
		=> options.UseSqlite($"Data Source={DbPath}");
}