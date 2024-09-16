using Azure;
using Azure.DigitalTwins.Core;
using Derby.DigitalTwins.ClassLibrary;

namespace Derby.DigitalTwins.MSTest
{

    [TestClass]
    public class TestClass_B_ModelManager
    {
        private ModelManager _modelManager;
        public string _digitalTwinsResourceName;
        [TestInitialize]
        public void TestInitialize()
        {
            _modelManager = new ModelManager();
            _digitalTwinsResourceName = "TestAzureDigitalTwinsInstance";
        }
        [TestMethod]
        [DataRow(@"https://raw.githubusercontent.com/gayanvoice/Derby.Azure.Project/master/Derby.DigitalTwins.MSTest/Static/PrimitiveModel.json", "dtmi:dtdl:context:primitiveModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.A - Uploading Primitive DTDL Model")]
        [DataRow(@"https://raw.githubusercontent.com/gayanvoice/Derby.Azure.Project/master/Derby.DigitalTwins.MSTest/Static/ComplexModel.json", "dtmi:dtdl:context:complexModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.A - Uploading Complex DTDL Model")]
        [DataRow(@"https://raw.githubusercontent.com/gayanvoice/Derby.Azure.Project/master/Derby.DigitalTwins.MSTest/Static/Factory.json", "dtmi:dtdl:context:Factory;1", DisplayName = "Derby.DigitalTwins.MSTest.B.A - Uploading Factory DTDL Model")]
        [DataRow(@"https://raw.githubusercontent.com/gayanvoice/Derby.Azure.Project/master/Derby.DigitalTwins.MSTest/Static/Robot.json", "dtmi:dtdl:context:Robot;1", DisplayName = "Derby.DigitalTwins.MSTest.B.A - Uploading Robot DTDL Model")]
        public async Task TestMethod_A_UploadDtdlModelAsync(string fileUrl, string id)
        {
            try
            {
                string dtdlModelFile = await new HttpClient().GetStringAsync(fileUrl);
                DigitalTwinsModelData digitalTwinsModelData = await _modelManager.UploadDtdlModel(dtdlModelFile);
                Assert.AreEqual(digitalTwinsModelData.Id, id);
            }
            catch (RequestFailedException requestFailedException)
            {
                Assert.AreEqual(requestFailedException.ErrorCode, "ModelIdAlreadyExists");
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }
        [TestMethod]
        [DataRow("dtmi:dtdl:context:primitiveModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.B - Getting Primitive DTDL Model")]
        [DataRow("dtmi:dtdl:context:complexModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.B - Getting Complex DTDL Model")]
        [DataRow("dtmi:dtdl:context:Factory;1", DisplayName = "Derby.DigitalTwins.MSTest.B.B - Getting Factory DTDL Model")]
        [DataRow("dtmi:dtdl:context:Robot;1", DisplayName = "Derby.DigitalTwins.MSTest.B.B - Getting Robot DTDL Model")]
        public async Task TestMethod_B_GetDtdlModelAsync(string modelId)
        {
            DigitalTwinsModelData digitalTwinsModelData = await _modelManager.GetDtdlModel(modelId);
            Assert.AreEqual(digitalTwinsModelData.Id, modelId);
        }
        [Ignore]
        [TestMethod]
        [DataRow("dtmi:dtdl:context:primitiveModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.D - Deleting Primitive DTDL Model")]
        [DataRow("dtmi:dtdl:context:complexModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.D - Deleting Complex DTDL Model")]
        [DataRow("dtmi:dtdl:context:Factory;1", DisplayName = "Derby.DigitalTwins.MSTest.B.D - Deleting Factory DTDL Model")]
        [DataRow("dtmi:dtdl:context:Robot;1", DisplayName = "Derby.DigitalTwins.MSTest.B.D - Deleting Robot DTDL Model")]
        public async Task TestMethod_D_DeleteDtdlModelAsync(string modelId)
        {
            Response response = await _modelManager.DeleteDtdlModel(modelId);
            Assert.AreEqual(response.Status, 204);
        }
    }
}
