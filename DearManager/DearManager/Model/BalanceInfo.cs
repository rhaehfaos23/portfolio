using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DearManager.Model
{
    public class BalanceInfo: ViewModel.BaseViewModel
    {
        [JsonProperty("person_id")]public uint Id { get; set; }
        [JsonProperty("name")]public string Name { get; set; }
        [JsonProperty("nickname")] public string NickName { get; set; }
        [JsonProperty("profile_image")]public string Image { get; set; }
        [JsonProperty("balance")]public decimal Balance { get; set; }
        public ImageSource ProfileImage { get; set; }
    }
}
