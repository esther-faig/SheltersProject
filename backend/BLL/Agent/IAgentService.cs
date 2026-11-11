using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Agent
{
    public interface IAgentService
    {
        Task<string> AskAsync(
        List<ChatMessageDTO> history,
        CancellationToken ct
        );

        IAsyncEnumerable<string> AskStreamingAsync(
            List<ChatMessageDTO> history,
            CancellationToken ct
        );
    }
}
