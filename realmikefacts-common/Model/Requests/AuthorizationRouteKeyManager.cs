using System;
using System.Collections.Generic;
using System.Text;

namespace realmikefacts_common.Model.Requests
{
	public static class AuthorizationRouteKeyManager
	{
		public const string Put = "PUT /authorization";
		public const string Post = "POST /authorization";
		public const string Get = "GET /authorization";
		public const string Options = "OPTIONS /authorization";

		/// <summary>
		/// Creates route key from HTTP method type string and the api path string
		/// </summary>
		/// <param name="method">HTTP method type</param>
		/// <param name="path">API Endpoint path</param>
		/// <returns>Route key string</returns>
		public static string CreateRouteKey(string method, string path)
		{
			return $"{method} {path}";
		}
	}
}
