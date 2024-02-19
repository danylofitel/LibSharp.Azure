// Copyright (c) LibSharp. All rights reserved.

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LibSharp.Common;
using Microsoft.Azure.Cosmos;

namespace LibSharp.Azure.Cosmos
{
    /// <summary>
    /// Extension methods for Container.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Creates an item if it does not exist, or updates if it does.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="container">Container.</param>
        /// <param name="id">Item ID.</param>
        /// <param name="partitionKey">Item partition key.</param>
        /// <param name="createItem">Function that creates an item if it doesn't exist.</param>
        /// <param name="updateItem">Function that updates an existing item.</param>
        /// <param name="requestOptions">Request options.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Item response.</returns>
        public static async Task<ItemResponse<T>> CreateOrUpdateItemAsync<T>(
            this Container container,
            string id,
            PartitionKey partitionKey,
            Func<T> createItem,
            Func<T, T> updateItem,
            ItemRequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            Argument.NotNull(container, nameof(container));
            Argument.NotNullOrEmpty(id, nameof(id));
            Argument.NotNull(partitionKey, nameof(partitionKey));
            Argument.NotNull(createItem, nameof(createItem));
            Argument.NotNull(updateItem, nameof(updateItem));

            ItemResponse<T> existingItem;

            try
            {
                existingItem = await container.ReadItemAsync<T>(id, partitionKey, requestOptions, cancellationToken).ConfigureAwait(false);
            }
            catch (CosmosException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
            {
                existingItem = null;
            }

            if (existingItem == null)
            {
                T newItem = createItem();
                return await container.CreateItemAsync(newItem, partitionKey, requestOptions, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                T updatedItem = updateItem(existingItem.Resource);

                ItemRequestOptions updateOptions = requestOptions ?? new ItemRequestOptions();
                updateOptions.IfMatchEtag = existingItem.ETag;

                return await container.ReplaceItemAsync(updatedItem, id, partitionKey, updateOptions, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
