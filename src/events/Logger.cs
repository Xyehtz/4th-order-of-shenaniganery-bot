using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class Logger {
    public Logger(DiscordSocketClient client, CommandService commands) {
        client.Log += Log;
        commands.Log += Log;
    }

    public static async Task Log(LogMessage message) {
        if (message.Exception is CommandException commandException) {
            Console.WriteLine($"[Command/{message.Severity}] {commandException.Command.Aliases.First()} failed to execute in {commandException.Context.Channel}");
            Console.WriteLine(commandException);

            using (StreamWriter outputFile = File.AppendText("logs/start_log.txt")) {
                await outputFile.WriteAsync($"[Command/{message.Severity}] {commandException.Command.Aliases.First()} failed to execute in {commandException.Context.Channel}\n{commandException}\n");
            }
        } else {
            Console.WriteLine($"[General/{message.Severity}] {message}");
            
            using (StreamWriter outputFile = File.AppendText("logs/start_log.txt")) {
                await outputFile.WriteAsync($"[General/{message.Severity}] {message}\n");
            }
        }
    }
}
