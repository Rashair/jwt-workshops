using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace CookieAuthWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SignInManager<IdentityUser> _signInManager;

    public IndexModel(ILogger<IndexModel> logger, SignInManager<IdentityUser> signInManager)
    {
        _logger = logger;
        _signInManager = signInManager;
    }

    public void OnGet()
    {
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
          var res = DecryptAuthCookie(HttpContext);
          _logger.LogInformation(res.ToString());
        }
    }


    public static string DecryptAuthCookie(HttpContext httpContext)
    {
        // ONE - grab the CookieAuthenticationOptions instance
        var opt = httpContext.RequestServices
            .GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>()
            .Get(IdentityConstants.ApplicationScheme);

        // TWO - Get the encrypted cookie value
        var cookie = opt.CookieManager.GetRequestCookie(httpContext, opt.Cookie.Name!);
        var dataProtector = opt.DataProtectionProvider!.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware",
            IdentityConstants.ApplicationScheme,
            "v2");

        byte[] protectedBytes = Base64UrlTextEncoder.Decode(cookie!);
        byte[] plainBytes = dataProtector.Unprotect(protectedBytes);
        var specialUtf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        string plainText = specialUtf8Encoding.GetString(plainBytes);

        // THREE - decrypt it
        return plainText;
    }
}