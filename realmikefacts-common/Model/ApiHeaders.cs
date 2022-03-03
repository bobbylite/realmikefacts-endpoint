using Newtonsoft.Json;

namespace realmikefacts_common.Model
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ApiHeaders
	{
		public ApiHeaders(string contentType = null)
		{
			ContentType = contentType;
		}

		[JsonProperty(PropertyName = "Content-Type")]
		public string ContentType { get; set; }
	}
}
