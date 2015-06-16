using System.Reflection;

// Version information for an assembly consists of the following four values:
//
//      Major Version : Manually incremented for major releases, such as adding many new features to the solution.
//      Minor Version : Manually incremented for minor releases, such as introducing small changes to existing features.
//      Build Number  : Typically incremented automatically as part of every build performed on the Build Server. This allows each build to be tracked and tested.
//      Revision      : Revision: Incremented for QFEs (a.k.a. “hotfixes” or patches) to builds released into the Production environment (PROD). This is set to zero for the initial release of any major/minor version of the solution. 
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]


// Note that the assembly version does not get incremented for every build
// to avoid problems with assembly binding (or requiring a policy or
// <bindingRedirect> in the config file).
//
// The AssemblyFileVersionAttribute is incremented with every build in order
// to distinguish one build from another. AssemblyFileVersion is specified
// in AssemblyVersionInfo.cs so that it can be easily incremented by the
// automated build process.
[assembly: AssemblyVersion("1.0.0.0")]

// By default, the "Product version" shown in the file properties window is
// the same as the value specified for AssemblyFileVersionAttribute.
// Set AssemblyInformationalVersionAttribute to be the same as
// AssemblyVersionAttribute so that the "Product version" in the file
// properties window matches the version displayed in the GAC shell extension.
[assembly: AssemblyInformationalVersion("1.0.0.0")] // a.k.a. "Product version"