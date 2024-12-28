using System.Text;
using System.Text.Json;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;

/// <summary>
/// This is the class in charge of the AskDoof command, this command sends a POST request to the locally installed llama3 using ollama. Ollama answers with a response that contains the response of the command alongside more information like the AI model
/// </summary>
public class AskDoof : ModuleBase<SocketCommandContext> {
    public string _response = "";
    private string _url = "https://api-inference.huggingface.co/models/meta-llama/Llama-3.2-3B-Instruct/v1/chat/completions";
    private string _prompt = File.ReadAllText("notes/prompt_version2.txt");
    private ulong _doofAiChannelId = new LoadSecrets().getDoofAiChannelId();
    private string _huggingFaceToken = new LoadSecrets().getHuggingFaceApi();

    /// <summary>
    /// This is the method that is called when the command !askDoof is executed on the guild, this method will do the following:
    ///     - Parse the parameter of the command into a string, said string will be added to a data variable that contains all the necessary information to make the POST request
    ///     - Using the HTTP client we declare the request headers and make the request
    ///     - Ollama will answer back with the content obtained from the AI model
    ///     - The AI is processed into a string and sent to the user
    /// </summary>
    /// <param name="contextMessage">
    /// contextMessage is a string array parameter that can be of a variable number of strings depending on the amount of words in the command message
    /// </param>
    /// <returns>
    /// Returns a message created by Llama3
    /// </returns>
    [Command("askDoof")]
    public async Task AskDoofAsync(params string[] contextMessage) {
        // The command can be only used in the doof-ai channel
        if (Context.Channel.Id != _doofAiChannelId) {
            await ReplyAsync($"This command can only be used on <#{_doofAiChannelId}>");
            return;
        }

        // Parse the parameter into a string and sent a placeholder response, trigger also the typing animation in Discord
        string context = string.Join(" ", contextMessage);
        Console.WriteLine(context);
        
        // Send a basic message to the user
        var answer = await Context.Message.ReplyAsync("Let me think for a moment...");
        await Context.Channel.TriggerTypingAsync();

        var payload = new 
        {
            model = "meta-llama/Llama-3.2-3B-Instruct",
            messages = new[] {
                new 
                { 
                    role = "user", 
                    content = _prompt + context
                }
            },
            max_tokens = 500,
            stream = false
        };

        string inputData = JsonSerializer.Serialize(payload);

        // Create the HttpClient, add the request headers and make the request sending the data as the request body
        using (HttpClient client = new HttpClient()) {
            try {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_huggingFaceToken}");

                StringContent content = new StringContent(inputData, Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = await client.PostAsync(_url, content);
                responseMessage.EnsureSuccessStatusCode();
                string responseBody = await responseMessage.Content.ReadAsStringAsync();

                var jsonLine = JObject.Parse(responseBody);
                _response += jsonLine["choices"]?[0]?["message"]?["content"]?.Value<string>();

                // Edit the placeholder answer with the new answer from the AI
                await answer.ModifyAsync(x => x.Content = _response);

            // Catch any errors and log them into the llama3_logs.log file
            } catch (Exception ex) {
                using (StreamWriter outputFile = File.AppendText("logs/llama3_logs.log"))
                {
                    await outputFile.WriteAsync($"{ex}\n");
                }

                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}