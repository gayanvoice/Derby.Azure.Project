using Azure.DigitalTwins.Core;
using Derby.DigitalTwins.ClassLibrary;
using System.Text;

namespace Derby.DigitalTwins.MSTest
{
    [TestClass]
    public class TestClass_B_TwinManager
    {
        private TwinManager _twinManager;
        public string _digitalTwinsResourceName;
        [TestInitialize]
        public void TestInitialize()
        {
            _twinManager = new TwinManager();
            _digitalTwinsResourceName = "TestAzureDigitalTwinsInstance";
        }
        [TestMethod]
        [DataRow("dtmi:dtdl:context:primitiveModel;1", "PrimitiveModel", DisplayName = "Creating Primitive Twin Model")]
        [DataRow("dtmi:dtdl:context:complexModel;1", "ComplexModel", DisplayName = "Creating Complex Twin Model")]
        [DataRow("dtmi:dtdl:context:Factory;1", "Factory", DisplayName = "Creating Factory Twin Model")]
        [DataRow("dtmi:dtdl:context:Robot;1", "Robot", DisplayName = "Creating Robot Twin Model")]
        public async Task TestMethod_A_CreateBasicDigitalTwinAsync(string modelId, string twinId)
        {
            switch (modelId)
            {
                case "dtmi:dtdl:context:primitiveModel;1":
                    bool booleanValue = true;
                    string dateValue = DateTime.Now.ToString("yyyy-MM-dd");
                    string dateTimeValue = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
                    double doubleValue = new Random().NextDouble();
                    string durationValue = GenerateRandomISO8601Duration();
                    float floatValue = (float)new Random().NextDouble();
                    int integerValue = new Random().Next();
                    long longValue = (long) new Random().Next();
                    string stringValue = GenerateRandomUTF8String(256);
                    string timeValue = DateTime.Now.ToString("hh:mm:ss");
                    BasicDigitalTwin primitiveBasicDigitalTwin = new BasicDigitalTwin
                    {
                        Id = twinId,
                        Metadata = { ModelId = modelId },
                        Contents =
                            {
                                { "booleanProperty", booleanValue },
                                { "dateProperty", dateValue },
                                { "dateTimeProperty", dateTimeValue },
                                { "doubleProperty",  doubleValue },
                                { "durationProperty", durationValue },
                                { "floatProperty",  floatValue },
                                { "integerProperty",  integerValue },
                                { "longProperty",  longValue },
                                { "stringProperty",  stringValue },
                                { "timeProperty",  timeValue },
                            },
                    };
                    BasicDigitalTwin primitiveBasicDigitalTwin2 = await _twinManager.CreateBasicDigitalTwinAsync(twinId, primitiveBasicDigitalTwin);
                    Assert.AreEqual(primitiveBasicDigitalTwin2.Id, twinId);
                    break;
                case "dtmi:dtdl:context:complexModel;1":

                    int enumValue = 2;
                    object objectValue = new { Field1 = 12.5, Field2 = 20.5 };
                    Dictionary<string, string> mapValue = new Dictionary<string, string>
                    {
                        { "Priority", "High" }
                    };
                    BasicDigitalTwin complexBasicDigitalTwin = new BasicDigitalTwin
                    {
                        Id = twinId,
                        Metadata = { ModelId = modelId },
                        Contents =
                        {
                            { "EnumProperty", enumValue },
                            { "ObjectProperty", objectValue },
                            { "MapProperty", mapValue },
                        },
                    };
                    BasicDigitalTwin complexBasicDigitalTwin2 = await _twinManager.CreateBasicDigitalTwinAsync(twinId, complexBasicDigitalTwin);
                    Assert.AreEqual(complexBasicDigitalTwin2.Id, twinId);
                    break;
                case "dtmi:dtdl:context:Factory;1":
                    BasicDigitalTwin factoryBasicDigitalTwin = new BasicDigitalTwin
                    {
                        Id = twinId,
                        Metadata = { ModelId = modelId }
                    };
                    BasicDigitalTwin factoryBasicDigitalTwin2 = await _twinManager.CreateBasicDigitalTwinAsync(twinId, factoryBasicDigitalTwin);
                    Assert.AreEqual(factoryBasicDigitalTwin2.Id, twinId);
                    break;
                case "dtmi:dtdl:context:Robot;1":
                    BasicDigitalTwin robotBasicDigitalTwin = new BasicDigitalTwin
                    {
                        Id = twinId,
                        Metadata = { ModelId = modelId },
                        Contents =
                            {
                                { "temperature", 50.05 },
                            }
                    };
                    BasicDigitalTwin robotBasicDigitalTwin2 = await _twinManager.CreateBasicDigitalTwinAsync(twinId, robotBasicDigitalTwin);
                    Assert.AreEqual(robotBasicDigitalTwin2.Id, twinId);
                    break;
            }
        }
        private string GenerateRandomISO8601Duration()
        {
            Random random = new Random();
            int years = random.Next(0, 5);
            int months = random.Next(0, 12);
            int days = random.Next(0, 31);
            int hours = random.Next(0, 24);
            int minutes = random.Next(0, 60);
            int seconds = random.Next(0, 60);
            return $"P{years}Y{months}M{days}DT{hours}H{minutes}M{seconds}S";
        }
        private static string GenerateRandomUTF8String(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" +
                               "αβγδεζηθικλμνξοπρστυφχψω" +
                               "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                               "𠀀𠀁𠀂𠀃𠀄𠀅𠀆𠀇𠀈𠀉𠀊𠀋𠀌𠀍𠀎𠀏" +
                               "🅰️🅱️🅾️🆎🆑🆓🆔🆗🆘🆙🆚🆛🆜🆝🆞💻🎉🔥🌍🚀";
            StringBuilder stringBuilder = new StringBuilder(length);
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }
            return stringBuilder.ToString();
        }
    }
}