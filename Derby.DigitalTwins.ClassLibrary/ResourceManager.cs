using Azure;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

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
            Console.WriteLine($"Getting Subscription Resource");
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { TenantId = _tenentId });
            ArmClient armClient = new ArmClient(defaultAzureCredential);
            SubscriptionResource subscriptionResource = await armClient.GetDefaultSubscriptionAsync();
            Console.WriteLine($"Subscription Resource Id: {subscriptionResource.Data.SubscriptionId}");
            Console.WriteLine($"Subscription Resource Name: {subscriptionResource.Data.DisplayName}");
            Console.WriteLine($"Subscription Resource Status: {subscriptionResource.Data.State}");
            return subscriptionResource;
        }

        public async Task<bool> CheckIfResourceGroupCollectionExistsAsync(string resourceGroupName)
        {
            Console.WriteLine($"Check If Resource Group Collection Exists");
            SubscriptionResource subscriptionResource = await GetSubscriptionResourceAsync();
            ResourceGroupCollection resourceGroupCollection = subscriptionResource.GetResourceGroups();
            bool resourceGroupCollectionExists = await resourceGroupCollection.ExistsAsync(resourceGroupName);
            Console.WriteLine($"Resource Group Collection: {resourceGroupCollectionExists}");
            return resourceGroupCollectionExists;
        }
        public async Task<ResourceGroupResource> CreateResourceGroupResource(string resourceGroupName)
        {
            Console.WriteLine($"Create Resource Group Resource");
            SubscriptionResource subscriptionResource = await GetSubscriptionResourceAsync();
            ResourceGroupCollection resourceGroupCollection = subscriptionResource.GetResourceGroups();
            ResourceGroupData resourceGroupData = new ResourceGroupData(location: Azure.Core.AzureLocation.UKSouth);
            ArmOperation<ResourceGroupResource> resourceGroupResourceArmOperation = await resourceGroupCollection
                .CreateOrUpdateAsync(waitUntil:WaitUntil.Completed, resourceGroupName: resourceGroupName, data:resourceGroupData);
            Console.WriteLine($"Resource Group Resource Id: {resourceGroupResourceArmOperation.Value.Data.Id}");
            Console.WriteLine($"Resource Group Resource Name: {resourceGroupResourceArmOperation.Value.Data.Name}");
            Console.WriteLine($"Resource Group Resource Location: {resourceGroupResourceArmOperation.Value.Data.Location}");
            return resourceGroupResourceArmOperation.Value;
        }
    }
}