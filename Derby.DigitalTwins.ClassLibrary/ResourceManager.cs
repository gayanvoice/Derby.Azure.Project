using Azure;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using System.Diagnostics;

namespace Derby.DigitalTwins.ClassLibrary
{
    //https://learn.microsoft.com/en-us/dotnet/api/overview/azure/resourcemanager-readme?view=azure-dotnet

    public class ResourceManager
    {
        private string _tenentId;
        public ResourceManager(string tenentId)
        {
            _tenentId = "5ecda7e7-179b-4603-85f3-302815e102fe";
        }

        public async Task<SubscriptionResource> GetSubscriptionResourceAsync()
        {
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { TenantId = _tenentId });
            ArmClient armClient = new ArmClient(defaultAzureCredential);
            SubscriptionResource subscriptionResource = await armClient.GetDefaultSubscriptionAsync();
            Trace.WriteLine($"Getting Subscription Resource");
            Trace.WriteLine($"Subscription Resource Id: {subscriptionResource.Data.SubscriptionId}");
            Trace.WriteLine($"Subscription Resource Name: {subscriptionResource.Data.DisplayName}");
            Trace.WriteLine($"Subscription Resource Status: {subscriptionResource.Data.State}");
            return subscriptionResource;
        }

        public async Task<bool> CheckIfResourceGroupCollectionExistsAsync(string resourceGroupName)
        {
            Trace.WriteLine($"Check If Resource Group Collection Exists");
            SubscriptionResource subscriptionResource = await GetSubscriptionResourceAsync();
            ResourceGroupCollection resourceGroupCollection = subscriptionResource.GetResourceGroups();
            bool resourceGroupCollectionExists = await resourceGroupCollection.ExistsAsync(resourceGroupName);
            Trace.WriteLine($"Resource Group Collection: {resourceGroupCollectionExists}");
            return resourceGroupCollectionExists;
        }
        public async Task<ResourceGroupResource> CreateResourceGroupResource(string resourceGroupName)
        {
            SubscriptionResource subscriptionResource = await GetSubscriptionResourceAsync();
            ResourceGroupCollection resourceGroupCollection = subscriptionResource.GetResourceGroups();
            ResourceGroupData resourceGroupData = new ResourceGroupData(location: Azure.Core.AzureLocation.UKSouth);
            ArmOperation<ResourceGroupResource> resourceGroupResourceArmOperation = await resourceGroupCollection
                .CreateOrUpdateAsync(waitUntil:WaitUntil.Completed, resourceGroupName: resourceGroupName, data:resourceGroupData);
            Trace.WriteLine($"Create Resource Group Resource");
            Trace.WriteLine($"Resource Group Resource Id: {resourceGroupResourceArmOperation.Value.Data.Id}");
            Trace.WriteLine($"Resource Group Resource Name: {resourceGroupResourceArmOperation.Value.Data.Name}");
            Trace.WriteLine($"Resource Group Resource Location: {resourceGroupResourceArmOperation.Value.Data.Location}");
            return resourceGroupResourceArmOperation.Value;
        }
    }
}
