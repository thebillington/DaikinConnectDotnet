using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DaikinConnectDotnet.Models;
using System.Text.RegularExpressions;

namespace DaikinConnectDotnet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var clientID = _configuration["DaikinClientID"];
        var daikinAuthURL = @$"https://idp.onecta.daikineurope.com/v1/oidc/authorize
            ?response_type=code
            &client_id={clientID}
            &redirect_uri=https://chief-colt-novel.ngrok-free.app
            &scope=openid%20onecta:basic.integration";
        var DaikinAPIModel = new DaikinAPIModel{ DaikinAuthURL = Regex.Replace(daikinAuthURL, @"\s+", "") };
        return View(DaikinAPIModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
