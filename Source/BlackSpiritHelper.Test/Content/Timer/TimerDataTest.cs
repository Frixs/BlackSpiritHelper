using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackSpiritHelper.Test
{
    [TestClass]
    public class TimerDataTest : BaseTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            TestInitializeMethod();
        }

        [TestCleanup]
        public void TestClean()
        {
            TestCleanMethod();
        }

        /// <summary>
        /// TODO: Unit tests
        ///     : App.xaml.cs should have separate class to set initialization of IoC. We cannot access App class atm.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(true);
        }
    }
}
