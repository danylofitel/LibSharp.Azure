// Copyright (c) LibSharp. All rights reserved.

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LibSharp.Azure.Cosmos;
using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace LibSharp.Azure.UnitTests.Cosmos
{
    [TestClass]
    public class ContainerExtensionsUnitTests
    {
        [TestMethod]
        public async Task CreateOrUpdateItemAsync_ItemDoesNotExist_Creates()
        {
            // Arrange
            Item item = new Item
            {
                Id = "itemId",
                Partition = "itemPartition",
                Version = 0,
            };

            Container container = Substitute.For<Container>();

            _ = container
                .ReadItemAsync<Item>(item.Id, Arg.Any<PartitionKey>(), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>())
                .Returns<Task<ItemResponse<Item>>>(_ => throw new CosmosException("Document not found.", HttpStatusCode.NotFound, 0, string.Empty, 1.0));

            _ = container
                .CreateItemAsync(item, new PartitionKey(item.Partition), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<ItemResponse<Item>>(null));

            // Act
            ItemResponse<Item> response = await container.CreateOrUpdateItemAsync(
                item.Id,
                new PartitionKey(item.Partition),
                () => item,
                _ => _).ConfigureAwait(false);

            // Assert
            Assert.IsNull(response);

            _ = container.Received(1).ReadItemAsync<Item>(Arg.Any<string>(), Arg.Any<PartitionKey>(), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>());
            _ = container.Received(1).CreateItemAsync(Arg.Any<Item>(), Arg.Any<PartitionKey>(), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>());
            _ = container.DidNotReceive().ReplaceItemAsync(Arg.Any<Item>(), Arg.Any<string>(), Arg.Any<PartitionKey>(), Arg.Any<ItemRequestOptions>(), Arg.Any<CancellationToken>());
        }
    }
}
