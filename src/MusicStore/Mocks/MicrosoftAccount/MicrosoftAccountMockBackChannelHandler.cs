﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNet.WebUtilities;

namespace MusicStore.Mocks.MicrosoftAccount
{
    /// <summary>
    /// Summary description for MicrosoftAccountMockBackChannelHandler
    /// </summary>
    public class MicrosoftAccountMockBackChannelHandler : HttpMessageHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage();

            if (request.RequestUri.AbsoluteUri.StartsWith("https://login.live.com/oauth20_token.srf"))
            {
                var formData = FormHelpers.ParseForm(await request.Content.ReadAsStringAsync());
                if (formData["grant_type"] == "authorization_code")
                {
                    if (formData["code"] == "ValidCode")
                    {
                        if (formData["redirect_uri"] != null && formData["redirect_uri"].EndsWith("signin-microsoft") &&
                           formData["client_id"] == "[ClientId]" && formData["client_secret"] == "[ClientSecret]")
                        {
                            System.Console.WriteLine("Handler2");
                            response.Content = new StringContent("{\"token_type\":\"bearer\",\"expires_in\":3600,\"scope\":\"wl.basic\",\"access_token\":\"ValidAccessToken\",\"refresh_token\":\"ValidRefreshToken\",\"authentication_token\":\"ValidAuthenticationToken\"}");
                        }
                    }
                }
            }
            else if (request.RequestUri.AbsoluteUri.StartsWith("https://apis.live.net/v5.0/me"))
            {
                if (request.Headers.Authorization.Parameter == "ValidAccessToken")
                {
                    response.Content = new StringContent("{\r   \"id\": \"fccf9a24999f4f4f\", \r   \"name\": \"AspnetvnextTest AspnetvnextTest\", \r   \"first_name\": \"AspnetvnextTest\", \r   \"last_name\": \"AspnetvnextTest\", \r   \"link\": \"https://profile.live.com/\", \r   \"gender\": null, \r   \"locale\": \"en_US\", \r   \"updated_time\": \"2013-08-27T22:18:14+0000\"\r}");
                }
                else
                {
                    response.Content = new StringContent("{\r   \"error\": {\r      \"code\": \"request_token_invalid\", \r      \"message\": \"The access token isn't valid.\"\r   }\r}", Encoding.UTF8, "text/javascript");
                }
            }

            return response;
        }
    }
}