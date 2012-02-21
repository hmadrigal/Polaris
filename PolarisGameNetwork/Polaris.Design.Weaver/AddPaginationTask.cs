using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Extensibility;
using PostSharp.Extensibility.Tasks;
using PostSharp.CodeModel;
using System.Reflection;
using PostSharp.CodeWeaver;

namespace Polaris.Design {
    public class AddPaginationTask : Task, IAdviceProvider {
        #region IAdviceProvider Members

        public void ProvideAdvices(PostSharp.CodeWeaver.Weaver codeWeaver) {
            // Gets the dictionary of custom attributes.
            AnnotationRepositoryTask customAttributeDictionary =
                AnnotationRepositoryTask.GetTask(this.Project);

            // Requests an enumerator of all instances of our NonNullAttribute.
            IEnumerator<IAnnotationInstance> customAttributeEnumerator =
                customAttributeDictionary.GetAnnotationsOfType(typeof(PaginateAttribute), true);
            // Simulating a Set
            IDictionary<MethodDefDeclaration, MethodDefDeclaration> methods =
                new Dictionary<MethodDefDeclaration, MethodDefDeclaration>();
            // For each instance of our NonNullAttribute.
            while (customAttributeEnumerator.MoveNext()) {
                // Gets the parameters to which it applies.
                var typeDef = customAttributeEnumerator.Current.TargetElement
                                                as TypeDefDeclaration;

                if (typeDef != null) {
                    //if ((typeDef.Attributes & TypeAttributes.Interface) == TypeAttributes.Interface) {
                    //    codeWeaver.AddMethodLevelAdvice(new NonNullReturnAdvice(),
                    //                                    new Singleton<MethodDefDeclaration>(paramDef.Parent),
                    //                                    JoinPointKinds.AfterMethodBodySuccess,
                    //                                    null);
                    //} else {
                    //    if (!methods.ContainsKey(paramDef.Parent)) {
                    //        codeWeaver.AddMethodLevelAdvice(new NonNullParameterAdvice(paramDef),
                    //                                        new Singleton<MethodDefDeclaration>(paramDef.Parent),
                    //                                        JoinPointKinds.BeforeMethodBody,
                    //                                        null);
                    //        methods.Add(paramDef.Parent, paramDef.Parent);
                    //    }
                    //}
                }
            }
        }

        #endregion
    }
}
