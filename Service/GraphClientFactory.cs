using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using OneDriveAmazoneS3.Service.Interface;
using OneDriveAmazoneS3.Settings;

namespace OneDriveAmazoneS3.Service
{
    public class GraphClientFactory : IGraphClientFactory
    {
        private readonly MicrosoftGraphSettings _microsoftGraphSettings;
        public GraphClientFactory(IOptions<MicrosoftGraphSettings> microsoftGraphSettings)
        {
            _microsoftGraphSettings = microsoftGraphSettings.Value;
        }
        public GraphServiceClient CreateClient()
        {
            var credential = new ClientSecretCredential(
                _microsoftGraphSettings.TenantId,
                _microsoftGraphSettings.ClientId,
                _microsoftGraphSettings.ClientSecret
            );
            return new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });
        }
    }
}
