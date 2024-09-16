using Azure;
using Azure.DigitalTwins.Core;

namespace Derby.DigitalTwins.ClassLibrary
{
    public class TwinManager
    {
        private DigitalTwinsResourceManager _digitalTwinsResourceManager;
        public string _digitalTwinsResourceName;
        public TwinManager()
        {
            _digitalTwinsResourceManager = new DigitalTwinsResourceManager(tenentId: "5ecda7e7-179b-4603-85f3-302815e102fe", resourceGroupResourceName: "TestResourceGroup");
            _digitalTwinsResourceName = "TestAzureDigitalTwinsInstance";
        }
        public async Task<BasicDigitalTwin> CreateBasicDigitalTwinAsync(string twinId, BasicDigitalTwin basicDigitalTwin)
        {
            Console.WriteLine($"Creating Basic Digital Twin Async");
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response<BasicDigitalTwin> basicDigitalTwinResponse = await digitalTwinsClient.CreateOrReplaceDigitalTwinAsync<BasicDigitalTwin>(twinId, basicDigitalTwin);
            BasicDigitalTwin basicDigitalTwin2 = basicDigitalTwinResponse.Value;
            Console.WriteLine($"Id: {basicDigitalTwin2.Id}");
            return basicDigitalTwin2;
        }
        public void GetTwinAsync(string twinId)
        {
            throw new NotImplementedException();
        }
        public void CreateRelationshipAsync(string fromTwinId, string toTwinId)
        {
            throw new NotImplementedException();
        }
        public void GetRelationshipAsync(string relationshipId)
        {
            throw new NotImplementedException();
        }
        public void GetIncomingRelationshipAsync(string twinId)
        {
            throw new NotImplementedException();
        }
        public void PublishTelemetryAsync(string twinId)
        {
            throw new NotImplementedException();
        }
        public void UpdateDigitalTwinAsync(string twinId)
        {
            throw new NotImplementedException();
        }
        public void QueryAsync(string query)
        {
            throw new NotImplementedException();
        }
        public void DeleteRelationship(string relationshipId)
        {
            throw new NotImplementedException();
        }
        public void DeleteTwin(string twinId)
        {
            throw new NotImplementedException();
        }
    }
}