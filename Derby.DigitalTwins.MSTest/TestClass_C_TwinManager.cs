﻿using Azure;
using Azure.DigitalTwins.Core;
using Derby.DigitalTwins.ClassLibrary;
using System.Text;
using System.Text.Json;

namespace Derby.DigitalTwins.MSTest
{
    [TestClass]
    public class TestClass_C_TwinManager
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
        [DataRow("dtmi:dtdl:context:primitiveModel;1", "Primitive", DisplayName = "Creating Primitive Basic Digital Twin Model")]
        [DataRow("dtmi:dtdl:context:complexModel;1", "Complex", DisplayName = "Creating Complex Basic Digital Twin Model")]
        [DataRow("dtmi:dtdl:context:Factory;1", "Factory", DisplayName = "Creating Factory Basic Digital Twin Model")]
        [DataRow("dtmi:dtdl:context:Robot;1", "Robot1", DisplayName = "Creating Robot1 Basic Digital Twin Model")]
        [DataRow("dtmi:dtdl:context:Robot;1", "Robot2", DisplayName = "Creating Robot2 Basic Digital Twin Model")]
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
                    Assert.AreEqual(primitiveBasicDigitalTwin2.Metadata.ModelId, modelId);
                    Assert.AreEqual(primitiveBasicDigitalTwin2.Id, twinId);

                    Dictionary<string, object> primitiveContentDictionary = await _twinManager.GetContentDictionaryAsync(twinId);
                    Assert.AreEqual(primitiveContentDictionary.Count(), 10);
                    primitiveContentDictionary.TryGetValue("booleanProperty", out object? primitiveBooleanProperty);
                    primitiveContentDictionary.TryGetValue("dateProperty", out object? primitiveDateProperty);
                    primitiveContentDictionary.TryGetValue("dateTimeProperty", out object? primitiveDateTimeProperty);
                    primitiveContentDictionary.TryGetValue("doubleProperty", out object? primitiveDoubleProperty);
                    primitiveContentDictionary.TryGetValue("durationProperty", out object? primitiveDurationProperty);
                    primitiveContentDictionary.TryGetValue("floatProperty", out object? primitiveFloatProperty);
                    primitiveContentDictionary.TryGetValue("integerProperty", out object? primitiveIntegerProperty);
                    primitiveContentDictionary.TryGetValue("longProperty", out object? primitiveLongProperty);
                    primitiveContentDictionary.TryGetValue("stringProperty", out object? primitiveStringProperty);
                    primitiveContentDictionary.TryGetValue("timeProperty", out object? primitiveTimeProperty);

                    if (primitiveBooleanProperty is not null)
                    {
                        string? propertyString = primitiveBooleanProperty.ToString();
                        if(propertyString is not null) Assert.AreEqual(bool.Parse(propertyString), booleanValue);
                        else Assert.Fail();
                    }
                    if (primitiveDateProperty is not null)
                    {
                        string? propertyString = primitiveDateProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(propertyString, dateValue);
                        else Assert.Fail();
                    }
                    if (primitiveDateTimeProperty is not null)
                    {
                        string? propertyString = primitiveDateTimeProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(propertyString, dateTimeValue);
                        else Assert.Fail();
                    }
                    if (primitiveDoubleProperty is not null)
                    {
                        string? propertyString = primitiveDoubleProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(double.Parse(propertyString), doubleValue);
                        else Assert.Fail();
                    }
                    if (primitiveDurationProperty is not null)
                    {
                        string? propertyString = primitiveDurationProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(propertyString, durationValue);
                        else Assert.Fail();
                    }
                    if (primitiveFloatProperty is not null)
                    {
                        string? propertyString = primitiveFloatProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(float.Parse(propertyString), floatValue);
                        else Assert.Fail();
                    }
                    if (primitiveIntegerProperty is not null)
                    {
                        string? propertyString = primitiveIntegerProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(int.Parse(propertyString), integerValue);
                        else Assert.Fail();
                    }
                    if (primitiveLongProperty is not null)
                    {
                        string? propertyString = primitiveLongProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(long.Parse(propertyString), longValue);
                        else Assert.Fail();
                    }
                    if (primitiveStringProperty is not null)
                    {
                        string? propertyString = primitiveStringProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(propertyString, stringValue);
                        else Assert.Fail();
                    }
                    if (primitiveTimeProperty is not null)
                    {
                        string? propertyString = primitiveTimeProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(propertyString, timeValue);
                        else Assert.Fail();
                    }
                    break;
                case "dtmi:dtdl:context:complexModel;1":

