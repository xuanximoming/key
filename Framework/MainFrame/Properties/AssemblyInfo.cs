using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Direcsoft")]
[assembly: AssemblyDescription("电子病历主窗口")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Direcsoft")]
[assembly: AssemblyProduct("Direcsoft")]
[assembly: AssemblyCopyright("Direcsoft Corporation© 2010-2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("599423b3-5628-40bb-95cf-b4c229524c79")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("3.6.1.3")]
[assembly: AssemblyFileVersion("6.5.2.3")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"LogApp.config", ConfigFileExtension = "config", Watch = true)]
[assembly: NeutralResourcesLanguageAttribute("zh-CN")]
