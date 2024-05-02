using System.Text;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;

/// <summary>
/// This is the class in charge of the AskDoof command, this command sends a POST request to the locally installed llama3 using ollama. Ollama answers with a response that contains the response of the command alongside more information like the AI model
/// </summary>
public class AskDoof : ModuleBase<SocketCommandContext> {
    public string _response = "";
    private string _endpoint = "http://localhost:11434/api/generate";
    private string _prompt = File.ReadAllText("notes/prompt_version2.txt");
    private ulong _doofAiChannelId = new LoadSecrets().getDoofAiChannelId();

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
        var firstAnswer = await Context.Message.ReplyAsync("Let me think for a moment...");
        await Context.Channel.TriggerTypingAsync();

        // Create the data variable that will be send with the request
        var data = $@"{{
            ""model"": ""llama3"",
            ""prompt"": ""{_prompt} {context}""
        }}";

        // Create the HttpClient, add the request headers and make the request sending the data as the request body
        using (HttpClient client = new HttpClient()) {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            try {
                await Context.Channel.TriggerTypingAsync();

                //? This type of request takes a long time to get the response from the AI, this can be due to a series of things such as the hardware in which it is running or even the tuning that it has as default

                //? The HTTP client of C# will wait until the complete response (the whole message) is received, this means that it will wait until that moment to parse all of the lines on the response

                // TODO - Search for ways to reduce the response time, a good option could be using fine tuning to improve the response time of the AI, another option could be to find a way to parse the responses as soon as we get the response

                HttpResponseMessage responseMessage = await client.PostAsync(_endpoint, content);

                // Create a StreamReader to read through the response message, declare also a line string that will have the one line of the response at a time
                using (var stream = await responseMessage.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream)) {
                    string line;

                    // Read through each line of the responseMessage that is not null, parse it into JSON and obtain the response of the AI, and add it to the _response string
                    while ((line = await reader.ReadLineAsync()) != null) {
                        var jsonLine = JObject.Parse(line);
                        _response += jsonLine["response"]?.Value<string>();

                        // If inside the JSON the done key has a true value exit the loop, this means that we have iterated over all of the lines of the response
                        if (jsonLine["done"].Value<bool>()) {
                            break;
                        }
                    }
                }

                // Edit the placeholder answer with the new answer from the AI
                await firstAnswer.ModifyAsync(x => x.Content = _response);

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