using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class AccessToken
    {
       // public static async Task<string> GetAccessTokenFromJSONKeyAsync(string jsonKeyFilePath, params string[] scopes)
       // {
       //     using (var stream = new FileStream(jsonKeyFilePath, FileMode.Open, FileAccess.Read))
       //     {
       //         var cred =  GoogleCredential
       //             .FromStream(stream) // Loads key file  
       //             .CreateScoped(scopes) // Gathers scopes requested  
       //             .UnderlyingCredential // Gets the credentials  
       //             .GetAccessTokenForRequestAsync(); // Gets the Access Token  
       //     }
       // }
       //// ~/assets/contentid-api-internal-e65f4f6da0f9.json
       // public static string GetAccessTokenFromJSONKey(string jsonKeyFilePath)
       // {
       //     string[] scopes1 = { "https://www.googleapis.com/auth/youtubepartner",
       //     "https://www.googleapis.com/auth/youtube",
       //     "https://www.googleapis.com/auth/youtube.readonly",
       //     "https://www.googleapis.com/auth/youtubepartner",
       //     "https://www.googleapis.com/auth/yt-analytics-monetary.readonly",
       //     "https://www.googleapis.com/auth/yt-analytics.readonly" };
       //     return GetAccessTokenFromJSONKeyAsync(jsonKeyFilePath, scopes1).Result;
       // }

       // public static string GetTokenAndCall()
       // {
       //     string path = HttpContext.Current.Server.MapPath("~/assets/contentid-api-internal-e65f4f6da0f9.json");
       //     var token = GetAccessTokenFromJSONKey(path);
       //     //Console.WriteLine(token);
       //     return token;
       //     //Console.WriteLine(new HttpClient().GetStringAsync($"https://www.googleapis.com/plus/v1/people/110259743757395873050?access_token={token}").Result);
       // }
    }
}