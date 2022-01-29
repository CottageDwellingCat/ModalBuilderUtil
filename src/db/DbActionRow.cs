namespace ModalBuilderUtil;


public class DbActionRow
{
	public int DbActionRowId { get; set; }

	public int DBModalId { get; set; }
	public DbModal Modal { get; set; }

	public List<DbComponent> Components { get; set; } = new();
}