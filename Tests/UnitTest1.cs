using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serify;

namespace Tests
{
    [TestClass]
    public class InstallModule
    {
        [TestMethod]
        public void InstallGet()
        {
            RunCommand(["add","get"]);
        }
    }
}