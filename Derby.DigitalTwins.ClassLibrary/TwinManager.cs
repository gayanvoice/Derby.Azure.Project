using Azure;
using Azure.DigitalTwins.Core;
using System.Text.Json;
using System.Text.Json.Nodes;

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
            Console.WriteLine($"ModelId: {basicDigitalTwin2.Metadata.ModelId} Id: {basicDigitalTwin2.Id}");
            return basicDigitalTwin2;
        }
        public async Task<BasicDigitalTwin> GetBasicDigitalTwinAsync(string twinId)
        {
            Console.WriteLine($"Getting Basic Digital Twin Async");
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response<BasicDigitalTwin> basicDigitalTwinResponse = await digitalTwinsClient.GetDigitalTwinAsync<BasicDigitalTwin>(twinId);
            BasicDigitalTwin basicDigitalTwin = basicDigitalTwinResponse.Value;
            Console.WriteLine($"ModelId: {basicDigitalTwin.Metadata.ModelId}  Id: {basicDigitalTwin.Id}");
            return basicDigitalTwin;
        }
        public async Task<Dictionary<string, object>> GetContentDictionaryAsync(string twinId)
        {
            Console.WriteLine($"Getting Basic Digital Twin Async");
            BasicDigitalTwin basicDigitalTwin = await GetBasicDigitalTwinAsync(twinId);
            Console.WriteLine($"ModelId: {basicDigitalTwin.Metadata.ModelId} Id: {basicDigitalTwin.Id}");
            Dictionary<string, object> contentDictionary = new Dictionary<string, object>();
            ICollection<string> keyCollection = basicDigitalTwin.Contents.Keys;
            if (keyCollection.Count() > 0)
            {
                Console.WriteLine($"Has {keyCollection.Count()} Contents");
                foreach (string key in keyCollection)
                {
                    object value  = basicDigitalTwin.Contents[key];
                    Console.WriteLine($"Key: {key} Value: {value}");
                    contentDictionary.Add(key, value);
                }
            }
            else
            {
                Console.WriteLine("No Contents");
            }
            return contentDictionary;
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