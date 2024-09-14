using Azure.ResourceManager.Authorization;
using Azure.ResourceManager.DigitalTwins;
using Derby.Authorization.ClassLibrary;
using Derby.DigitalTwins.ClassLibrary;

namespace Derby.DigitalTwins.MSTest
{
    [TestClass]
    public class TestClass_1_DigitalTwinsResourceManager
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
        [TestMethod]
        [DataRow(DisplayName = "Test Method 1 - Create Digital Twins Description Resource Async")]
        public async Task TestMethod_1_CreateDigitalTwinsDescriptionResourceAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager.CreateDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            Assert.IsNotNull(digitalTwinsDescriptionResource);
            Assert.AreEqual(digitalTwinsDescriptionResource.Data.Name, _digitalTwinsResourceName);
        }
        [TestMethod]
        [DataRow(DisplayName = "Test Method 2 - Create Role Assignment Async")]
        public async Task TestMethod_2_CreateRoleAssignmentAsync()
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
        [DataRow(DisplayName = "Test Method 3 - Check If Digital Twins Description Exists Async")]
        public async Task TestMethod_3_CheckIfDigitalTwinsDescriptionExistsAsync()
        {
            bool digitalTwinsDescriptionExists = await _digitalTwinsResourceManager.CheckIfDigitalTwinsDescriptionExistsAsync(_digitalTwinsResourceName);
            Assert.IsTrue(digitalTwinsDescriptionExists);
        }
        [TestMethod]
        [DataRow(DisplayName = "Test Method 4 - Get Digital Twins Description Resource Async")]
        public async Task TestMethod_4_GetDigitalTwinsDescriptionResourceAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager.GetDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            Assert.IsNotNull(digitalTwinsDescriptionResource);
            Assert.AreEqual(_digitalTwinsResourceName, digitalTwinsDescriptionResource.Data.Name);
        }

        [TestMethod]
        [DataRow(DisplayName = "Test Method 5 - Get Digital Twins Description Collection Async")]
        public async Task TestMethod_5_GetDigitalTwinsDescriptionCollectionAsync()
        {
            DigitalTwinsDescriptionCollection digitalTwinsDescriptionCollection = await _digitalTwinsResourceManager.GetDigitalTwinsDescriptionCollectionAsync();
            Assert.IsNotNull(digitalTwinsDescriptionCollection);
            Assert.IsTrue(digitalTwinsDescriptionCollection.Count() > 0);
        }
        [TestMethod]
        [DataRow(DisplayName = "Test Method 6 - Get Role Assignment Data List Async")]
        public async Task TestMethod_6_GetRoleAssignmentListAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager.GetDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            List<RoleAssignmentData> roleAssignmentDataList = await _authorizationResourceManager.GetRoleAssignmentDataListAsync(resourceId: digitalTwinsDescriptionResource.Data.Id);
            Assert.IsTrue(roleAssignmentDataList.Count() > 0);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Test Method 7 - Get Role Assignment Async")]
        public async Task TestMethod_7_GetRoleAssignmentAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager.GetDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            RoleAssignmentData roleAssignmentData = await _authorizationResourceManager.GetRoleAssignmentAsync(resourceId: digitalTwinsDescriptionResource.Data.Id, name: "Azure Digital Twins Data Owner");
            Assert.IsNotNull(roleAssignmentData);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Test Method 8 - Delete Role Assignment Async")]
        public async Task TestMethod_8_DeleteRoleAssignmentAsync()
        {
            DigitalTwinsDescriptionResource digitalTwinsDescriptionResource = await _digitalTwinsResourceManager
                .GetDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            RoleAssignmentData roleAssignmentData = await _authorizationResourceManager
                .DeleteRoleAssignmentAsync(resourceId: digitalTwinsDescriptionResource.Data.Id, name: "Azure Digital Twins Data Owner");
            AuthorizationRoleDefinitionData authorizationRoleDefinitionData = await _authorizationResourceManager
                .GetAuthorizationRoleDefinitionData(resourceId: digitalTwinsDescriptionResource.Data.Id, name: "Azure Digital Twins Data Owner");
            Assert.AreEqual(roleAssignmentData.RoleDefinitionId, authorizationRoleDefinitionData.Id);
        }
        [TestMethod]
        [DataRow(DisplayName = "Test Method 9 - Delete Digital Twins Description Resource Async")]
        public async Task TestMethod_9_DeleteDigitalTwinsDescriptionResourceAsync()
        {
            bool resourceGroupCollectionExists = await _digitalTwinsResourceManager.DeleteDigitalTwinsDescriptionResourceAsync(_digitalTwinsResourceName);
            Assert.IsFalse(resourceGroupCollectionExists);
        }
    }
}