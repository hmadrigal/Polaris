using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal.DataRepositories
{
    public static class EntityFactory
    {
        private static IEntityFactory Factory { get; set; }

        static EntityFactory()
        {
            Factory = CreateNewEntityFactory();
        }

        public static EntityType GetNewEntity<EntityType>() where EntityType : IDataEntity
        {
            return Factory.GetNewEntity<EntityType>();
        }


        private static IEntityFactory CreateNewEntityFactory()
        {
            return Polaris.Bal.Helpers.Settings.Plugin.CreateNewInstanceOf<IEntityFactory>(AppSettings.DalAssemblyName, AppSettings.DalEntityFactoryName);
        }
    }
}
