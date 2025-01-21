using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DaikinConnectDotnet.Models;

namespace DaikinConnectDotnet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DaikinApiHttpClient _client;

    public HomeController(ILogger<HomeController> logger, DaikinApiHttpClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<IActionResult> Index(string? code = null)
    {
        string? deviceData = null;
        if (code != null)
        {
            var accessToken = await _client.GetAccessToken(code);
            if (accessToken != null)
            {
                deviceData = await _client.GetDeviceData(accessToken);
            }
            else {
                deviceData = "There was an error getting the access token for the Daikin API, try again later...";
            }
        }
        var daikinAPIModel = new DaikinAPIModel{ DaikinAuthURL = _client.GetDaikinSsoUri(), DeviceData = deviceData };
        return View(daikinAPIModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
