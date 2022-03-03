using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace realmikefacts_common.Model.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RealMikeFactsAuthorizationRequest
	{
        [JsonProperty(PropertyName = "AuthorizationToken")]
        public string AuthorizationToken { get; set; }
	}
}