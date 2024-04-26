using Discord;

public class Logger {
    public static async Task Log(LogMessage message) {
        Console.WriteLine(message.ToString());

        using (StreamWriter outputFile = File.AppendText("logs/start_log.txt")) {
            await outputFile.WriteAsync(message.ToString() + "\n");
        }
    }
}
