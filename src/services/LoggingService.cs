namespace ModalBuilderUtil;

public class LoggingService
{
	public LogSeverity Severity { get; set; }
	public Func<LogMessage, string> GetFormattedMessage { get; set; }

	public LoggingService(LogSeverity severity = LogSeverity.Info, Func<LogMessage, string> messageFormatter = null)
	{
		Severity = severity;
		GetFormattedMessage = messageFormatter ?? new(x => x.ToString());
	}

	public void Log(LogMessage message) 
		=> Console.WriteLine(GetFormattedMessage(message));

	public void Log(string source, string message, LogSeverity severity = LogSeverity.Info)
		=> Log(new(severity, source, message));
		
	public void Log(string source, string message, LogSeverity severity = LogSeverity.Error, Exception exception = null)
		=> Log(new(severity, source, message, exception));
}