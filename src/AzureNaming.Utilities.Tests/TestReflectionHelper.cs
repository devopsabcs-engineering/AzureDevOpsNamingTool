namespace AzureNaming.Utilities.Tests
{
    [TestClass]
    public class TestReflectionHelper
    {
        [TestMethod]
        public void Test_Unique_String_Multiple_Seeds_Via_Reflection()
        {
            string expected = "fd63l2gacbtca";

            string resourceGroupId = "/subscriptions/3c7f956b-55ee-4686-9d30-da1eb631dc59/resourceGroups/rg-devopsshieldek010dev";

            var actual = ReflectionHelper.CallArmFunctionName("UniqueString", new object[] { new string[] { resourceGroupId } });

            Assert.AreEqual(expected, actual);
        }
    }
}
