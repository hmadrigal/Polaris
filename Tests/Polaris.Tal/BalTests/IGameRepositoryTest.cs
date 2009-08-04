using Polaris.Bal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Polaris.Portal.Tests
{
    /// <summary>
    ///This is a test class for IGameRepositoryTest and is intended
    ///to contain all IGameRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("Polaris.Dal.dll")]
    public class IGameRepositoryTest
    {
        internal virtual IGameRepository CreateIGameRepository()
        {
            IGameRepository target = RepositoryFactory.GetNewRepository<IGameRepository>(); ;
            return target;
        }

        /// <summary>
        ///A test for GetGames
        ///</summary>
        [TestMethod()]
        public void GetGamesTest()
        {
            IGameRepository target = CreateIGameRepository(); 
            IEnumerable<IGame> actual;
            actual = target.GetGames();
            Assert.AreNotEqual(null, actual);
        }

        /// <summary>
        ///A test for GetFeaturedGames
        ///</summary>
        [TestMethod()]
        public void GetFeaturedGamesTest()
        {
            IGameRepository target = CreateIGameRepository();
            IEnumerable<IGame> actual;
            actual = target.GetFeaturedGames();
            Assert.AreNotEqual(null, actual);
        }
    }
}
