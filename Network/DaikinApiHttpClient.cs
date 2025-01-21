using System.Text.Json;

public class DaikinApiHttpClient {
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public string? _clientId { get; set; }
    private readonly string? _clientSecret;

    public DaikinApiHttpClient(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
        _clientId = _configuration["DaikinClientID"];
        _clientSecret = Environment.GetEnvironmentVariable("DAIKIN_CLIENT_SECRET");
    }

    public async Task<string?> GetAccessToken( string code )
    {
        var tokenFetchUriBuilder = new UriBuilder("https://idp.onecta.daikineurope.com/v1/oidc/token");
        var authQuery = new []{
            "grant_type=authorization_code",
            $"client_id={_clientId}",
            $"client_secret={_clientSecret}",
            $"code={code}",
            $"redirect_uri=https://chief-colt-novel.ngrok-free.app"
        };
        tokenFetchUriBuilder.Query = string.Join('&', authQuery);
        
        var authResponse = await _client.PostAsync(tokenFetchUriBuilder.Uri, null);
        if (authResponse.IsSuccessStatusCode)
        {
            var data = await authResponse.Content.ReadAsStringAsync();
            return JsonDocument.Parse(data).RootElement.GetProperty("access_token").GetString();
        }
        else
        {
            return null;
        }
    }

    public async Task<string?> GetDeviceData( string accessToken ) {
        
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.onecta.daikineurope.com/v1/gateway-devices"),
            Headers = { {"Authorization",$"Bearer {accessToken}"} }
        };
        var authResponse = await _client.SendAsync(httpRequestMessage);

        if (authResponse.IsSuccessStatusCode)
        {
            return await authResponse.Content.ReadAsStringAsync();
        }
        else
        {
            return "There was an error reading device data, try again later...";
        }
    }

    public string GetDaikinSsoUri()
    {
        var ssoUriBuiler = new UriBuilder("https://idp.onecta.daikineurope.com/v1/oidc/authorize");
        var ssoQuery = new []
        {
            "response_type=code",
            $"client_id={_clientId}",
            $"scope=openid%20onecta:basic.integration",
            "redirect_uri=https://chief-colt-novel.ngrok-free.app"
        };
        ssoUriBuiler.Query = string.Join('&', ssoQuery);
        return ssoUriBuiler.ToString();
    }
}