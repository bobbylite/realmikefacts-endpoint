using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace realmikefacts_common.Model.Jwt
{
	[JsonObject(MemberSerialization.OptIn)]
	public class RealMikeFactsJwtTokenPayload
	{
		[JsonProperty(PropertyName = "at_hash")]
		public string AtHash { get; set; }

		[JsonProperty(PropertyName = "sub")]
		public string Sub { get; set; }

		[JsonProperty(PropertyName = "aud")]
		public string Aud { get; set; }

		[JsonProperty(PropertyName = "email_verified")]
		public bool IsEmailVerified { get; set; }

		[JsonProperty(PropertyName = "token_use")]
		public string TokenUse { get; set; }

		[JsonProperty(PropertyName = "auth_time")]
		public DateTime AuthTime { get; set; }
	}
}
