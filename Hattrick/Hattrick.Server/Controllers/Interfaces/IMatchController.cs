using Hattrick.Server.Requests;
using Hattrick.Server.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Hattrick.Server.Controllers.Interfaces
{
    public interface IMatchController
    {
        IEnumerable<MatchResponse> Index();
        IActionResult Post([FromBody] MatchRequest matchRequest);
    }
}
