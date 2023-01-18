namespace DemoTests
{
    public class PrivateRegistryTests
    {
        private static readonly string IMAGE_NAME = "tjemartins/helloworld:latest";
        private static readonly string ENDPOINT = "http://localhost:9119/hello/";
        private static readonly int PORT = 9119;

        [Test]
        [TestCase("spyro")]
        public async Task Assert_HelloWorldResponse_MatchesProvidedArgument(string value)
        {
            var _container = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage(IMAGE_NAME)
                .WithPortBinding(PORT, 80)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(80))
                .Build();

            await _container.StartAsync();

            var client = new HttpClient();

            var response = await client.GetAsync($"{ENDPOINT}{value}");

            response.EnsureSuccessStatusCode();

            Assert.That(await response.Content.ReadAsStringAsync(), Is.EqualTo($"Hello {value}"));

            await _container.CleanUpAsync();
        }
    }
}
