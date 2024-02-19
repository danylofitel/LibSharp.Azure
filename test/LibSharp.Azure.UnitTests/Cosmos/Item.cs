// Copyright (c) LibSharp. All rights reserved.

using Newtonsoft.Json;

namespace LibSharp.Azure.UnitTests.Cosmos
{
    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partition")]
        public string Partition { get; set; }

        public int Version { get; set; }
    }
}
