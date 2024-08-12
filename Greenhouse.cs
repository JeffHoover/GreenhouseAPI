using System;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Greenhouse;
public class Greenhouse
{
    static readonly string userName = "7a6a10907c13f97b24828b6bd09ace36-4";
    static readonly string userPassword = "";
    static readonly string authenticationString = $"{userName}:{userPassword}";
    static readonly string base64String = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));

    public async Task goAsync()
    {
        HttpResponseMessage response = GetResponse("scorecards");

        if (response.Headers.Contains("link"))
        {
            var input = response.Headers.GetValues("link").First();
            // <https://harvest.greenhouse.io/v1/scorecards?page=2&per_page=100>; rel="next",<https://harvest.greenhouse.io/v1/scorecards?page=91&per_page=100>; rel="last"

            var linkToFind = @"""next""";
            Regex regex = new($"<([^>]+)>; rel={linkToFind}");

            Match match = regex.Match(input);
            string url = match.Groups[1].Value;
            Console.WriteLine(linkToFind + "---> " + url);
        }

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);

        //if (result.EnsureSuccessStatusCode().IsSuccessStatusCode)
        //Console.WriteLine(content);
    }

    // TODO - can httpClient.Send() take an absolute or something to append to BaseAddress?
    private HttpResponseMessage GetResponse(string route)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, route);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64String);
        var response = sharedClient.Send(requestMessage);
        return response;
    }

    public async Task<string?> GetNextSchedulesAsync()
    {
        HttpResponseMessage response = GetResponse("scorecards");

        string url = "";
        if (response.Headers.Contains("link"))
        {
            var input = response.Headers.GetValues("link").First();
            // <https://harvest.greenhouse.io/v1/scorecards?page=2&per_page=100>; rel="next",<https://harvest.greenhouse.io/v1/scorecards?page=91&per_page=100>; rel="last"

            var linkToFind = @"""next""";
            Regex regex = new($"<([^>]+)>; rel={linkToFind}");

            Match match = regex.Match(input);
            url = match.Groups[1].Value;
            Console.WriteLine(linkToFind + "---> " + url);
        }

        Console.WriteLine("++++ " + url);
        var finalResponse = GetResponse(url);
        return await finalResponse.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetSchedulesAsync()
    {
        var response = GetResponse("scorecards");
        return await response.Content.ReadAsStringAsync();
    }

    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://harvest.greenhouse.io/v1/"),
    };
}

