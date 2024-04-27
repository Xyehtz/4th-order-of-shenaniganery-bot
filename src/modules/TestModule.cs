using Discord.Commands;

public class TestModule : ModuleBase
{
    [Command("test")]
    public async Task TestAsync() {
        await ReplyAsync("This is a test of the command handler of the bot, currently I would say that the bot is around a 40% completed");
    }
}
