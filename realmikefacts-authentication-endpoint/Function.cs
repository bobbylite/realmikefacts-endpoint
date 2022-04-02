using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using realmikefacts_common.Model;
using realmikefacts_common.Model.Requests;
using realmikefacts_common.Model.Responses;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace realmikefacts_authentication_endpoint
{
    public class Function
    {

        public static System.IdentityModel.Tokens.Jwt.Deserializer JwtDeserializer { get; set; }
        public static System.IdentityModel.Tokens.Jwt.Serializer JwtSerializer { get; set; }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
        {
            try
            {
                switch (request?.RouteKey)
                {
                    case AuthorizationRouteKeyManager.Options:
                        var optionsResponse = new APIGatewayHttpApiV2ProxyResponse()
                        {
                            StatusCode = 200
                        };

                        return optionsResponse;

                    case AuthorizationRouteKeyManager.Post:                           
                        if (request?.Cookies != null)
                        {
                            foreach (var cookieObject in request?.Cookies)
                            {
                                if (!cookieObject.Contains("realmikefacts_session"))
                                    continue;

                                var jwt = cookieObject.Split('\u003d')[1];
                                var cookieJwtHandler = new JwtSecurityTokenHandler();
                                var cookieJwtSecurityToken = cookieJwtHandler.ReadJwtToken(jwt);
                                string[] cookieArray = { cookieObject };

                                if (cookieJwtSecurityToken.ValidTo > DateTime.Now)
                                    return new RealMikeFactsAuthorizationResponse
                                    {
                                        StatusCode = ApiStatusCode.OK,
                                        Jwt = jwt,
                                        Cookies = cookieArray,
                                        Body = JsonConvert.SerializeObject("Success")
                                    };
                            }
                        }

                        return CreateCookieFromRequestData(request);

                    case AuthorizationRouteKeyManager.Get:
                        if (request?.Cookies != null)
                        {
                            foreach (var cookieObject in request.Cookies)
                            {
                                if (!cookieObject.Contains("realmikefacts_session"))
                                    continue;

                                var jwt = cookieObject.Split('\u003d')[1];
                                var cookieJwtHandler = new JwtSecurityTokenHandler();
                                var cookieJwtSecurityToken = cookieJwtHandler.ReadJwtToken(jwt);

                                if (cookieJwtSecurityToken.ValidTo < DateTime.Now)
                                    return new RealMikeFactsAuthorizationResponse
                                    {
                                        StatusCode = ApiStatusCode.UNAUTHORIZED,
                                    };

                                return new RealMikeFactsAuthorizationResponse
                                {
                                    StatusCode = ApiStatusCode.OK
                                };
                            }
                        }

                        return new RealMikeFactsAuthorizationResponse
                        {
                            StatusCode = ApiStatusCode.UNAUTHORIZED,
                        };

                    default:
                        return new RealMikeFactsAuthorizationResponse
                        {
                            StatusCode = ApiStatusCode.BAD,
                            Body = string.Empty
                        };
                }
            }
            catch (Exception e)
            {
                LambdaLogger.Log($"EXCEPTION: {e.Message}");
                return new RealMikeFactsAuthorizationResponse
                {
                    StatusCode = ApiStatusCode.SERVER_ERROR,
                    Body = e.Message
                };
            }
        }

        private static RealMikeFactsAuthorizationResponse CreateCookieFromRequestData(APIGatewayHttpApiV2ProxyRequest request)
        {
            var deserializedRequest = JsonConvert.DeserializeObject<RealMikeFactsAuthorizationRequest>(request?.Body);

            if (string.IsNullOrEmpty(deserializedRequest?.AuthorizationToken))
                return new RealMikeFactsAuthorizationResponse
                {
                    StatusCode = ApiStatusCode.UNAUTHORIZED,
                    Body = JsonConvert.SerializeObject("Unauthorized attempt")
                };

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtHandler.ReadJwtToken(deserializedRequest.AuthorizationToken);

            return CreateCookieFromJwt(deserializedRequest.AuthorizationToken, jwtSecurityToken.ValidTo, jwtSecurityToken.ValidTo < DateTime.Now);
        }

        private static RealMikeFactsAuthorizationResponse CreateCookieFromJwt(string jwt, DateTime expires, bool expired)
        {
            if (string.IsNullOrEmpty(jwt))
            {
            }

            var cookie = new Cookie("realmikefacts_session", jwt)
            {
                HttpOnly = true,
                Expires = expires,
                Expired = expired
            };
            var serializedCookie = $"{cookie}; HttpOnly; Expires={cookie.Expires}; secure; SameSite=Lax;";
            var response = new RealMikeFactsAuthorizationResponse
            {
                StatusCode = ApiStatusCode.OK,
                Body = JsonConvert.SerializeObject(jwt)
            };
            response.SetHeaderValues("Set-Cookie", serializedCookie, true);
            response.SetHeaderValues("access-control-expose-headers", "Set-Cookie", true);
            response.SetHeaderValues("Access-Control-Allow-Headers", "Set-Cookie", true);
            response.SetHeaderValues("Access-Control-Allow-Credentials", "true", true);
            response.SetHeaderValues("Access-Control-Allow-Origin", "https://api.realmikefacts.com", true);
            response.SetHeaderValues("Access-Control-Allow-Headers", "Set-Cookie", true);

            return response;
        }
    }
}
