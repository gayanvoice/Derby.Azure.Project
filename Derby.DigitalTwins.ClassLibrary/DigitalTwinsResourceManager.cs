using Azure.ResourceManager.DigitalTwins;
using Azure;
using Azure.ResourceManager.Resources;
using Derby.Subscription.ClassLibrary;
using Azure.Core;
using Azure.ResourceManager;

namespace Derby.DigitalTwins.ClassLibrary
{
    // https://learn.microsoft.com/en-us/azure/digital-twins/how-to-set-up-instance-portal
    public class DigitalTwinsResourceManager
    {
        private SubscriptionResourceManager _subscriptionResourceManager;
        public string _resourceGroupResourceName;
        public DigitalTwinsResourceManager(string tenentId, string resourceGroupResourceName)
        {
            _subscriptionResourceManager = new SubscriptionResourceManager(tenentId);
            _resourceGroupResourceName = resourceGroupResourceName;
        }
        public async Task<DigitalTwinsDescriptionCollection> GetDigitalTwinsDescriptionCollectionAsync()
        {
            Console.WriteLine($"Getting Digital Twins Description Collection");
            ResourceGroupResource resourceGroupResource = await _subscriptionResourceManager.GetResourceGroupResourceAsync(_resourceGroupResourceName);
            DigitalTwinsDescriptionCollection digitalTwinsDescriptionCollection = resourceGroupResource.GetDigitalTwinsDescriptions();
            await foreach (DigitalTwinsDescriptionResource digitalTwinsDescriptionResource in digitalTwinsDescriptionCollection)
            {
                Console.WriteLine($"Id: {digitalTwinsDescriptionResource.Data.Id} Name: {digitalTwinsDescriptionResource.Data.Name} Location: {digitalTwinsDescriptionResource.Data.Location} " +
                      $"Provisioning State: {digitalTwinsDescriptionResource.Data.ProvisioningState}");
            }
            return digitalTwinsDescriptionCollection;
        }
        public async Task<bool> CheckIfDigitalTwinsDescriptionExistsAsync(string digitalTwinsResourceName)
        {
            Console.WriteLine($"Checking If Digital Twins Description Collection Exists");
            DigitalTwinsDescriptionCollection digitalTwinsDescriptionCollection = await GetDigitalTwinsDescriptionCollectionAsync();
            bool digitalTwinsDescriptionExists = await digitalTwinsDescriptionCollection.ExistsAsync(digitalTwinsResourceName);
            Console.WriteLine($"Exists: {digitalTwinsDescriptionExists}");
            return digitalTwinsDescriptionExists;
        }
        public async Task<DigitalTwinsDescriptionResource> GetDigitalTwinsDescriptionResourceAsync(string digitalTwinsResourceName)
        {
            Console.WriteLine($"Getting Digital Twins Description Resource");
            DigitalTwinsDescriptionCollection digitalTwinsDescriptionCollection = await GetDigitalTwinsDescriptionCollectionAsync();
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await digitalTwinsDescriptionCollection.GetAsync(digitalTwinsResourceName);
            Console.WriteLine($"Id: {digitalTwinsDescriptionResource.Data.Id} Name: {digitalTwinsDescriptionResource.Data.Name} Location: {digitalTwinsDescriptionResource.Data.Location} " +
                $"Provisioning State: {digitalTwinsDescriptionResource.Data.ProvisioningState}");
            return digitalTwinsDescriptionResource;
        }
        public async Task<DigitalTwinsDescriptionResource> CreateDigitalTwinsDescriptionResourceAsync(string digitalTwinsResourceName)
        {
            Console.WriteLine($"Creating Digital Twins Description Resource");
            DigitalTwinsDescriptionCollection digitalTwinsDescriptionCollection = await GetDigitalTwinsDescriptionCollectionAsync();
            DigitalTwinsDescriptionData digitalTwinsDescriptionData = new DigitalTwinsDescriptionData(AzureLocation.UKSouth);
            digitalTwinsDescriptionData.PublicNetworkAccess = Azure.ResourceManager.DigitalTwins.Models.DigitalTwinsPublicNetworkAccess.Enabled;
            ArmOperation<DigitalTwinsDescriptionResource> digitalTwinsDescriptionResourceArmOperation = await digitalTwinsDescriptionCollection
                .CreateOrUpdateAsync(waitUntil: WaitUntil.Completed, resourceName: digitalTwinsResourceName, data: digitalTwinsDescriptionData);
            Console.WriteLine($"Id: {digitalTwinsDescriptionResourceArmOperation.Value.Data.Id} Name: {digitalTwinsDescriptionResourceArmOperation.Value.Data.Name} " +
                $"Location: {digitalTwinsDescriptionResourceArmOperation.Value.Data.Location} Provisioning State: {digitalTwinsDescriptionResourceArmOperation.Value.Data.ProvisioningState}");
            return digitalTwinsDescriptionResourceArmOperation.Value;
        }
        public async Task<bool> DeleteDigitalTwinsDescriptionResourceAsync(string digitalTwinsResourceName)
        {
            Console.WriteLine($"Getting Digital Twins Description Resource");
            DigitalTwinsDescriptionCollection digitalTwinsDescriptionCollection = await GetDigitalTwinsDescriptionCollectionAsync();
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await digitalTwinsDescriptionCollection.GetAsync(digitalTwinsResourceName);
            Console.WriteLine($"Id: {digitalTwinsDescriptionResource.Data.Id} Name: {digitalTwinsDescriptionResource.Data.Name} Location: {digitalTwinsDescriptionResource.Data.Location} " +
                $"Provisioning State: {digitalTwinsDescriptionResource.Data.ProvisioningState}");
            await digitalTwinsDescriptionResource.DeleteAsync(WaitUntil.Completed);
            digitalTwinsDescriptionCollection = await GetDigitalTwinsDescriptionCollectionAsync();
            bool resourceGroupCollectionExists = await digitalTwinsDescriptionCollection.ExistsAsync(digitalTwinsResourceName);
            Console.WriteLine($"Exists: {resourceGroupCollectionExists}");
            return resourceGroupCollectionExists;
        }
    }
}