namespace DemoTests.Abstract
{
    public class BaseFixture
    {
        private static readonly string IMAGE_NAME = "postgres:latest";
        private static readonly string POSTGRES_USER = Guid.NewGuid().ToString("N");
        private static readonly string POSTGRES_PASSWORD = Guid.NewGuid().ToString("N");
        private static readonly int Port = Random.Shared.Next(12000, 16000);
        protected static readonly string POSTGRES_DB = Guid.NewGuid().ToString("N");

        private TestcontainersContainer _container;
        private readonly ITestcontainersBuilder<TestcontainersContainer> _containerBuilder = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(IMAGE_NAME)
            .WithEnvironment(nameof(POSTGRES_USER), POSTGRES_USER)
            .WithEnvironment(nameof(POSTGRES_PASSWORD), POSTGRES_PASSWORD)
            .WithEnvironment(nameof(POSTGRES_DB), POSTGRES_DB)
            .WithPortBinding(Port, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432));

        protected NpgsqlConnection _connection;

        [SetUp]
        public async Task SetupAsync()
        {
            _container = _containerBuilder.Build();
            await _container.StartAsync();
            _connection = new NpgsqlConnection($"Server=127.0.0.1;Port={Port};Database={POSTGRES_DB};User Id={POSTGRES_USER};Password={POSTGRES_PASSWORD};");
            await _connection.OpenAsync();
        }

        [TearDown]
        public async Task CleanupAsync()
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
            await _container.DisposeAsync();
        }

        [OneTimeTearDown]
        public async Task FinalCleanupAsync()
        {
            await _container.CleanUpAsync();
        }
    }
}
