//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "12.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Student {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Student() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Student", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Τμήμα:.
        /// </summary>
        internal static string Department {
            get {
                return ResourceManager.GetString("Department", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ον/μο (ελληνικά):.
        /// </summary>
        internal static string GreekName {
            get {
                return ResourceManager.GetString("GreekName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ίδρυμα:.
        /// </summary>
        internal static string Institution {
            get {
                return ResourceManager.GetString("Institution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ον/μο (λατινικά):.
        /// </summary>
        internal static string LatinName {
            get {
                return ResourceManager.GetString("LatinName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Σχολή:.
        /// </summary>
        internal static string School {
            get {
                return ResourceManager.GetString("School", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Στοιχεία Φοιτητή.
        /// </summary>
        internal static string StudentDetails {
            get {
                return ResourceManager.GetString("StudentDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Αρ. Μητρώου:.
        /// </summary>
        internal static string StudentNumber {
            get {
                return ResourceManager.GetString("StudentNumber", resourceCulture);
            }
        }
    }
}