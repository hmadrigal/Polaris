using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Extensibility;

namespace Polaris.Design {
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Interface,
        Inherited = true,
        AllowMultiple = false)]
    [MulticastAttributeUsage(MulticastTargets.Class | MulticastTargets.Interface, 
        AllowMultiple = false,
        TargetMemberAttributes = MulticastAttributes.NonAbstract, 
        Inheritance = MulticastInheritance.Strict,
        PersistMetaData = true)]
    [RequirePostSharp("Polaris.Design", "Polaris.Design.AddPagination")]
    public class PaginateAttribute : MulticastAttribute {
    }
}
