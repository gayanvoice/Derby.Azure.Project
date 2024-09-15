using Azure;
using Azure.DigitalTwins.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<DigitalTwinsModelData> UploadDtdlModel(string fileUrl)
        {
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            string dtdlModelFile = await new HttpClient().GetStringAsync(fileUrl);
            Response<DigitalTwinsModelData[]>  digitalTwinsModelDataArrayResponse = await digitalTwinsClient.CreateModelsAsync(new[] { dtdlModelFile });
            DigitalTwinsModelData[] digitalTwinsModelDataArray = digitalTwinsModelDataArrayResponse.Value;
            foreach (DigitalTwinsModelData digitalTwinsModelData in digitalTwinsModelDataArray)
            {
                Console.WriteLine($"Id: {digitalTwinsModelData.Id}");
            }
            return digitalTwinsModelDataArray.First();
        }

        public void GetModel()
        {
            throw new NotImplementedException();
        }

        public void DeleteModel()
        {
            throw new NotImplementedException();
        }
    }
}
