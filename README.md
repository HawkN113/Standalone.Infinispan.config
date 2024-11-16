# Standlone.Infinispan.config
This project allows to get standlone distributed cache (Infinispan) and working with cache


# Infinispan user roles and permissions

https://infinispan.org/docs/14.0.x/titles/configuring/configuring.html#mapping_users_to_roles_and_permissions_in_infinispan
https://infinispan.org/docs/stable/titles/security/security.html

Reader1 - 123qweASD
Applicant1 - 123qweASD
Monitor1 - 123qweASD

```c#
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

public class DigestAuthenticationHandler
{
    private static readonly HttpClient _httpClient = new HttpClient();

    // This method will perform Digest Authentication and send a request
    public async Task<string> SendDigestAuthenticatedRequestAsync(string url, string username, string password)
    {
        // Step 1: Perform an initial request to get the WWW-Authenticate header and nonce
        HttpResponseMessage initialResponse = await _httpClient.GetAsync(url);
        if (initialResponse.StatusCode != System.Net.HttpStatusCode.Unauthorized)
        {
            throw new Exception("Authentication required, but request was successful.");
        }

        // Step 2: Extract the nonce from the WWW-Authenticate header
        var authHeader = initialResponse.Headers.WwwAuthenticate.ToString();
        string nonce = ExtractNonceFromAuthHeader(authHeader);

        // Step 3: Prepare the Digest Authentication header using the extracted nonce
        string digestHeader = CreateDigestHeader(username, password, nonce, url);

        // Step 4: Send the actual request with the Digest Authentication header
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Digest", digestHeader);
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            // Return the response body if the request is successful
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception($"Request failed with status code {response.StatusCode}");
        }
    }

    // Helper method to extract nonce from the WWW-Authenticate header
    private string ExtractNonceFromAuthHeader(string authHeader)
    {
        var nonceStartIndex = authHeader.IndexOf("nonce=\"") + 7;
        var nonceEndIndex = authHeader.IndexOf("\"", nonceStartIndex);
        return authHeader.Substring(nonceStartIndex, nonceEndIndex - nonceStartIndex);
    }

    // Helper method to create the Digest Authentication header
    private string CreateDigestHeader(string username, string password, string nonce, string uri)
    {
        string realm = "exampleRealm";  // This should be extracted from the WWW-Authenticate header too
        string method = "GET";  // HTTP method (GET, POST, etc.)

        // Calculate the A1 and A2 hashes
        string a1 = $"{username}:{realm}:{password}";
        string a2 = $"{method}:{uri}";
        string ha1 = CalculateMD5Hash(a1);
        string ha2 = CalculateMD5Hash(a2);

        // Calculate the response hash
        string response = CalculateMD5Hash($"{ha1}:{nonce}:{ha2}");

        // Build the Digest header
        return $"username=\"{username}\", realm=\"{realm}\", nonce=\"{nonce}\", uri=\"{uri}\", response=\"{response}\"";
    }

    // Helper method to calculate the MD5 hash of a string
    private string CalculateMD5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}

```

```c#
class Program
{
    static async Task Main(string[] args)
    {
        var url = "http://example.com/protected/resource";  // Replace with your URL
        var username = "yourUsername";
        var password = "yourPassword";

        var digestAuthHandler = new DigestAuthenticationHandler();
        var result = await digestAuthHandler.SendDigestAuthenticatedRequestAsync(url, username, password);
        Console.WriteLine(result);
    }
}

```