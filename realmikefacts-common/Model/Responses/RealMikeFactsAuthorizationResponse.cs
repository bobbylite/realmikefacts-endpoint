using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace realmikefacts_common.Model.Responses
{
	public class RealMikeFactsAuthorizationResponse : APIGatewayHttpApiV2ProxyResponse
	{
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }

		[JsonProperty(PropertyName = "jwt")]
		public string Jwt { get; set; }
	}
}
