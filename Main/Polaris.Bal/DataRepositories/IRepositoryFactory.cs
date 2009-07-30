using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal.DataRepositories {

    /// <summary>
    /// Represents the repository factory implemented by the data abstraction layer.
    /// </summary>
    public interface IRepositoryFactory {

        /// <summary>
        /// Returns a new instance of the specified repository type.
        /// </summary>
        /// <typeparam name="RepositoryType">Type of the repository to return.</typeparam>
        /// <returns>A new instance of the specified repository type.</returns>
        RepositoryType GetNewRepository<RepositoryType>() where RepositoryType : IRepository;

    }
}
