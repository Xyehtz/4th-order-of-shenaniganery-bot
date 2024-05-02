using Discord;
using Discord.Commands;
using Discord.WebSocket;

/// <summary>
/// This class is in charge of logging the events that happen inside of the bot, mostly errors
/// </summary>
public class Logger {
    public Logger(DiscordSocketClient client, CommandService commands) {
        client.Log += Log;
        commands.Log += Log;
    }

    /// <summary>
    /// This method will log the events that happen in the bot. Depending on if the exception is a command exception or not it will slightly change the log message. This will output the log into the console in development, when the project is deployed it will be only on the logs.log file
    /// </summary>
    /// <param name="message">
    /// The LogMessage sent by the Discord API
    /// </param>
    /// <returns>
    /// Appends a new line of information to the logs.log file
    /// </returns>
    public static async Task Log(LogMessage message) {
        if (message.Exception is CommandException commandException) {
            Console.WriteLine($"[Command/{message.Severity}] {commandException.Command.Aliases.First()} failed to execute in {commandException.Context.Channel}");
            Console.WriteLine(commandException);

            using (StreamWriter outputFile = File.AppendText("logs/logs.log")) {
                await outputFile.WriteAsync($"[Command/{message.Severity}] {commandException.Command.Aliases.First()} failed to execute in {commandException.Context.Channel}\n{commandException}\n");
            }
        } else {
            Console.WriteLine($"[General/{message.Severity}] {message}");
            
            using (StreamWriter outputFile = File.AppendText("logs/logs.log")) {
                await outputFile.WriteAsync($"[General/{message.Severity}] {message}\n");
            }
        }
    }
}
