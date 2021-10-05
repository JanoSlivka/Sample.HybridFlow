
namespace StavebnyDennik;
public class PlatfromRepository
{
    private static string _refreshToken;

    public static bool ExistRefreshToken()
        => !string.IsNullOrEmpty(_refreshToken);

    public static string GetRefreshToken()
        => _refreshToken;

    public static void SetRefreshToken(string refreshToken)
    {
        _refreshToken = refreshToken;
    }
}