                    int enumValue = 2;
                    object objectValue = new { Field1 = new Random().NextDouble(), Field2 = new Random().NextDouble() };
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
                    Assert.AreEqual(complexBasicDigitalTwin2.Metadata.ModelId, modelId);
                    Assert.AreEqual(complexBasicDigitalTwin2.Id, twinId);

                    Dictionary<string, object> complexContentDictionary = await _twinManager.GetContentDictionaryAsync(twinId);
                    Assert.AreEqual(complexContentDictionary.Count(), 3);
                    complexContentDictionary.TryGetValue("EnumProperty", out object? primitiveEnumProperty);
                    complexContentDictionary.TryGetValue("ObjectProperty", out object? primitiveObjectProperty);
                    complexContentDictionary.TryGetValue("MapProperty", out object? primitiveMapProperty);

                    if (primitiveEnumProperty is not null)
                    {
                        string? propertyString = primitiveEnumProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(int.Parse(propertyString), enumValue);
                        else Assert.Fail();
                    }
                    if (primitiveObjectProperty is not null)
                    {
                        Assert.AreEqual(primitiveObjectProperty.ToString(), JsonSerializer.Serialize(objectValue));
                    }
                    if (primitiveMapProperty is not null)
                    {
                        Assert.AreEqual(primitiveMapProperty.ToString(), JsonSerializer.Serialize(mapValue));
                    }
                    break;
                case "dtmi:dtdl:context:Factory;1":
                    BasicDigitalTwin factoryBasicDigitalTwin = new BasicDigitalTwin
                    {
                        Id = twinId,
                        Metadata = { ModelId = modelId }
                    };
                    BasicDigitalTwin factoryBasicDigitalTwin2 = await _twinManager.CreateBasicDigitalTwinAsync(twinId, factoryBasicDigitalTwin);
                    Assert.AreEqual(factoryBasicDigitalTwin2.Metadata.ModelId, modelId);
                    Assert.AreEqual(factoryBasicDigitalTwin2.Id, twinId);
                    Dictionary<string, object> factoryContentDictionary = await _twinManager.GetContentDictionaryAsync(twinId);
                    Assert.AreEqual(factoryContentDictionary.Count(), 0);
                    break;
                case "dtmi:dtdl:context:Robot;1":
                    double temperatureValue = new Random().NextDouble();
                    BasicDigitalTwin robotBasicDigitalTwin = new BasicDigitalTwin
                    {
                        Id = twinId,
                        Metadata = { ModelId = modelId },
                        Contents =
                            {
                                { "temperature", temperatureValue },
                            }
                    };
                    BasicDigitalTwin robotBasicDigitalTwin2 = await _twinManager.CreateBasicDigitalTwinAsync(twinId, robotBasicDigitalTwin);
                    Assert.AreEqual(robotBasicDigitalTwin2.Metadata.ModelId, modelId);
                    Assert.AreEqual(robotBasicDigitalTwin2.Id, twinId);
                    Dictionary<string, object> robotContentDictionary = await _twinManager.GetContentDictionaryAsync(twinId);
                    Assert.AreEqual(robotContentDictionary.Count(), 1);
                    robotContentDictionary.TryGetValue("temperature", out object? temperatureDoubleProperty);
                    if (temperatureDoubleProperty is not null)
                    {
                        string? propertyString = temperatureDoubleProperty.ToString();
                        if (propertyString is not null) Assert.AreEqual(double.Parse(propertyString), temperatureValue);
                        else Assert.Fail();
                    }
                    break;
            }
        }
        [TestMethod]
        [DataRow("Primitive", DisplayName = "Getting Primitive Basic Digital Twin Model")]
        [DataRow("Complex", DisplayName = "Getting Complex Basic Digital Twin Model")]
        [DataRow("Factory", DisplayName = "Getting Factory Basic Digital Twin Model")]
        [DataRow("Robot1", DisplayName = "Getting Robot1 Basic Digital Twin Model")]
        [DataRow("Robot2", DisplayName = "Getting Robot2 Basic Digital Twin Model")]
        public async Task TestMethod_B_GetBasicDigitalTwinAsync(string twinId)
        {
            BasicDigitalTwin basicDigitalTwin = await _twinManager.GetBasicDigitalTwinAsync(twinId);
            Assert.AreEqual(basicDigitalTwin.Id, twinId);
        }
        [TestMethod]
        [DataRow("Primitive", 10, DisplayName = "Getting Primitive Content Dictionary")]
        [DataRow("Complex", 3, DisplayName = "Getting Complex Content Dictionary")]
        [DataRow("Factory", 0, DisplayName = "Getting Factory Content Dictionary")]
        [DataRow("Robot1", 1, DisplayName = "Getting Robot1 Content Dictionary")]
        [DataRow("Robot2", 1, DisplayName = "Getting Robot2 Content Dictionary")]
        public async Task TestMethod_C_GetContentDictionaryAsync(string twinId, int keyCount)
        {
            Dictionary<string, object> contentDictionary = await _twinManager.GetContentDictionaryAsync(twinId);
            Assert.AreEqual(contentDictionary.Count(), keyCount);
        }
        [TestMethod]
        [DataRow("Factory", "Robot1", "has_robot", DisplayName = "Creating Factory to Robot1 Basic Relationship")]
        [DataRow("Factory", "Robot2", "has_robot", DisplayName = "Creating Factory to Robot2 Basic Relationship")]
        public async Task TestMethod_D_CreateBasicRelationshipAsync(string sourceId, string targetId, string name)
        {
            BasicRelationship basicRelationship = await _twinManager.CreateBasicRelationshipAsync(sourceId, targetId, name);
            Assert.AreEqual(basicRelationship.Name, name);
            Assert.AreEqual(basicRelationship.SourceId, sourceId);
            Assert.AreEqual(basicRelationship.TargetId, targetId);
        }
        [TestMethod]
        [DataRow("Factory", "Robot1", "has_robot", DisplayName = "Getting Factory to Robot1 Basic Relationship")]
        [DataRow("Factory", "Robot2", "has_robot", DisplayName = "Getting Factory to Robot2 Basic Relationship")]
        public async Task TestMethod_E_GetBasicRelationshipAsync(string sourceId, string targetId, string name)
        {
            string relationshipId = $"{sourceId}-{name}-{targetId}";
            BasicRelationship basicRelationship = await _twinManager.GetBasicRelationshipAsync(sourceId, relationshipId);
            Assert.AreEqual(basicRelationship.Name, name);
            Assert.AreEqual(basicRelationship.SourceId, sourceId);
            Assert.AreEqual(basicRelationship.TargetId, targetId);
        }
        [TestMethod]
        [DataRow("Robot1", DisplayName = "Getting Robot1 Incoming Relationship List")]
        [DataRow("Robot2", DisplayName = "Getting Robot2 Incoming Relationship List")]
        public async Task TestMethod_F_GetIncomingRelationshipListAsync(string targetId)
        {
            List<IncomingRelationship> incomingRelationshipList = await _twinManager.GetIncomingRelationshipListAsync(targetId);
            Assert.AreEqual(incomingRelationshipList.Count(), 1);
        }
        [TestMethod]
        [DataRow("Factory", "Robot1", "has_robot", DisplayName = "Deleting Factory to Robot1 Relationship")]
        [DataRow("Factory", "Robot2", "has_robot", DisplayName = "Deleting Factory to Robot2 Relationship")]
        public async Task TestMethod_G_GetBasicRelationshipAsync(string sourceId, string targetId, string name)
        {
            string relationshipId = $"{sourceId}-{name}-{targetId}";
            Response response = await _twinManager.DeleteRelationshipAsync(sourceId, relationshipId);
            Assert.AreEqual(response.Status, 204);
        }
        [TestMethod]
        [DataRow("Factory", DisplayName = "Deleting Factory Digital Twin")]
        [DataRow("Robot1", DisplayName = "Deleting Robot1 Digital Twin")]
        [DataRow("Robot2", DisplayName = "Deleting Robot2 Digital Twin")]
        [DataRow("Primitive", DisplayName = "Deleting Primitive Digital Twin")]
        [DataRow("Complex", DisplayName = "Deleting Complex Digital Twin")]
        public async Task TestMethod_H_DeleteDigitalTwinAsync(string twinId)
        {
            Response response = await _twinManager.DeleteDigitalTwinAsync(twinId);
            Assert.AreEqual(response.Status, 204);
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
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789αβγδεζηθικλμνξοπρστυφχψωΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ";
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