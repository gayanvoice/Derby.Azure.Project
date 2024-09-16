using Azure.Core;
using Azure.ResourceManager;
using Azure;
using Azure.ResourceManager.IotHub;
using Azure.ResourceManager.Resources;
using Derby.Subscription.ClassLibrary;
using Azure.ResourceManager.IotHub.Models;
using System.Net.Http.Headers;
using Azure.ResourceManager.Models;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

namespace Derby.IoTHub.ClassLibrary
{
    public class IotHubResourceManager
    {
        private SubscriptionResourceManager _subscriptionResourceManager;
        public string _resourceGroupResourceName;
        public IotHubResourceManager(string tenentId, string resourceGroupResourceName)
        {
            _subscriptionResourceManager = new SubscriptionResourceManager(tenentId);
            _resourceGroupResourceName = resourceGroupResourceName;
        }
        public async Task<IotHubDescriptionCollection> GetIotHubDescriptionCollectionAsync()
        {
            Console.WriteLine($"Getting IoT Hub Description Collection");
            ResourceGroupResource resourceGroupResource = await _subscriptionResourceManager.GetResourceGroupResourceAsync(_resourceGroupResourceName);
            IotHubDescriptionCollection iotHubDescriptionCollection = resourceGroupResource.GetIotHubDescriptions();
            await foreach (IotHubDescriptionResource iotHubDescriptionResource in iotHubDescriptionCollection)
            {
                Console.WriteLine($"Id: {iotHubDescriptionResource.Data.Id} Name: {iotHubDescriptionResource.Data.Name} Location: {iotHubDescriptionResource.Data.Location} " +
                    $"SKU: {iotHubDescriptionResource.Data.Sku.Name}");
            }
            return iotHubDescriptionCollection;
        }
        public async Task<IotHubNameAvailabilityResponse> CheckIfIotHubNameAvailabilityAsync(string iotHubResourceName)
        {
            Console.WriteLine($"Checking If IoT Hub Description Collection Exists");
            SubscriptionResource subscriptionResource = await _subscriptionResourceManager.GetSubscriptionResourceAsync();
            IotHubNameAvailabilityContent iotHubNameAvailabilityContent = new IotHubNameAvailabilityContent(iotHubResourceName);
            IotHubNameAvailabilityResponse iotHubNameAvailabilityResponse = await subscriptionResource.CheckIotHubNameAvailabilityAsync(iotHubNameAvailabilityContent);
            return iotHubNameAvailabilityResponse;
        }
        public async Task<bool> CheckIfIotHubDescriptionExistsAsync(string iotHubResourceName)
        {
            Console.WriteLine($"Checking If IoT Hub Description Collection Exists");
            IotHubDescriptionCollection iotHubDescriptionCollection = await GetIotHubDescriptionCollectionAsync();
            bool iotHubDescriptionExists = await iotHubDescriptionCollection.ExistsAsync(iotHubResourceName);
            Console.WriteLine($"Exists: {iotHubDescriptionExists}");
            return iotHubDescriptionExists;
        }
        public async Task<IotHubDescriptionResource> GetIotHubDescriptionResourceAsync(string iotHubResourceName)
        {
            Console.WriteLine($"Getting IoT Hub Description Resource");
            IotHubDescriptionCollection iotHubDescriptionCollection = await GetIotHubDescriptionCollectionAsync();
            IotHubDescriptionResource iotHubDescriptionResource = await iotHubDescriptionCollection.GetAsync(iotHubResourceName);
            Console.WriteLine($"Id: {iotHubDescriptionResource.Data.Id} Name: {iotHubDescriptionResource.Data.Name} Location: {iotHubDescriptionResource.Data.Location} " +
                $"SKU: {iotHubDescriptionResource.Data.Sku.Name}");
            return iotHubDescriptionResource;
        }
        //https://azure.microsoft.com/en-gb/pricing/details/iot-hub/
        //https://learn.microsoft.com/en-us/dotnet/api/azure.resourcemanager.iothub.iothubdescriptiondata.sku?view=azure-dotnet
        //https://learn.microsoft.com/en-us/dotnet/api/azure.resourcemanager.iothub.models.iothubskuinfo?view=azure-dotnet
        //https://github.com/Azure/azure-rest-api-specs-examples/blob/708f9d8ec4651a2a8ecd6b34a767cb1f47399670/specification/iothub/resource-manager/Microsoft.Devices/stable/2023-06-30/examples-dotnet/iothub_createOrUpdate.cs
        public async Task<IotHubDescriptionResource> CreateIotHubDescriptionResourceAsync(string iotHubResourceName)
        {
            Console.WriteLine($"Creating Digital Twins Description Resource");
            IotHubDescriptionCollection iotHubDescriptionCollection = await GetIotHubDescriptionCollectionAsync();

            IotHubSkuInfo iotHubSkuInfo = new IotHubSkuInfo(IotHubSku.F1);
            iotHubSkuInfo.Capacity = 1;

            EventHubCompatibleEndpointProperties eventHubCompatibleEndpointProperties = new EventHubCompatibleEndpointProperties();
            eventHubCompatibleEndpointProperties.RetentionTimeInDays = 1;
            eventHubCompatibleEndpointProperties.PartitionCount = 2;


            IotHubProperties iotHubProperties = new IotHubProperties();
            iotHubProperties.PublicNetworkAccess = IotHubPublicNetworkAccess.Enabled;
            //Requested IoT Hub features "MinimumTlsVersion1_2, RootCertificateV2" not available in 'uksouth' region.
            //iotHubProperties.MinTlsVersion = "1.2";
            iotHubProperties.EventHubEndpoints.Add("events", eventHubCompatibleEndpointProperties);

            IotHubDescriptionData iotHubDescriptionData = new IotHubDescriptionData(AzureLocation.UKSouth, iotHubSkuInfo);
            iotHubDescriptionData.Properties = iotHubProperties;

            ArmOperation<IotHubDescriptionResource> iotHubDescriptionResourceArmOperation = await iotHubDescriptionCollection
                .CreateOrUpdateAsync(waitUntil: WaitUntil.Completed, resourceName: iotHubResourceName, data: iotHubDescriptionData);

            Console.WriteLine($"Id: {iotHubDescriptionResourceArmOperation.Value.Data.Id} Name: {iotHubDescriptionResourceArmOperation.Value.Data.Name} " +
                $"Location: {iotHubDescriptionResourceArmOperation.Value.Data.Location} SKU: {iotHubDescriptionResourceArmOperation.Value.Data.Sku.Name}");
            return iotHubDescriptionResourceArmOperation.Value;
        }
        public async Task<bool> DeleteDigitalTwinsDescriptionResourceAsync(string iotHubResourceName)
        {
            Console.WriteLine($"Getting Digital Twins Description Resource");
            IotHubDescriptionCollection iotHubDescriptionCollection = await GetIotHubDescriptionCollectionAsync();
            IotHubDescriptionResource iotHubDescriptionResource = await iotHubDescriptionCollection.GetAsync(iotHubResourceName);
            Console.WriteLine($"Id: {iotHubDescriptionResource.Data.Id} Name: {iotHubDescriptionResource.Data.Name} Location: {iotHubDescriptionResource.Data.Location} " +
                $"SKU: {iotHubDescriptionResource.Data.Sku.Name}");
            // System.ArgumentNullException: Value cannot be null. (Parameter 'id')
            try
            {
                await iotHubDescriptionResource.DeleteAsync(WaitUntil.Completed);
                IotHubDescriptionCollection iotHubDescriptionCollection2 = await GetIotHubDescriptionCollectionAsync();
                bool resourceGroupCollectionExists = await iotHubDescriptionCollection2.ExistsAsync(iotHubResourceName);
                Console.WriteLine($"Exists: {resourceGroupCollectionExists}");
                return resourceGroupCollectionExists;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Exists: false");
                return false;
            }
        }
    }
}