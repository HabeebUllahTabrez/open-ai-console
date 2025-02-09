using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI;

class Program
{
    private static readonly string _apiKey = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to the OpenAI Chat Console!");
        Console.WriteLine("Type 'exit' to quit the application.");
        
        var openAiService = new OpenAIService(new OpenAiOptions { ApiKey = _apiKey });
        var messages = new List<ChatMessage>
        {
            ChatMessage.FromSystem("You are a helpful AI assistant.")
        };

        while (true)
        {
            Console.Write("\nYou: ");
            string userInput = Console.ReadLine() ?? string.Empty;

            if (userInput.ToLower() == "exit")
                break;

            if (string.IsNullOrWhiteSpace(userInput))
                continue;

            try
            {
                messages.Add(ChatMessage.FromUser(userInput));
                
                var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
                {
                    Messages = messages,
                    Model = "gpt-4o-mini"
                });


                if (completionResult.Successful)
                {
                    var response = completionResult.Choices.First().Message.Content;
                    messages.Add(ChatMessage.FromAssistant(response));
                    Console.WriteLine($"\nAssistant: {response}");
                }
                else
                {
                    Console.WriteLine($"\nError: {completionResult.Error?.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        Console.WriteLine("\nThank you for using the OpenAI Chat Console!");
    }
}
