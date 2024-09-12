using Derby.DigitalTwins.ClassLibrary;

namespace Derby.DigitalTwins.MSTest
{
    [TestClass]
    public class ResourceManager_UnitTest
    {
        [TestMethod]
        public async Task CreateNewResource()
        {
            ResourceManager resourceManager = new ResourceManager("5ecda7e7-179b-4603-85f3-302815e102fe");
            await resourceManager.CheckIfResourceGroupCollectionExistsAsync("TestResourceGroup3");
            await resourceManager.CreateResourceGroupResource("TestResourceGroup3");
            await resourceManager.CheckIfResourceGroupCollectionExistsAsync("TestResourceGroup3");
        }
    }
}