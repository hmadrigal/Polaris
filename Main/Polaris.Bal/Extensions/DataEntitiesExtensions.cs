using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal.Extensions {
    public static class DataEntitiesExtensions {

        private static Dictionary<SiteSection, Dictionary<Type, Int32>> PageSizeDictionary { get; set; }

        static DataEntitiesExtensions() {
            PageSizeDictionary = new Dictionary<SiteSection, Dictionary<Type, int>>();
        }

        public static void RegisterPageSize<EntityType>(this SiteSection siteSection, Int32 pageSize) where EntityType : IDataEntity {
            var siteSectionPageSizeDictionary = siteSection.GetSiteSectionPageSizeDictionary();
            var entityType = typeof(EntityType);
            if (siteSectionPageSizeDictionary.ContainsKey(entityType)) {
                siteSectionPageSizeDictionary[entityType] = pageSize;
            } else {
                siteSectionPageSizeDictionary.Add(entityType, pageSize);
            }
        }

        public static Int32 GetPageSize<EntityType>(this SiteSection siteSection) {
            var siteSectionPageSizeDictionary = siteSection.GetSiteSectionPageSizeDictionary();
            var entityType = typeof(EntityType);
            if (siteSectionPageSizeDictionary.ContainsKey(entityType)) {
                return siteSectionPageSizeDictionary[entityType];
            } else {
                throw new InvalidOperationException("A page size is not registered with this site section for the specified entity type");
            }
        }

        private static Dictionary<Type, Int32> GetSiteSectionPageSizeDictionary(this SiteSection siteSection) {
            if (!PageSizeDictionary.ContainsKey(siteSection)) {
                PageSizeDictionary.Add(siteSection, new Dictionary<Type, Int32>());
            }
            return PageSizeDictionary[siteSection];
        }

    }
}
