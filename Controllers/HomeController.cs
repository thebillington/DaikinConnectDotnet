using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DaikinConnectDotnet.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace DaikinConnectDotnet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? code = null)
    {
        var clientID = _configuration["DaikinClientID"];
        string? deviceData = null;

        if (code != null) {
            var clientSecret = Environment.GetEnvironmentVariable("DAIKIN_CLIENT_SECRET");
            // This should go elsewhere as a formattable string but not sure on convention
            string tokenFetchURL = Regex.Replace(@$"https://idp.onecta.daikineurope.com/v1/oidc/token?
                grant_type=authorization_code
                &client_id={clientID}
                &client_secret={clientSecret}
                &code={code}
                &redirect_uri=https://chief-colt-novel.ngrok-free.app", @"\s+", "");
            // This has to be a post request even though all the data is in url params - content is null
            HttpResponseMessage authResponse = await _httpClient.PostAsync(tokenFetchURL, null);
            if (authResponse.IsSuccessStatusCode) {
                string data = authResponse.Content.ReadAsStringAsync().Result;
                // Need to work out best way to convert this to JSON and extract access_token
                deviceData = data;
            }
        }

        string daikinAuthURL = @$"https://idp.onecta.daikineurope.com/v1/oidc/authorize
            ?response_type=code
            &client_id={clientID}
            &redirect_uri=https://chief-colt-novel.ngrok-free.app
            &scope=openid%20onecta:basic.integration";
        DaikinAPIModel daikinAPIModel = new DaikinAPIModel{ DaikinAuthURL = Regex.Replace(daikinAuthURL, @"\s+", ""), DeviceData = deviceData };
        return View(daikinAPIModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
