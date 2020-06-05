using Pulumi;
using ApiManagement = Pulumi.Azure.ApiManagement;
using Pulumi.Azure.Core;
using Pulumi.Azure.Storage;
using System.IO;

class MyStack : Stack
{
    public MyStack()
    {
        // Create an Azure Resource Group
        var group = new ResourceGroup("your-api-demo");

        // Create Azure API Managment instance
        var apiService = new ApiManagement.Service("your-api-demo", new ApiManagement.ServiceArgs
        {
            ResourceGroupName = group.Name,
            PublisherName = "Your Name",
            PublisherEmail = "nospam@nospam.com",
            SkuName = "Developer_1"
        });

        this.GatewayUrl = apiService.GatewayUrl;
        this.PortalUrl = apiService.PortalUrl;

        // Create Conference API
        var exampleApi = new ApiManagement.Api("Conf", new ApiManagement.ApiArgs
        {
            ResourceGroupName = group.Name,
            ApiManagementName = apiService.Name,
            Revision = "1",
            DisplayName = "Conference API",
            Path = "conference",
            Protocols =
            {
                "https",
            },
            Import = new ApiManagement.Inputs.ApiImportArgs
            {
                ContentFormat = "swagger-link-json",
                ContentValue = "http://conferenceapi.azurewebsites.net/?format=json",
            },
        });
    }

    [Output]
    public Output<string> GatewayUrl { get; set; }

    [Output]
    public Output<string> PortalUrl { get; set; }

}
