using BlackSpiritHelper;
using BlackSpiritHelper.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackSpiritHelper.Test
{
    /// <summary>
    /// TODO: Create UnitTest structure.
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        private ScheduleDataViewModel mViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            // TODO: Create separate method in base project to hold setup methods.
            // TODO: To test command methods, we need to make them public?
            //((App)Application.Current).ApplicationModuleSetup();
            mViewModel = new ScheduleDataViewModel();
        }

        [TestCleanup]
        public void TestClean()
        {
            mViewModel = null;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var result = mViewModel.IsItemAlreadyDefined("BDO-RU");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            mViewModel.Setup();
            var result = mViewModel.IsItemAlreadyDefined("BDO-RU");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMethod3()
        {
            mViewModel.AddItem("Blah", "000001", false);
            var result = mViewModel.IsItemAlreadyDefined("Blah");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMethod4()
        {
            mViewModel.AddItem(" Blah", "000001", false);
            var result = mViewModel.IsItemAlreadyDefined("Blah");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var result = mViewModel.IsItemAlreadyDefined("Blah");
            Assert.IsFalse(result);
        }
    }
}
