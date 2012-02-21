using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal {

    public interface IDataEntity {

      IDataEntity CreateNew();
    
    }

    public interface IDataEntity<PrimaryKeyType> : IDataEntity {

        PrimaryKeyType Id { get; }

    }
}
