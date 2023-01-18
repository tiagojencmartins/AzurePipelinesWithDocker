using DemoTests.Abstract;

namespace DemoTests
{
    public class PostgresTests : BaseFixture
    {

        [Test]
        public void Assert_PingDatabase_IsOpened()
        {
            Assert.That(_connection.FullState, Is.EqualTo(ConnectionState.Open));
        }

        [Test]
        public async Task Assert_Database_Exists()
        {
            var query = $"SELECT 1 FROM pg_database WHERE datname='{POSTGRES_DB}'";

            await using var command = new NpgsqlCommand(query, _connection);
            await using var reader = await command.ExecuteReaderAsync();

            Assert.That(reader, Is.Not.Null);

            while (await reader.ReadAsync())
            {
                Assert.That(reader.GetInt32(0), Is.EqualTo(1));
            }
        }
    }
}