using Azure.DigitalTwins.Core;
using Derby.DigitalTwins.ClassLibrary;

namespace Derby.DigitalTwins.MSTest
{

    [TestClass]
    public class TestClass_B_ModelManager
    {
        private ModelManager _modeleManager;
        public string _digitalTwinsResourceName;
        [TestInitialize]
        public void TestInitialize()
        {
            _modeleManager = new ModelManager();
            _digitalTwinsResourceName = "TestAzureDigitalTwinsInstance";
        }

        [TestMethod]
        [DataRow(1, @"https://raw.githubusercontent.com/gayanvoice/Derby.Azure.Project/master/Derby.DigitalTwins.MSTest/Static/parentModel.json", "dtmi:dtdl:context:parentModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.A.1 - Uploading Parent DTDL Model")]
        [DataRow(2, @"https://raw.githubusercontent.com/gayanvoice/Derby.Azure.Project/master/Derby.DigitalTwins.MSTest/Static/childModel.json", "dtmi:dtdl:context:childModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.A.2 - Uploading Child DTDL Model")]
        [DataRow(3, @"https://raw.githubusercontent.com/gayanvoice/Derby.Azure.Project/master/Derby.DigitalTwins.MSTest/Static/PrimitiveModel.json", "dtmi:dtdl:context:primitiveModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.A.3 - Uploading Primitive DTDL Model")]
        [DataRow(4, @"https://raw.githubusercontent.com/gayanvoice/Derby.Azure.Project/master/Derby.DigitalTwins.MSTest/Static/ComplexModel.json", "dtmi:dtdl:context:complexModel;1", DisplayName = "Derby.DigitalTwins.MSTest.B.A.4 - Uploading Complex DTDL Model")]
        public async Task TestMethod_A_UploadDtdlModelAsync(int order, string fileUrl, string id)
        {
            DigitalTwinsModelData digitalTwinsModelData = await _modeleManager.UploadDtdlModel(fileUrl);
            Assert.AreEqual(digitalTwinsModelData.Id, id);
        }
    }
}
