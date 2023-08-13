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
    }
}