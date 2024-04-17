using Microsoft.Maui.Handlers;
using Mobile.Interfaces;

namespace Mobile.Platforms.Windows;

public class WindowsHttpMessageHandler : IPlatformHttpMessageHandler
{
    public HttpMessageHandler GetHttpMessageHandler()
        => new HttpClientHandler
        {
          ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        
}
