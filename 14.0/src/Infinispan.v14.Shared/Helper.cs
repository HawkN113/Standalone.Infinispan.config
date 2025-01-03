using System.Net;

namespace Infinispan.v14.Shared;

internal static class Helper
{
    public static HttpClient GetClient(NetworkCredential credentials, Uri baseAddress)
    {
        var handler = new HttpClientHandler
        {
            Credentials = credentials
        };
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = baseAddress
        };
        return httpClient;
    }
}