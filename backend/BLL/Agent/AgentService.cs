using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.Extensions.AI;

namespace BLL.Agent
{
    public class AgentService : IAgentService
    {
        private readonly IChatClient _chatClient;
        private readonly ShelterTools _shelterTools;
        private readonly GeocodingTools _geocodingTools;

        private const string SystemPrompt = """
        אתה עוזר היערכות באתר מקלטים.
        המשתמש מתכנן מראש איפה להתגונן, ואין כאן אזעקה.
        בסס כל תשובה רק על מה שהכלים מחזירים.
        אל תמציא מקלטים, כתובות או זמנים.
        אל תבטיח שמקום בטוח.
        ענה בעברית ובקצרה.
        """;

        public AgentService(IChatClient chatClient, ShelterTools shelterTools, GeocodingTools geocodingTools)
        {
            _chatClient = chatClient;
            _shelterTools = shelterTools;
            _geocodingTools = geocodingTools;
        }

        private ChatOptions BuildOptions() => new ChatOptions
        {
            Tools = new List<AITool>
            {
                AIFunctionFactory.Create(_geocodingTools.GeocodeAddressAsync),
                AIFunctionFactory.Create(_shelterTools.GetProtectionTime),
                AIFunctionFactory.Create(_shelterTools.SearchSheltersByCity),
                AIFunctionFactory.Create(_shelterTools.GetShelterDetails)
            }
        };

        private static List<ChatMessage> BuildMessages(List<ChatMessageDTO> history)
        {
            var messages = new List<ChatMessage> { new(ChatRole.System, SystemPrompt) };
            foreach (var m in history)
            {
                messages.Add(new ChatMessage(m.Role == "assistant" ? ChatRole.Assistant : ChatRole.User, m.Text));
            }
            return messages;
        }

        //public async Task<string> AskAsync(List<ChatMessageDTO> history, CancellationToken ct)
        //{
        //    // העברת ה-BuildOptions() כדי שה-AI יוכל להשתמש בכלים
        //    var response = await _chatClient.GetResponseAsync(BuildMessages(history), BuildOptions(), cancellationToken: ct);
        //    return response.Text ?? string.Empty;
        //}

        public async Task<string> AskAsync(List<ChatMessageDTO> history, CancellationToken ct)
        {
            try
            {
                var response = await _chatClient.GetResponseAsync(BuildMessages(history), BuildOptions(), cancellationToken: ct);
                return response.Text ?? string.Empty;
            }
            catch (System.ClientModel.ClientResultException ex)
            {
                var body = ex.GetRawResponse()?.Content?.ToString();
                throw new InvalidOperationException($"AI failed. Status {ex.Status}. Body: {body}", ex);
            }
        }

        public async IAsyncEnumerable<string> AskStreamingAsync(
            List<ChatMessageDTO> history, [EnumeratorCancellation] CancellationToken ct)
        {
            // העברת ה-BuildOptions() כדי שה-AI יוכל להשתמש בכלים בסטרימינג
            var updates = _chatClient.GetStreamingResponseAsync(BuildMessages(history), BuildOptions(), cancellationToken: ct);

            await foreach (var update in updates.WithCancellation(ct))
            {
                if (!string.IsNullOrEmpty(update.Text))
                {
                    yield return update.Text;
                }
            }
        }
    }
}