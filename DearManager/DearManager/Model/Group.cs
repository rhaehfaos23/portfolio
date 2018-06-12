using Newtonsoft.Json;

namespace DearManager.Model
{
    public class Group
    {
        [JsonProperty("id")]public uint Id { get; set; }
        [JsonProperty("name")]public string Name { get; set; }
        [JsonProperty("admin")]public uint AdminId { get; set; }
    }
}
