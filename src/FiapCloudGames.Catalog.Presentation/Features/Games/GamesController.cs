namespace FiapCloudGames.Catalog.Presentation.Features.Games;

[ApiController]
[Route("v1/games")]
public sealed class GamesController : ControllerBase
{
    private const string GetByIdRoute = "Catalog.Games.GetByIdAsync";

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<GameResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<GameResponse>>> ListAsync(
        [FromQuery] GameQueryRequest request,
        [FromServices] ListGameService service,
        CancellationToken cancellation)
    {
        var results = await service.ExecuteAsync(request.ToInput(), cancellation);

        // TODO: não é o correto fazer um mapeamento aqui, daria pra responder um PageResult<GameResult> e alterar a assinatura do método para Task<IActionResult>, para evitar essa transformação aqui.
        var pagedResponse = new PagedResponse<GameResponse>
        {
            Items = results.Items.Select(x => x.ToResponse()).ToList(),
            Page = results.Page,
            PageSize = results.PageSize,
            TotalItems = results.TotalItems,
            TotalPages = results.TotalPages
        };

        return Ok(pagedResponse);
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
