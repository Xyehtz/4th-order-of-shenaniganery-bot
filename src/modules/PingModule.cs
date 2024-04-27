using Discord.Commands;

public class PingModule : ModuleBase {
    [Command("ping")]
    public async Task PingAsync() {
        await ReplyAsync("Pong!");
    }
}
