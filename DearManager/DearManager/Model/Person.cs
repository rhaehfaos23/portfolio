using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DearManager.Model
{
    public class Person
    {
        [JsonProperty("id")]public uint Id { get; set; }
        [JsonProperty("name")]public string Name { get; set; }
        [JsonProperty("nickname")]public string NickName { get; set; }
        [JsonProperty("profile_image")]public string ImagePath { get; set; }
        public ImageSource ProfileImage { get; set; }

        public override bool Equals(object obj)
        {
            return Id == (obj as Person).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class PersonEqualityComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Person obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
