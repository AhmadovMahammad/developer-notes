internal class Program
{
    private static void Main(string[] args)
    {
        /* What is an Assembly?

        In .NET, an assembly is a compiled code library used by the Common Language Runtime (CLR).
        It can contain code (Intermediate Language, or IL), metadata, resources (e.g., images or strings),
        and manifests (descriptions about the assembly).

        Assemblies are the building blocks of .NET applications and are essential for code deployment, reuse, and versioning.

        1. Assemblies are the building blocks
        in .NET. Every .NET application is composed of one or more assemblies, which are .dll (Dynamic Link Library) or .exe (Executable) files.
        These files encapsulate compiled code, metadata, and resources, making them self-contained and modular.

        2. Code Deployment
        An assembly bundles code and related data into one file, making it easier to distribute and deploy your application.
        Since each assembly includes a version number in its manifest,
        you can manage and update parts of your application independently without affecting the whole system.

        --- What Does an Assembly Contain?

        1. Assembly Manifest
        Describes the assembly to the CLR.
        Provides metadata such as the assembly's name, version, and list of referenced assemblies.

        2. Application Manifest
        Describes the assembly to the operating system.
        Specifies deployment details, such as whether the application requires administrative privileges.

        3. Compiled Types
        Contains the actual compiled IL (Intermediate Language) code and
        metadata for the types (classes, structs, interfaces, etc.) defined in the assembly.

        4. Resources
        Embeds additional data such as images, localized text, or configuration files.


        --- The Assembly Manifest
        The assembly manifest serves two purposes:

        1_ Describe the Assembly
        It provides metadata to the CLR, enabling it to understand the assembly’s identity and dependencies.

        2_ Act as a Directory
        It lists all the resources, types, and modules in the assembly, making it self-describing.
        This means that, a consumer can discover all of an assembly’s data, types, and functions without needing additional files

        --- Information in an Assembly Manifest

        1_ Functional Data (Mandatory)
        Simple Name: The name of the assembly.
        Version Number: Specifies the assembly version (e.g., 1.0.0.0).
        Public Key and Hash: If the assembly is strongly named, it includes a public key and hash for verification.
        Referenced Assemblies: Lists other assemblies the current assembly depends on.
        Defined Types: Lists the types (e.g., classes, enums) included in the assembly.
        Culture: Specifies the target culture if the assembly is a satellite assembly for localization.

        2_ Informational Data (Optional)
        Title and Description: Metadata such as AssemblyTitle and AssemblyDescription.
        Company and Copyright: Information like AssemblyCompany and AssemblyCopyright.
        Display Version: A user-friendly version number (AssemblyInformationalVersion).
        Custom Attributes: Any additional data provided using custom attributes.

        code examples:
        Assembly assembly = Assembly.GetExecutingAssembly();

        // Display basic information about the assembly
        Console.WriteLine("Assembly Name: " + assembly.GetName().Name);
        Console.WriteLine("Version: " + assembly.GetName().Version);
        Console.WriteLine("Culture: " + (assembly.GetName().CultureInfo?.Name ?? "neutral"));

        // List referenced assemblies
        Console.WriteLine("\nReferenced Assemblies:");
        foreach (var reference in assembly.GetReferencedAssemblies())
        {
            Console.WriteLine("- " + reference.FullName);
        }

        // List custom attributes
        Console.WriteLine("\nCustom Attributes:");
        foreach (var attribute in assembly.GetCustomAttributes())
        {
            Console.WriteLine("- " + attribute.GetType().Name);
        }

        */

        /* The Assembly Class

        The Assembly class in System.Reflection is a gateway to accessing assembly metadata at runtime.
        There are a number of ways to obtain an assembly object:

        1. Using a Type’s Assembly Property

        The simplest way to obtain an Assembly object is through a type in the assembly. For example:
        Assembly assembly = typeof(Program).Assembly;

        2. Using Assembly Class Static Methods
        The Assembly class provides static methods to retrieve the current or related assemblies:

        2.1. Assembly.GetExecutingAssembly()
        Retrieves the assembly of the type defining the currently executing function:

        Assembly assembly2 = Assembly.GetExecutingAssembly();

        2.2. Assembly.GetCallingAssembly()
        Retrieves the assembly of the method that called the currently executing function:

        Assembly assembly3 = Assembly.GetCallingAssembly();

        2.3. Assembly.GetEntryAssembly()
        Retrieves the assembly that contains the application’s original entry point (e.g., Main method):

        Assembly? assembly4 = Assembly.GetEntryAssembly();

        ----- Using an Assembly Object
        Once you have an Assembly object, you can use its members to query metadata or reflect on the types and resources it contains.

        -- Metadata and Attributes
        1. FullName: Gets the fully qualified name of the assembly, including its name, version, culture, and public key token:

        Console.WriteLine(assembly.FullName);

        OUTPUT:
        Assemblies_ch17, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
        name             version          culture          public key token

        2. GetName: Returns an AssemblyName object containing detailed metadata about the assembly:

        AssemblyName assemblyName = assembly.GetName();
        Console.WriteLine(assemblyName.Name); // Name of the assembly
        Console.WriteLine(assemblyName.Version); // Version number

        -- Location and Modules
        1. Location: Retrieves the file path of the loaded assembly:

        Assembly assembly = typeof(Program).Assembly;
        Console.WriteLine(assembly.Location);

        OUTPUT: 
        C:\Users\mahammada\source\repos\Studying\C#_fromScratch\CoreProgramming\Assemblies_ch17\bin\Debug\net6.0\Assemblies_ch17.dll

        */
    }
}