using Azure.DigitalTwins.Core;
using Azure.ResourceManager.Authorization;
using Azure.ResourceManager.DigitalTwins;
using Derby.Authorization.ClassLibrary;
using Derby.DigitalTwins.ClassLibrary;

namespace Derby.DigitalTwins.MSTest
{
    [TestClass]
    public class TestClass_A_DigitalTwinsResourceManager
    {
        private DigitalTwinsResourceManager _digitalTwinsResourceManager;
        private AuthorizationResourceManager _authorizationResourceManager;
        public string _digitalTwinsResourceName;
        [TestInitialize]
        public void TestInitialize()
        {
            _digitalTwinsResourceManager = new DigitalTwinsResourceManager(tenentId: "5ecda7e7-179b-4603-85f3-302815e102fe", resourceGroupResourceName: "TestResourceGroup");
            _authorizationResourceManager = new AuthorizationResourceManager(tenentId: "5ecda7e7-179b-4603-85f3-302815e102fe", principleId: "e48f4382-218a-4fb6-a6b3-e9b4eacd62c3");
            _digitalTwinsResourceName = "TestAzureDigitalTwinsInstance";
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Creating Digital Twins Description Resource Async")]
        public async Task TestMethod_A_CreateDigitalTwinsDescriptionResourceAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager.CreateDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            Assert.IsNotNull(digitalTwinsDescriptionResource);
            Assert.AreEqual(digitalTwinsDescriptionResource.Data.Name, _digitalTwinsResourceName);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Creating Role Assignment Async")]
        public async Task TestMethod_B_CreateRoleAssignmentAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager
                .GetDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            RoleAssignmentData roleAssignmentData = await _authorizationResourceManager
                .CreateRoleAssignmentAsync(resourceId: digitalTwinsDescriptionResource.Data.Id, name: "Azure Digital Twins Data Owner");
            AuthorizationRoleDefinitionData authorizationRoleDefinitionData = await _authorizationResourceManager
                .GetAuthorizationRoleDefinitionData(resourceId: digitalTwinsDescriptionResource.Data.Id, name: "Azure Digital Twins Data Owner");
            Assert.AreEqual(roleAssignmentData.RoleDefinitionId, authorizationRoleDefinitionData.Id);
        }
        [TestMethod]
        [DataRow(DisplayName = "Checking If Digital Twins Description Exists Async")]
        public async Task TestMethod_C_CheckIfDigitalTwinsDescriptionExistsAsync()
        {
            bool digitalTwinsDescriptionExists = await _digitalTwinsResourceManager.CheckIfDigitalTwinsDescriptionExistsAsync(_digitalTwinsResourceName);
            Assert.IsTrue(digitalTwinsDescriptionExists);
        }
        [TestMethod]
        [DataRow(DisplayName = "Getting Digital Twins Description Collection Async")]
        public async Task TestMethod_D_GetDigitalTwinsDescriptionCollectionAsync()
        {
            DigitalTwinsDescriptionCollection digitalTwinsDescriptionCollection = await _digitalTwinsResourceManager.GetDigitalTwinsDescriptionCollectionAsync();
            Assert.IsNotNull(digitalTwinsDescriptionCollection);
            Assert.IsTrue(digitalTwinsDescriptionCollection.Count() > 0);
        }
        [TestMethod]
        [DataRow(DisplayName = "Getting Digital Twins Description Resource Async")]
        public async Task TestMethod_E_GetDigitalTwinsDescriptionResourceAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager.GetDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            Assert.IsNotNull(digitalTwinsDescriptionResource);
            Assert.AreEqual(_digitalTwinsResourceName, digitalTwinsDescriptionResource.Data.Name);
        }
        [TestMethod]
        [DataRow(DisplayName = "Getting Digital Twins Client Async")]
        public async Task TestMethod_F_GetDigitalTwinsClientAsync()
        {
            DigitalTwinsClient digitalTwinsClient = await _digitalTwinsResourceManager.GetDigitalTwinsClientAsync(_digitalTwinsResourceName);
            Assert.IsNotNull(digitalTwinsClient);
        }
        [TestMethod]
        [DataRow(DisplayName = "Getting Role Assignment Data List Async")]
        public async Task TestMethod_G_GetRoleAssignmentListAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager.GetDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            List<RoleAssignmentData> roleAssignmentDataList = await _authorizationResourceManager.GetRoleAssignmentDataListAsync(resourceId: digitalTwinsDescriptionResource.Data.Id);
            Assert.IsTrue(roleAssignmentDataList.Count() > 0);
        }
        [TestMethod]
        [DataRow(DisplayName = "Getting Role Assignment Async")]
        public async Task TestMethod_H_GetRoleAssignmentAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager.GetDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            RoleAssignmentData roleAssignmentData = await _authorizationResourceManager.GetRoleAssignmentDataAsync(resourceId: digitalTwinsDescriptionResource.Data.Id, name: "Azure Digital Twins Data Owner");
            Assert.IsNotNull(roleAssignmentData);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Deleting Role Assignment Async")]
        public async Task TestMethod_I_DeleteRoleAssignmentAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager
                .GetDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            bool roleAssignmentDataExists = await _authorizationResourceManager
                .DeleteRoleAssignmentResourceAsync(resourceId: digitalTwinsDescriptionResource.Data.Id, name: "Azure Digital Twins Data Owner");
            Assert.IsFalse(roleAssignmentDataExists);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Deleting Digital Twins Description Resource Async")]
        public async Task TestMethod_J_DeleteDigitalTwinsDescriptionResourceAsync()
        {
            bool resourceGroupCollectionExists = await _digitalTwinsResourceManager.DeleteDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            Assert.IsFalse(resourceGroupCollectionExists);
        }
    }
}