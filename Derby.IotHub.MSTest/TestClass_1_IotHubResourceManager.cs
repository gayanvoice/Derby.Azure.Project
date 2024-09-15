using Azure.ResourceManager.Authorization;
using Azure.ResourceManager.IotHub;
using Azure.ResourceManager.IotHub.Models;
using Derby.Authorization.ClassLibrary;
using Derby.IoTHub.ClassLibrary;

namespace Derby.IotHub.MSTest
{
    [TestClass]
    public class TestClass_1_IotHubResourceManager
    {
        private IotHubResourceManager _iotHubResourceManager;
        private AuthorizationResourceManager _authorizationResourceManager;
        public string _iotHubResourceName;
        [TestInitialize]
        public void TestInitialize()
        {
            _iotHubResourceManager = new IotHubResourceManager(tenentId: "5ecda7e7-179b-4603-85f3-302815e102fe", resourceGroupResourceName: "TestResourceGroup");
            _authorizationResourceManager = new AuthorizationResourceManager(tenentId: "5ecda7e7-179b-4603-85f3-302815e102fe", principleId: "e48f4382-218a-4fb6-a6b3-e9b4eacd62c3");
            _iotHubResourceName = "TestIotHubInstance1";
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Test Method A - Check If IoT Hub Name Availability Async")]
        public async Task TestMethod_A_CheckIfIotHuNameAvailabilityAsync()
        {
            IotHubNameAvailabilityResponse IotHubNameAvailabilityResponse = await _iotHubResourceManager.CheckIfIotHubNameAvailabilityAsync(_iotHubResourceName);
            Assert.IsTrue(IotHubNameAvailabilityResponse.IsNameAvailable);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Test Method B - Create IoT Hub Description Resource Async")]
        public async Task TestMethod_B_CreateDigitalTwinsDescriptionResourceAsync()
        {
            IotHubDescriptionResource iotHubDescriptionResource = await _iotHubResourceManager.CreateIotHubDescriptionResourceAsync(_iotHubResourceName);
            Assert.IsNotNull(iotHubDescriptionResource);
            Assert.AreEqual(iotHubDescriptionResource.Data.Name, _iotHubResourceName);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Test Method C - Create Role Assignment Async")]
        public async Task TestMethod_C_CreateRoleAssignmentAsync()
        {
            IotHubDescriptionResource iotHubDescriptionResource = await _iotHubResourceManager.GetIotHubDescriptionResourceAsync(_iotHubResourceName);
            RoleAssignmentData roleAssignmentData = await _authorizationResourceManager
                .CreateRoleAssignmentAsync(resourceId: iotHubDescriptionResource.Data.Id, name: "IoT Hub Data Contributor");
            AuthorizationRoleDefinitionData authorizationRoleDefinitionData = await _authorizationResourceManager
                .GetAuthorizationRoleDefinitionData(resourceId: iotHubDescriptionResource.Data.Id, name: "IoT Hub Data Contributor");
            Assert.AreEqual(roleAssignmentData.RoleDefinitionId, authorizationRoleDefinitionData.Id);
        }
        [TestMethod]
        [DataRow(DisplayName = "Test Method D - Get IoT Hub Description Collection Async")]
        public async Task TestMethod_D_GetIotHubDescriptionCollectionAsync()
        {
            IotHubDescriptionCollection iotHubDescriptionCollection = await _iotHubResourceManager.GetIotHubDescriptionCollectionAsync();
            Assert.IsNotNull(iotHubDescriptionCollection);
            Assert.IsTrue(iotHubDescriptionCollection.Count() > 0);
        }
        [TestMethod]
        [DataRow(DisplayName = "Test Method E - Check If IoT Hub Description Exists Async")]
        public async Task TestMethod_E_CheckIfIotHubDescriptionExistsAsync()
        {
            bool iotHubDescriptionExists = await _iotHubResourceManager.CheckIfIotHubDescriptionExistsAsync(_iotHubResourceName);
            Assert.IsTrue(iotHubDescriptionExists);
        }
        [TestMethod]
        [DataRow(DisplayName = "Test Method F - Get IoT Hub Description Resource Async")]
        public async Task TestMethod_F_GetIotHubDescriptionResourceAsync()
        {
            IotHubDescriptionResource iotHubDescriptionResource = await _iotHubResourceManager.GetIotHubDescriptionResourceAsync(_iotHubResourceName);
            Assert.IsNotNull(iotHubDescriptionResource);
            Assert.AreEqual(_iotHubResourceName, iotHubDescriptionResource.Data.Name);
        }
        [TestMethod]
        [DataRow(DisplayName = "Test Method G - Get Role Assignment Data List Async")]
        public async Task TestMethod_G_GetRoleAssignmentListAsync()
        {
            IotHubDescriptionResource iotHubDescriptionResource = await _iotHubResourceManager.GetIotHubDescriptionResourceAsync(_iotHubResourceName);
            List<RoleAssignmentData> roleAssignmentDataList = await _authorizationResourceManager.GetRoleAssignmentDataListAsync(resourceId: iotHubDescriptionResource.Data.Id);
            Assert.IsTrue(roleAssignmentDataList.Count() > 0);
        }
        [TestMethod]
        [DataRow(DisplayName = "Test Method H - Get Role Assignment Async")]
        public async Task TestMethod_H_GetRoleAssignmentDataAsync()
        {
            IotHubDescriptionResource iotHubDescriptionResource = await _iotHubResourceManager.GetIotHubDescriptionResourceAsync(_iotHubResourceName);
            RoleAssignmentData roleAssignmentData = await _authorizationResourceManager.GetRoleAssignmentDataAsync(resourceId: iotHubDescriptionResource.Data.Id, name: "IoT Hub Data Contributor");
            Assert.IsNotNull(roleAssignmentData);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Test Method I - Delete Role Assignment Async")]
        public async Task TestMethod_I_DeleteRoleAssignmentAsync()
        {
            IotHubDescriptionResource iotHubDescriptionResource = await _iotHubResourceManager.GetIotHubDescriptionResourceAsync(_iotHubResourceName);
            bool roleAssignmentDataExists = await _authorizationResourceManager
                .DeleteRoleAssignmentResourceAsync(resourceId: iotHubDescriptionResource.Data.Id, name: "IoT Hub Data Contributor");
            Assert.IsFalse(roleAssignmentDataExists);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Test Method J - Delete Digital Twins Description Resource Async")]
        public async Task TestMethod_J_DeleteDigitalTwinsDescriptionResourceAsync()
        {
            bool resourceGroupCollectionExists = await _iotHubResourceManager.DeleteDigitalTwinsDescriptionResourceAsync(_iotHubResourceName);
            Assert.IsFalse(resourceGroupCollectionExists);
        }
    }
}