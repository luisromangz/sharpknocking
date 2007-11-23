// TargetExtensionFactory.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by mangelp<@>gmail[*]com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
//

using System;
using System.IO;
using System.Reflection;

using Developer.Common.Types;

using IptablesNet.Core;

namespace IptablesNet.Core.Extensions.ExtendedTarget
{
	/// <summary>
	/// Factory to build objects derived from TargetExtensionHandler
	/// </summary>
	public class TargetExtensionFactory
	{
	    private static TargetExtensionFactory factory
	                       = new TargetExtensionFactory();

        public static TargetExtensionFactory Instance
        {
           get { return TargetExtensionFactory.factory;}    
        }
	    
	    private static string assemblySearchPath;
	    
	    public static string AssemblySearchPath
	    {
	        get
	        {
                return assemblySearchPath;
	        }
	    }
	    
	    /// <summary>
	    /// Default private constructor.
	    /// </summary>
       	private TargetExtensionFactory()
       	{
       	    //Set the search path as a subdirectory where this assembly is
       	    //located
       	    Assembly asm = Assembly.GetExecutingAssembly();
			//The search path is the directory where this assembly is
       	    assemblySearchPath =
				Path.GetDirectoryName(asm.Location);
       	}
       	
       	/// <summmary>
       	/// Gets the type that implements the extension.
       	/// </summary>
       	/// <returns>
       	/// A reference to the type if it can find the class that implements
       	/// the extension or null otherwise.
       	/// </returns>
       	public static Type GetExtensionType(TargetExtensions extType)
       	{
       	    return TargetExtensionFactory.GetExtensionType(extType.ToString());
       	}
       	
       	/// <summary>
       	/// Gets the type that implements the extension.
       	/// </summary>
       	/// <param name="typeName>Name of the extension. This must be the same
       	/// name that is used to load the extension with the -m option. It is
       	/// case-insensitive for the current implementation of this method.
       	/// </param>
       	/// <returns>
       	/// A reference to the type if it can find the class that implements
       	/// the extension or null otherwise.
       	/// </returns>
       	public static Type GetExtensionType(string typeName)
       	{
            if(String.IsNullOrEmpty(typeName))
                throw new ArgumentException("The name of the type can't be null"+
                                            " or empty", "typeName");
				
      	    //The full name of each class must follow this convention:
       	    // IptablesNet.Extensions.Match.{EnumName}TargetExtension
       	    //So here we can build the name automatically
       	    string fullName = "IptablesNet.Extensions.Targets."+typeName+"TargetExtension";
			string customAsmName = assemblySearchPath+Path.DirectorySeparatorChar
					+"IptablesNet.Extensions."+typeName+"Targets.dll";
			string commonAsmName = assemblySearchPath+Path.DirectorySeparatorChar+
					"IptablesNet.Extensions.Targets.dll";
			return AssemblyUtil.TryLoadWithType(fullName, customAsmName, commonAsmName); 
       	}
       	
       	/// <summary>
       	/// Returns an instance of an extension if the name matches any known
       	/// extension.
       	/// </summary>
       	/// <remarks>
       	/// The name is used to search the assembly for a type that implements
       	/// the extension.
       	/// </remarks>
       	public static TargetExtensionHandler GetExtension(string name)
       	{
  	        Type extensionType = TargetExtensionFactory.GetExtensionType(name);
			if(extensionType==null)
				throw new InvalidOperationException("Can't get the type for name");
  	        return (TargetExtensionHandler)Activator.CreateInstance(extensionType);
       	}
       	
       	/// <summary>
       	/// Returns an instance from the type.
       	/// </summary>
       	public static TargetExtensionHandler GetExtension(Type extensionType)
       	{
       	    return (TargetExtensionHandler)Activator.CreateInstance(extensionType);    
       	}
	}
}