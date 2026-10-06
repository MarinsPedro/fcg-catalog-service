using FiapCloudGames.BuildingBlocks.Web;
using FiapCloudGames.Catalog.Application.Features.Games.Activate;
using FiapCloudGames.Catalog.Application.Features.Games.Create;
using FiapCloudGames.Catalog.Application.Features.Games.Deactivate;
using FiapCloudGames.Catalog.Application.Features.Games.Get;
using FiapCloudGames.Catalog.Application.Features.Games.List;
using FiapCloudGames.Catalog.Application.Features.Games.Update;
using FiapCloudGames.Catalog.Presentation.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Catalog.Presentation.Features.Games;

[ApiController]
[Route("v1/games")]
public sealed class GamesController : ControllerBase
{
    private const string GetByIdRoute = "Catalog.Games.GetByIdAsync";

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<GameResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<GameResponse>>> ListAsync(
        [FromServices] ListGameService service,
        CancellationToken cancellation)
    {
        var results = await service.ExecuteAsync(onlyActive: true, cancellation);
        return Ok(results);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = GetByIdRoute)]
    [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GameResponse>> GetByIdAsync(
        [FromRoute] Guid id,
        [FromServices] GetGameService service,
        CancellationToken cancellationToken)
    {
        var result = await service.ExecuteAsync(id, cancellationToken);
        return Ok(result.ToResponse());
    }

    // TODO: verificar o building blocks.
    // [Authorize(Policy = CatalogPolicies.GamesCreate)]
    [HttpPost]
    [ProducesResponseType<GameResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GameResponse>> CreateAsync(
        [FromBody] CreateGameRequest request,
        [FromServices] CreateGameService service,
        CancellationToken cancellation)
    {
        var result = await service.ExecuteAsync(request.ToInput(), cancellation);
        var response = result.ToResponse();
        return CreatedAtRoute(GetByIdRoute, new { id = response.Id }, response);
    }

    // [Authorize(Roles = "Administrator")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GameResponse>> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateGameRequest request,
        [FromServices] UpdateGameService service,
        CancellationToken cancellationToken)
    {
        var result = await service.ExecuteAsync(id, request.ToInput(), cancellationToken);
        return Ok(result.ToResponse());
    }

    // [Authorize(Roles = "Administrator")]
    [HttpPost("activate/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActivateAsync(
        [FromRoute] Guid id,
        [FromServices] ActivateGameService service,
        CancellationToken cancellationToken)
    {
        await service.ExecuteAsync(id, cancellationToken);
        return NoContent();
    }

    // [Authorize(Roles = "Administrator")]
    [HttpDelete("deactivate/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeactivateAsync(
        [FromRoute] Guid id,
        [FromServices] DeactivateGameService service,
        CancellationToken cancellationToken)
    {
        await service.ExecuteAsync(id, cancellationToken);
        return NoContent();
    }
}
