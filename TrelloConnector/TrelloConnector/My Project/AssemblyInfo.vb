Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

<Assembly: AssemblyTitle("KornSW - Trello Connector")>
<Assembly: AssemblyDescription("Trello Connector")>
<Assembly: AssemblyProduct("Trello Connector")>
<Assembly: AssemblyTrademark("KornSW")>
<Assembly: AssemblyCompany("KornSW")>
<Assembly: AssemblyCopyright("KornSW")>

<Assembly: CLSCompliant(True)>
<Assembly: ComVisible(False)>
<Assembly: Guid("e37c0ac5-c34c-4f81-b736-c018787930ee")>

<Assembly: AssemblyVersion(Major + "." + Minor + "." + Fix + "." + BuildNumber)>
<Assembly: AssemblyInformationalVersion(Major + "." + Minor + "." + Fix + "-" + BuildType)>

Public Module SemanticVersion

  'increment this on breaking change:
  Public Const Major = "4"

  'increment this on new feature (w/o breaking change):
  Public Const Minor = "18"

  'increment this on internal fix (w/o breaking change):
  Public Const Fix = "1"

  'AND DONT FORGET TO UPDATE THE VERSION-INFO OF THE *.nuspec FILE!!!
#Region "..."

  'dont touch this, beacuse it will be replaced ONLY by the build process!!!

  Public Const BuildNumber = "*"
  Public Const BuildType = "LOCALBUILD"

#End Region
End Module