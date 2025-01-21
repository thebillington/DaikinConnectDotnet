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
            var accessToken = await _client.getAccessToken(code);
            if (accessToken != "-1")
            {

            }
            else {
                deviceData = "There was an error fetching device data from the Daikin API, try again later...";
            }
        }
        var daikinAPIModel = new DaikinAPIModel{ DaikinAuthURL = _client.getDaikinSsoUri(), DeviceData = deviceData };
        return View(daikinAPIModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
