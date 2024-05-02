using Discord.WebSocket;

/// <summary>
/// Interface with the public methods used in the Status class
/// </summary>
public interface IStatus {
    Task SetGameAsync();
}

/// <summary>
/// This class manages the status of the bot, this means that it manages the change of the Playing status of the bot in Discord
/// </summary>
public class Status: IStatus {
    private DiscordSocketClient _client;
    private string[] _games = {
        "Infiltrate and Dominate the World... or at Least This Server",
        "Plotting My Next Move in the Scheme-ulator",
        "Calculating the Perfect Plan to Steal All the Cheese in the Land",
        "Inventorially Challenged (But Still Working on It)",
        "Scheming to Outsmart Perry the Platypus (Again)",
        "Fiddling with My Latest -inator Prototype",
        "Concocting a Plan to Take Over Danville (Just Kidding... Or Am I?)",
        "Researching Ways to Improve My Disguises (Still Need Work)",
        "Currently Reorganizing My Doofenshmirtz Industries' Filing System",
        "Plotting to Steal the World's Most Valuable Invention (Again)"
    };

    public Status(DiscordSocketClient client) {
        _client = client;
    }

    /// <summary>
    /// This class contains the timer that will call the changeStatus method every 10 minutes, when the bot starts it will automatically start the changeStatus method instead of waiting 10 minutes
    /// </summary>
    /// <returns>
    /// A change in the status
    /// </returns>
    public async Task SetGameAsync() {
        // Creation of the timer and setting the interval, auto reset and what to do after 10 minutes
        var timer = new System.Timers.Timer();
        timer.Interval = TimeSpan.FromMinutes(10).TotalMilliseconds;
        timer.AutoReset = true;
        timer.Elapsed += async (sender, e) => {
            await changeStatus();
        };
        timer.Start();

        await changeStatus();
    }

    /// <summary>
    /// This method just tries to change the status of the bot, if it fails it will log the error on logs.log
    /// </summary>
    /// <returns>
    /// A new status in the bot
    /// </returns>
    private async Task changeStatus() {
        try {
            await _client.SetGameAsync(_games[new Random().Next(0, _games.Length - 1)]);
        } catch (Exception e) {
            using (StreamWriter sw = File.AppendText("logs/logs.log")) {
                await sw.WriteAsync($"[{e.Source}] {e.Message}\n");
            }
        }
    }
}
