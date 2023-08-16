using Newtonsoft.Json.Linq;

namespace AzureNaming.Utilities.Tests
{
    [TestClass]
    public class TestArmFunctions
    {

        [TestMethod]
        public void Test_Unique_String_Simple()
        {
            string expected = "zcztcwvu6iyg6";

            var actual = ArmFunctions.UniqueString("tyeth");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Unique_String_Multiple_Seeds()
        {
            string expected = "fd63l2gacbtca";

            string resourceGroupId = "/subscriptions/3c7f956b-55ee-4686-9d30-da1eb631dc59/resourceGroups/rg-devopsshieldek010dev";

            var actual = ArmFunctions.UniqueString(new string[] { resourceGroupId });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Base64()
        {
            string expected = "b25lLCB0d28sIHRocmVl";

            string stringData = "one, two, three";

            var actual = ArmFunctions.Base64(new string[] { stringData });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Base64ToJson()
        {
            dynamic expectedJObject = new JObject();
            expectedJObject.one = "a";
            expectedJObject.two = "b";

            string jsonFormattedData = "{'one': 'a', 'two': 'b'}";

            var base64Object = ArmFunctions.Base64(new string[] { jsonFormattedData });

            var actualJObect = ArmFunctions.Base64ToJson(new string[] { base64Object });

            //find a better way to compare JObjects
            Assert.AreEqual(expectedJObject.ToString(), actualJObect.ToString());
        }

        [TestMethod]
        public void Test_Base64ToString()
        {
            string expected = "one, two, three";

            string stringData = "one, two, three";

            var base64String = ArmFunctions.Base64(new string[] { stringData });

            var actual = ArmFunctions.Base64ToString(new string[] { base64String });

            Assert.AreEqual(expected, actual);
        }
    }
}