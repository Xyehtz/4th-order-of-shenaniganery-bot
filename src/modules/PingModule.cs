using Discord.Commands;

/// <summary>
/// Simple ping command that will reply with pong
/// </summary>
public class PingModule : ModuleBase {
    [Command("ping")]
    public async Task PingAsync() {
        await ReplyAsync("Pong!");
    }
}
