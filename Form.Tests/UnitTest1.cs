using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Form.Tests
{
    public class UnitTest1
    {
        private const string EndpointUrl = "Your_Endpoint_Url";
        private const string PrimaryKey = "Your_Primary_Key";
        private const string DatabaseId = "Your_Database_Id";
        private const string ContainerId = "Your_Container_Id";
        
        [Fact]
        public async Task CreateAndDeleteDatabase_Success()
        {
            using (var client = new CosmosClient(EndpointUrl, PrimaryKey))
            {
                //Arrrange.
                
                    // Create database
                await client.CreateDatabaseIfNotExistsAsync(DatabaseId);

                //Act

                    // Check if database exists
                DatabaseResponse response = await client.GetDatabase(DatabaseId).ReadAsync();
                
                //Assert
                Assert.NotNull(response);
                Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);

                    // Delete database
                await client.GetDatabase(DatabaseId).DeleteAsync();

                    // Ensure database is deleted
                await Assert.ThrowsAsync<CosmosException>(() => client.GetDatabase(DatabaseId).ReadAsync());
            }
        }

        [Fact]
        public async Task CreateAndDeleteContainer_Success()
        {
            using (var client = new CosmosClient(EndpointUrl, PrimaryKey))
            {
                //Arrange.
                
                    // Create database
                await client.CreateDatabaseIfNotExistsAsync(DatabaseId);
                    // Get database
                Database database = client.GetDatabase(DatabaseId);

                //Act

                    // Create container
                await database.CreateContainerIfNotExistsAsync(ContainerId, "/partitionKey");

                    // Check if container exists
                ContainerResponse response = await database.GetContainer(ContainerId).ReadContainerAsync();
                
                //Assert

                Assert.NotNull(response);
                Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);

                    // Delete container
                await database.GetContainer(ContainerId).DeleteContainerAsync();
                    // Delete database
                await client.GetDatabase(DatabaseId).DeleteAsync();
                    // Ensure container is deleted
                await Assert.ThrowsAsync<CosmosException>(() => database.GetContainer(ContainerId).ReadContainerAsync());
            }
        }

        [Fact]
        public async Task ReadItemsFromContainer_Success()
        {
            try
            {

                // Initialize CosmosClient
                using ( var client = new CosmosClient(EndpointUrl, PrimaryKey))
                {
                    //Arrange
                    // Create database
                    await client.CreateDatabaseIfNotExistsAsync(DatabaseId);
                    // Get database
                    Database database = client.GetDatabase(DatabaseId);
                    // Create container
                    await database.CreateContainerIfNotExistsAsync(ContainerId, "/partitionKey");
                    // Get container
                    Container container = database.GetContainer(ContainerId);

                    //Act
                    // Query items
                    QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c");
                    FeedIterator<dynamic> feedIterator = container.GetItemQueryIterator<dynamic>(queryDefinition);

                    List<dynamic> items = new List<dynamic>();
                    while (feedIterator.HasMoreResults)
                    {
                        FeedResponse<dynamic> response = await feedIterator.ReadNextAsync();
                        items.AddRange(response);
                    }

                    //Assert
                        // Assert that items are not null
                    Assert.NotNull(items);
                        // Optionally assert that items count is as expected
                    Assert.Equal(0, items?.Count);
                        // Delete container
                    await database.GetContainer(ContainerId).DeleteContainerAsync();
                        // Delete database
                    await client.GetDatabase(DatabaseId).DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            finally
            {

            }
            
        }
    }
}