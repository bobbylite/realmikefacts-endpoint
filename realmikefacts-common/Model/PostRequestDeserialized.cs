using Newtonsoft.Json;

namespace realmikefacts_common.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PostRequestDeserialized
    {
        [JsonProperty(PropertyName = "SetCookie")]
        public bool SetCookie { get; set; }

        [JsonProperty(PropertyName = "JwtToken")]
        public string JwtToken { get; set; }
    }
}
