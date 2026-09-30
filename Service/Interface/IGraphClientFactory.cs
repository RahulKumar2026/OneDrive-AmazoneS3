using Microsoft.Graph;
namespace OneDriveAmazoneS3.Service.Interface
{
    public interface IGraphClientFactory
    {
        GraphServiceClient CreateClient();
    }
}
