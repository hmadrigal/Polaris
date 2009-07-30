using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal {
    public interface IRepository {

        /// <summary>
        /// When implemented, returns a new instance of the specified data entity type.
        /// </summary>
        /// <typeparam name="DataEntityType">Implementation of IDataEntity associated with this repository.</typeparam>
        /// <returns>A new instance of the specified data entity type.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this repository is not able to return a new instance of the specified data entity type.
        /// </exception>
        DataEntityType CreateNew<DataEntityType>() where DataEntityType : IDataEntity;

        /// <summary>
        /// When implemented, persists all the changes performed on the data entities.
        /// </summary>
        void Save();

    }
}
