﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DrectSoft.Core.TimeLimitQC {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class QcResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal QcResource() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DrectSoft.Core.TimeLimitQC.QcResource", typeof(QcResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 &lt;qc&gt;
        ///	&lt;qct name=&quot;病人状态改变&quot; classtype=&quot;NetWorkStudio.Common.Eop.Inpatient,NetWorkStudio.Framework&quot;&gt;
        ///		&lt;pp id=&quot;.State&quot; name=&quot;病人状态&quot; /&gt;
        ///		&lt;pp id=&quot;.PreState&quot; name=&quot;病人原状态&quot; /&gt;
        ///	&lt;/qct&gt;
        ///	&lt;qct name=&quot;病历文件改变&quot; classtype=&quot;NetWorkStudio.Logic.Model.EmrModel,NetWorkStudio.Logic.Model&quot;&gt;
        ///		&lt;pp id=&quot;.OtherId&quot; name=&quot;标准编码&quot; /&gt;
        ///	&lt;/qct&gt;
        ///	&lt;qct name=&quot;医嘱改变&quot; classtype=&quot;NetWorkStudio.Common.Eop.Order,NetWorkStudio.Framework&quot;&gt;
        ///		&lt;pp id=&quot;.Content.OrderKind&quot; name=&quot;医嘱类型&quot; /&gt;
        ///		&lt;pp id=&quot;.Content.Item.Code&quot; name=&quot;医嘱项目代码&quot; /&gt;	
        ///	&lt;/qct&gt;
        ///&lt; [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string PropertiesDefine {
            get {
                return ResourceManager.GetString("PropertiesDefine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap 保存 {
            get {
                object obj = ResourceManager.GetObject("保存", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap 展开 {
            get {
                object obj = ResourceManager.GetObject("展开", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap 收缩 {
            get {
                object obj = ResourceManager.GetObject("收缩", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap 新增 {
            get {
                object obj = ResourceManager.GetObject("新增", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap 查询 {
            get {
                object obj = ResourceManager.GetObject("查询", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap 重置 {
            get {
                object obj = ResourceManager.GetObject("重置", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
