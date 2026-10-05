using FiapCloudGames.Catalog.Application.Features.Games.Create;
using FiapCloudGames.Catalog.Presentation.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Catalog.Presentation.Features.Games;

[ApiController]
[Route("internal/v1/games")]
public sealed class GamesController : ControllerBase
{
    private const string GetByIdRoute = "Catalog.Games.GetById";

    [Authorize(Policy = CatalogPolicies.GamesCreate)]
    [HttpPost]
    [ProducesResponseType<GameResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<GameResponse>> CreateAsync(
        [FromBody] CreateGameRequest request,
        [FromServices] CreateGameService createGameService,
        CancellationToken cancellation)
    {
        var result = await createGameService.ExecuteAsync(request.ToInput(), cancellation);
        var response = result.ToResponse();
        return CreatedAtRoute(GetByIdRoute, new { id = response.Id }, response);
    }
}
