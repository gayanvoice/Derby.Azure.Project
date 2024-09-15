using Azure;
using Azure.DigitalTwins.Core;
using DTDLParser;
using DTDLParser.Models;

namespace Derby.DigitalTwins.ClassLibrary
{
    public class ModelManager
    {
        private DigitalTwinsResourceManager _digitalTwinsResourceManager;
        public string _digitalTwinsResourceName;
        public ModelManager()
        {
            _digitalTwinsResourceManager = new DigitalTwinsResourceManager(tenentId: "5ecda7e7-179b-4603-85f3-302815e102fe", resourceGroupResourceName: "TestResourceGroup");
            _digitalTwinsResourceName = "TestAzureDigitalTwinsInstance";
        }
        public async Task<DigitalTwinsModelData> UploadDtdlModel(string dtdlModelFile)
        {
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response<DigitalTwinsModelData[]>  digitalTwinsModelDataArrayResponse = await digitalTwinsClient.CreateModelsAsync(new[] { dtdlModelFile });
            DigitalTwinsModelData[] digitalTwinsModelDataArray = digitalTwinsModelDataArrayResponse.Value;
            foreach (DigitalTwinsModelData digitalTwinsModelData in digitalTwinsModelDataArray)
            {
                Console.WriteLine($"Id: {digitalTwinsModelData.Id}");
            }
            return digitalTwinsModelDataArray.First();
        }
        public async Task<DigitalTwinsModelData> GetDtdlModel(string modelId)
        {
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response<DigitalTwinsModelData> digitalTwinsModelDataResponse = await digitalTwinsClient.GetModelAsync(modelId);
            DigitalTwinsModelData digitalTwinsModelData = digitalTwinsModelDataResponse.Value;
            Console.WriteLine($"Id: {digitalTwinsModelData.Id} Dtdl Model: {digitalTwinsModelData.DtdlModel}");
            return digitalTwinsModelData;
        }
        public async Task<Response> DeleteDtdlModel(string modelId)
        {
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Response response = await digitalTwinsClient.DeleteModelAsync(modelId);
            Console.WriteLine($"Status: {response.Status}");
            return response;
        }
    }
}
