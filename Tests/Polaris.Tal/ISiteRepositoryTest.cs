﻿using Polaris.Bal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Polaris.Bal.DataRepositories;

namespace Polaris.Portal.Tests
{
    
    
    /// <summary>
    ///This is a test class for ISiteRepositoryTest and is intended
    ///to contain all ISiteRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("Polaris.Dal.dll")]
    public class ISiteRepositoryTest
    {
        internal virtual ISiteRepository CreateISiteRepository()
        {
            // TODO: Instantiate an appropriate concrete class.
            ISiteRepository target = RepositoryFactory.GetNewRepository<ISiteRepository>();
            return target;
        }

        /// <summary>
        ///A test for GetUsers
        ///</summary>
        [TestMethod()]
        public void GetUsersTest()
        {
            ISiteRepository target = CreateISiteRepository(); 
            IEnumerable<IUser> actual;
            actual = target.GetUsers();
            Assert.AreNotEqual(null, actual);
        }
    }
}