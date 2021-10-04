using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using BlogEngine.Core;

namespace BlogEngine.Testing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
        [TestMethod]
        public void TestDBContext()
        {
            using (var Context = new BlogEngineDBContext())
            {
                var sql = Context.Database.GenerateCreateScript();
                Context.Database.EnsureCreated();
            }
        }
    }
}
