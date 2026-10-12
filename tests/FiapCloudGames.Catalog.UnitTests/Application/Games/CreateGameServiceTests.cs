namespace FiapCloudGames.Catalog.UnitTests.Application.Games;

public class CreateGameServiceTests
{
    public static TheoryData<CreateGameInput> GetFakeGames => new TheoryData<CreateGameInput>
    {
        new CreateGameInput("Existing Game 1", "Description 1", "Category 1", 0.0m),
        new CreateGameInput("Existing Game 2", "Description 2", "Category 2", 9999999999.99m)
    };

    [Fact(DisplayName = "Criar jogo - deve criar e persistir o jogo com dados válidos")]
    public async Task ExecuteAsync_WithValidInput_CreatesAndPersistsGame()
    {
        // Arrange
        var fakeRepository = new FakeGameRepository();
        var unitOfWork = new SpyCatalogUnitOfWork();
        var service = new CreateGameService(
            fakeRepository,
            unitOfWork,
            new FixedTimeProvider(CatalogTestData.NowUtc),
            NullLogger<CreateGameService>.Instance);

        // Act
        var result = await service.ExecuteAsync(
            new CreateGameInput(
                "Test title Game",
                "Test game description",
                "Test category game",
                0.0m),
            cancellationToken: CancellationToken.None);

        // Assert
        Assert.NotNull(fakeRepository.AddedGame);
        Assert.Equal("Test title Game", fakeRepository.AddedGame.Title);
        Assert.Equal("Test game description", fakeRepository.AddedGame.Description);
        Assert.Equal("Test category game", fakeRepository.AddedGame.Category);
        Assert.Equal(0.0m, fakeRepository.AddedGame.BasePrice.Amount);
        Assert.Equal(CatalogTestData.NowUtc, fakeRepository.AddedGame.CreatedAtUtc);
        Assert.Equal(1, unitOfWork.SaveChangesCount);
        Assert.Equal(fakeRepository.AddedGame.Id, result.Id);
    }

    [Fact(DisplayName = "Criar jogo - deve falhar se o preço base for negativo")]
    public async Task ExecuteAsync_WithNegativeBasePrice_ReturnsDomainRuleViolationException()
    {
        // Arrange
        var fakeRepository = new FakeGameRepository();
        var unitOfWork = new SpyCatalogUnitOfWork();
        var service = new CreateGameService(
            fakeRepository,
            unitOfWork,
            new FixedTimeProvider(CatalogTestData.NowUtc),
            NullLogger<CreateGameService>.Instance);

        // Act & Assert
        await Assert.ThrowsAsync<DomainRuleViolationException>(() =>
            service.ExecuteAsync(
                new CreateGameInput(
                    "Test title Game",
                    "Test game description",
                    "Test category game",
                    -20.0m),
                cancellationToken: CancellationToken.None));
    }

    [Theory(DisplayName = "Criar jogo - deve criar e persistir o jogo com preço base dentro do intervalo válido possível")]
    [MemberData(nameof(GetFakeGames))]
    public async Task ExecuteAsync_WithMinMaxBasePrice_CreatesAndPersistsGame(CreateGameInput createGameInput)
    {
        // Arrange
        var fakeRepository = new FakeGameRepository();
        var unitOfWork = new SpyCatalogUnitOfWork();
        var service = new CreateGameService(
            fakeRepository,
            unitOfWork,
            new FixedTimeProvider(CatalogTestData.NowUtc),
            NullLogger<CreateGameService>.Instance);

        // Act
        var result = await service.ExecuteAsync(createGameInput, cancellationToken: CancellationToken.None);

        // Assert
        Assert.NotNull(fakeRepository.AddedGame);
        Assert.Equal(createGameInput.Title, fakeRepository.AddedGame.Title);
        Assert.Equal(createGameInput.Description, fakeRepository.AddedGame.Description);
        Assert.Equal(createGameInput.Category, fakeRepository.AddedGame.Category);
        Assert.Equal(createGameInput.Category, fakeRepository.AddedGame.Category);
        Assert.Equal(createGameInput.BasePrice, fakeRepository.AddedGame.BasePrice.Amount);
        Assert.Equal(CatalogTestData.NowUtc, fakeRepository.AddedGame.CreatedAtUtc);
        Assert.Equal(1, unitOfWork.SaveChangesCount);
        Assert.Equal(fakeRepository.AddedGame.Id, result.Id);
    }
}
