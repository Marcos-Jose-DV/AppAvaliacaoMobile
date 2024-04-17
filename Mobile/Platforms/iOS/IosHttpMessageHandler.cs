using Mobile.Interfaces;
using Security;

namespace Mobile.Platforms.iOS;

public class IosHttpMessageHandler : IPlatformHttpMessageHandler
{
    public HttpMessageHandler GetHttpMessageHandler() =>
        new NSUrlSessionHandler
        {
            TrustOverrideForUrl = (NSUrlSessionHandler sender, string url, SecTrust trust)
                => url.StartsWith("http://localhost")
        };
}