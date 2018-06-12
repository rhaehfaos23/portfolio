using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DearManager.Model
{
    public class Receipt
    {
        [JsonProperty("person_id")] public uint PersonId { get; set; }
        [JsonProperty("person_name")] public string PersonName { get; set; }
        [JsonProperty("nickname")] public string PersonNickName { get; set; }
        [JsonProperty("group_id")]public uint GroupId { get; set; }
        [JsonProperty("group_name")]public string GroupName { get; set; }
        [JsonProperty("money")]public decimal Money { get; set; }
        [JsonProperty("tag")]public string Tag { get; set; }
        [JsonProperty("datetime")]public DateTime DateTime { get; set; }
    }
}
