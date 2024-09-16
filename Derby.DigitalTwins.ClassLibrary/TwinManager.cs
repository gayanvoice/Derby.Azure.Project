using Azure;
using Azure.DigitalTwins.Core;
using System;
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
            Console.WriteLine($"Creating Basic Digital Twin");
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response<BasicDigitalTwin> basicDigitalTwinResponse = await digitalTwinsClient.CreateOrReplaceDigitalTwinAsync<BasicDigitalTwin>(twinId, basicDigitalTwin);
            BasicDigitalTwin basicDigitalTwin2 = basicDigitalTwinResponse.Value;
            Console.WriteLine($"Model Id: {basicDigitalTwin2.Metadata.ModelId} Id: {basicDigitalTwin2.Id}");
            return basicDigitalTwin2;
        }
        public async Task<BasicDigitalTwin> GetBasicDigitalTwinAsync(string twinId)
        {
            Console.WriteLine($"Getting Basic Digital Twin");
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response<BasicDigitalTwin> basicDigitalTwinResponse = await digitalTwinsClient.GetDigitalTwinAsync<BasicDigitalTwin>(twinId);
            BasicDigitalTwin basicDigitalTwin = basicDigitalTwinResponse.Value;
            Console.WriteLine($"Model Id: {basicDigitalTwin.Metadata.ModelId}  Id: {basicDigitalTwin.Id}");
            return basicDigitalTwin;
        }
        public async Task<Dictionary<string, object>> GetContentDictionaryAsync(string twinId)
        {
            Console.WriteLine($"Getting Basic Digital Twin");
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

        public async Task<BasicRelationship> CreateBasicRelationshipAsync(string sourceId, string targetId, string name)
        {
            Console.WriteLine($"Creating Basic Relationship Twin");
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            BasicRelationship basicRelationship = new BasicRelationship();
            basicRelationship.TargetId = targetId;
            basicRelationship.Name = name;
            string relationshipId = $"{sourceId}-{name}-{targetId}";
            Response<BasicRelationship> basicRelationshipResponse = await digitalTwinsClient
                .CreateOrReplaceRelationshipAsync<BasicRelationship>(digitalTwinId: sourceId, relationshipId:relationshipId, relationship:basicRelationship);
            BasicRelationship basicRelationship2 = basicRelationshipResponse.Value;
            Console.WriteLine($"Id: {basicRelationship2.Id} Source Id: {basicRelationship2.SourceId} Target Id: {basicRelationship2.TargetId}");
            return basicRelationship2;
        }
        public async Task<BasicRelationship> GetBasicRelationshipAsync(string sourceId, string relationshipId)
        {
            Console.WriteLine($"Getting Basic Relationship Twin");
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response<BasicRelationship> basicRelationshipResponse = await digitalTwinsClient.GetRelationshipAsync<BasicRelationship>(digitalTwinId: sourceId, relationshipId: relationshipId);
            BasicRelationship basicRelationship = basicRelationshipResponse.Value;
            Console.WriteLine($"Id: {basicRelationship.Id} Source Id: {basicRelationship.SourceId} Target Id: {basicRelationship.TargetId}");
            return basicRelationship;
        }
        public async Task<List<IncomingRelationship>> GetIncomingRelationshipListAsync(string targetId)
        {
            Console.WriteLine($"Getting Incoming Basic Relationships Twin");
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            AsyncPageable<IncomingRelationship> incomingRelationshipAsyncPageable = digitalTwinsClient.GetIncomingRelationshipsAsync(digitalTwinId: targetId);
            List<IncomingRelationship> incomingRelationshipList = new List<IncomingRelationship>();
            await foreach (IncomingRelationship incomingRelationship in incomingRelationshipAsyncPageable)
            {
                Console.WriteLine($"Relationship Id: {incomingRelationship.RelationshipId} Relationship Name: {incomingRelationship.RelationshipName} Relationship Link: {incomingRelationship.RelationshipLink}");
                incomingRelationshipList.Add(incomingRelationship);
            }
            return incomingRelationshipList;
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
        public async Task<Response> DeleteRelationshipAsync(string sourceId, string relationshipId)
        {
            Console.WriteLine($"Deleting Basic Digital Twin");
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response response = await digitalTwinsClient.DeleteRelationshipAsync(digitalTwinId: sourceId, relationshipId: relationshipId);
            Console.WriteLine($"Status: {response.Status}");
            return response;
        }
        public async Task<Response> DeleteDigitalTwinAsync(string twinId)
        {
            Console.WriteLine($"Deleting Basic Digital Twin");
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response response = await digitalTwinsClient.DeleteDigitalTwinAsync(twinId);
            Console.WriteLine($"Status: {response.Status}");
            return response;
        }
    }
}