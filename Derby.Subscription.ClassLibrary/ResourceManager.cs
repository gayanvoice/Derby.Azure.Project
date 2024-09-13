using Azure;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace Derby.Subscription.ClassLibrary
{
    //https://learn.microsoft.com/en-us/dotnet/api/overview/azure/resourcemanager-readme?view=azure-dotnet

    public class ResourceManager
    {
        private string _tenentId;
        public ResourceManager(string tenentId)
        {
            _tenentId = "5ecda7e7-179b-4603-85f3-302815e102fe";
        }

        private async Task<SubscriptionResource> GetSubscriptionResourceAsync()
        {
            Console.WriteLine($"Getting Subscription Resource");
            DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions() { TenantId = _tenentId });
            ArmClient armClient = new ArmClient(defaultAzureCredential);
            SubscriptionResource subscriptionResource = await armClient.GetDefaultSubscriptionAsync();
            Console.WriteLine($"Id: {subscriptionResource.Data.SubscriptionId} Name: {subscriptionResource.Data.DisplayName} " +
                $"Status: {subscriptionResource.Data.State}");
            return subscriptionResource;
        }

        public async Task<bool> CheckIfResourceGroupCollectionExistsAsync(string resourceGroupName)
        {
            Console.WriteLine($"Checking If Resource Group Collection Exists");
            SubscriptionResource subscriptionResource = await GetSubscriptionResourceAsync();
            ResourceGroupCollection resourceGroupCollection = subscriptionResource.GetResourceGroups();
            bool resourceGroupCollectionExists = await resourceGroupCollection.ExistsAsync(resourceGroupName);
            Console.WriteLine($"Exists: {resourceGroupCollectionExists}");
            return resourceGroupCollectionExists;
        }
        public async Task<ResourceGroupResource> GetResourceGroupResourceAsync(string resourceGroupName)
        {
            Console.WriteLine($"Getting Resource Group Resource");
            SubscriptionResource subscriptionResource = await GetSubscriptionResourceAsync();
            ResourceGroupCollection resourceGroupCollection = subscriptionResource.GetResourceGroups();
            ResourceGroupResource resourceGroupResource = await resourceGroupCollection.GetAsync(resourceGroupName);
            Console.WriteLine($"Id: {resourceGroupResource.Data.Id} Name: {resourceGroupResource.Data.Name} " +
                $"Location: {resourceGroupResource.Data.Location}");
            return resourceGroupResource;
        }
        public async Task<ResourceGroupCollection> GetAllResourceGroupCollectionAsync()
        {
            Console.WriteLine($"Getting All Resource Group Collection");
            SubscriptionResource subscriptionResource = await GetSubscriptionResourceAsync();
            ResourceGroupCollection resourceGroupCollection = subscriptionResource.GetResourceGroups();
            await foreach (ResourceGroupResource resourceGroupResource in resourceGroupCollection)
            {
                Console.WriteLine($"Id: {resourceGroupResource.Data.Id} Name: {resourceGroupResource.Data.Name} " +
                    $"Location: {resourceGroupResource.Data.Location}");
            }
            return resourceGroupCollection;
        }
        public async Task<ResourceGroupResource> CreateResourceGroupResourceAsync(string resourceGroupName)
        {
            Console.WriteLine($"Creating Resource Group Resource");
            SubscriptionResource subscriptionResource = await GetSubscriptionResourceAsync();
            ResourceGroupCollection resourceGroupCollection = subscriptionResource.GetResourceGroups();
            ResourceGroupData resourceGroupData = new ResourceGroupData(location: Azure.Core.AzureLocation.UKSouth);
            ArmOperation<ResourceGroupResource> resourceGroupResourceArmOperation = await resourceGroupCollection
                .CreateOrUpdateAsync(waitUntil:WaitUntil.Completed, resourceGroupName: resourceGroupName, data:resourceGroupData);
            Console.WriteLine($"Id: {resourceGroupResourceArmOperation.Value.Data.Id} Name: {resourceGroupResourceArmOperation.Value.Data.Name} " +
                $"Location: {resourceGroupResourceArmOperation.Value.Data.Location}");
            return resourceGroupResourceArmOperation.Value;
        }
        public async Task<bool> DeleteResourceGroupResourceAsync(string resourceGroupName)
        {
            Console.WriteLine($"Deleting Resource Group Resource");
            SubscriptionResource subscriptionResource = await GetSubscriptionResourceAsync();
            ResourceGroupCollection resourceGroupCollection = subscriptionResource.GetResourceGroups();
            ResourceGroupResource resourceGroupResource = await resourceGroupCollection.GetAsync(resourceGroupName);
            Console.WriteLine($"Id: {resourceGroupResource.Data.Id} Name: {resourceGroupResource.Data.Name}");
            await resourceGroupResource.DeleteAsync(WaitUntil.Completed);
            resourceGroupCollection = subscriptionResource.GetResourceGroups();
            bool resourceGroupCollectionExists = await resourceGroupCollection.ExistsAsync(resourceGroupName);
            Console.WriteLine($"Exists: {resourceGroupCollectionExists}");
            return resourceGroupCollectionExists;
        }
    }
}