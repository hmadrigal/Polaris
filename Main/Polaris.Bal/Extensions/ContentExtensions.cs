using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal.Helpers.Filters;

namespace Polaris.Bal.Extensions
{
    public static class ContentExtensions
    {
        public static ContentDefinition GetActiveContentType(this IDictionary<String, ContentDefinition> contentTypes, string contentTypeName, ContentDefinition defaultContentType)
        {
            ContentDefinition contentType = defaultContentType;
            if (!String.IsNullOrEmpty(contentTypeName))
            {
                if (contentTypes.ContainsKey(contentTypeName))
                {
                    contentType = contentTypes[contentTypeName];
                }
            }
            return contentType;
        }
    }
}
