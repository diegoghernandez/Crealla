using CreallaTest.Fixturas;

// namespace CreallaTest.Inicializador;

[CollectionDefinition(nameof(SharedFixtureCollection))]
public class SharedFixtureCollection : IClassFixture<DirectorioFixturas> { }