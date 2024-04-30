using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AskDoof : ModuleBase<SocketCommandContext> {
    public string _response = "";
    private string _endpoint = "http://localhost:11434/api/generate";
    private string _prompt = File.ReadAllText("notes/prompt_version2.txt");
    private ulong _doofAiChannelId = new LoadSecrets().getDoofAiChannelId();

    [Command("askDoof")]
    public async Task AskDoofAsync(params string[] contextMessage) {
        if (Context.Channel.Id != _doofAiChannelId) {
            await ReplyAsync($"This command can only be used on <#{_doofAiChannelId}>");
            return;
        }

        string context = string.Join(" ", contextMessage);
        var firstAnswer = await Context.Message.ReplyAsync("Let me think for a moment...");
        await Context.Channel.TriggerTypingAsync();

        var data = $@"{{
            ""model"": ""llama3"",
            ""prompt"": ""{_prompt} {context}""
        }}";

        using (HttpClient client = new HttpClient()) {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            try {
                await Context.Channel.TriggerTypingAsync();

                HttpResponseMessage responseMessage = await client.PostAsync(_endpoint, content);

                using (var stream = await responseMessage.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream)) {
                    string line;

                    while ((line = await reader.ReadLineAsync()) != null) {
                        var jsonLine = JObject.Parse(line);
                        _response += jsonLine["response"]?.Value<string>();

                        if (jsonLine["done"].Value<bool>()) {
                            break;
                        }
                    }
                }

                await firstAnswer.ModifyAsync(x => x.Content = _response);

            } catch (Exception ex) {
                using (StreamWriter outputFile = File.AppendText("logs/llama3_logs.txt"))
                {
                    await outputFile.WriteAsync($"{ex}\n");
                }

                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}