using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StavebnyDennik.Pages;

public class Projects2Model : PageModel
{
    public Projects2Model()
    {
    }

    public string Message { get; set; }
    public bool IsButtonVisible { get; set; }

    public async Task OnGetAsync([FromQuery] string? code)
    {
        IsButtonVisible = false;
        if (PlatfromRepository.ExistRefreshToken())
        {            
            await LoadData();
        }
        else if (!string.IsNullOrEmpty(code))
        {
            Message = $"Vytváram spojenie s platformou";
            await ObtainRefreshToken(code);
            await LoadData();
        }
        else
        {
            IsButtonVisible = true;
            Message = $"Je potrebné prepojenie s platformou!";
        }
    }

    public ActionResult OnPost()
    {
        var state = Guid.NewGuid().ToString("N");
        var url = "https://localhost:6001/connect/authorize" +
            "?client_id=StavebnyDennik" +
            "&response_type=code id_token" +
            "&redirect_uri=https%3A%2F%2Flocalhost%3A5001%2Ftest" +
            "&scope=projects%20openid%20offline_access%20profile" +
            "&state=" + state +
            "&nonce=" + Guid.NewGuid().ToString("N");

        return Redirect(url);
    }

    private async Task ObtainRefreshToken(string code)
    {
        var resp = await "https://localhost:6001/connect/token".PostUrlEncodedAsync(new
        {
            client_id = "StavebnyDennik",
            client_secret = "mysecret",
            grant_type = "authorization_code",
            code,
            redirect_uri = "https://localhost:5001/test"
        }).ReceiveJson();

        PlatfromRepository.SetRefreshToken(resp.refresh_token);
    }

    private static async Task<string> GetAccessToken()
    {
        var resp = await "https://localhost:6001/connect/token".PostUrlEncodedAsync(new
        {
            client_id = "StavebnyDennik",
            client_secret = "mysecret",
            grant_type = "refresh_token",
            refresh_token = PlatfromRepository.GetRefreshToken()
        }).ReceiveJson();

        return resp.access_token;
    }

    public IEnumerable<Project> Projects { get; private set; }

    private async Task LoadData()
    {
        Message = $"Prepojenie s platformou úspešne prebehlo. Zobrazujem dáta z platformy,.";
        string accessToken = await GetAccessToken();
        Projects = await "https://localhost:5101/api/projects".WithOAuthBearerToken(accessToken).GetJsonAsync<IEnumerable<Project>>();
    }
}
