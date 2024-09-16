using Azure.ResourceManager.Resources;
using Derby.Subscription.ClassLibrary;

namespace Derby.Subscription.MSTest
{
    [TestClass]
    public class TestClass_A_SubscriptionResourceManager
    {
        private SubscriptionResourceManager _subscriptionResourceManager;
        private string _resourceGroupName;

        [TestInitialize]
        public void TestInitialize()
        {
            _subscriptionResourceManager = new SubscriptionResourceManager("5ecda7e7-179b-4603-85f3-302815e102fe");
            _resourceGroupName = "TestResourceGroup";
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Creating Resource Group Resource Async")]
        public async Task TestMethod_A_CreateResourceGroupResourceAsync()
        {
            ResourceGroupResource resourceGroupResource = await _subscriptionResourceManager.CreateResourceGroupResourceAsync(_resourceGroupName);
            Assert.IsNotNull(resourceGroupResource);
            Assert.AreEqual(_resourceGroupName, resourceGroupResource.Data.Name);
        }
        [TestMethod]
        [DataRow(DisplayName = "Checking If Resource Group Collection Exists Async")]
        public async Task TestMethod_B_CheckIfResourceGroupCollectionExistsAsync()
        {
            bool resourceGroupCollectionExists = await _subscriptionResourceManager.CheckIfResourceGroupCollectionExistsAsync(_resourceGroupName);
            Assert.IsTrue(resourceGroupCollectionExists);
        }
        [TestMethod]
        [DataRow(DisplayName = "Getting Resource Group Resource Async")]
        public async Task TestMethod_C_GetResourceGroupResourceAsync()
        {
            ResourceGroupResource resourceGroupResource = await _subscriptionResourceManager.GetResourceGroupResourceAsync(_resourceGroupName);
            Assert.IsNotNull(resourceGroupResource);
            Assert.AreEqual(_resourceGroupName, resourceGroupResource.Data.Name);
        }
        [TestMethod]
        [DataRow(DisplayName = "Getting Resource Group Collection Async")]
        public async Task TestMethod_D_GetResourceGroupCollectionAsync()
        {
            ResourceGroupCollection resourceGroupCollection = await _subscriptionResourceManager.GetResourceGroupCollectionAsync();
            Assert.IsNotNull(resourceGroupCollection);
            Assert.IsTrue(resourceGroupCollection.Count() > 0);
        }
        [Ignore]
        [TestMethod]
        [DataRow(DisplayName = "Deleting Resource Group Collection Async")]
        public async Task TestMethod_E_CreateResourceGroupResource()
        {
            bool resourceGroupCollectionExists = await _subscriptionResourceManager.DeleteResourceGroupResourceAsync(_resourceGroupName);
            Assert.IsFalse(resourceGroupCollectionExists);
        }
    }
}