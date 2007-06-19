
using System;

using Developer.Common.Types;

namespace IptablesNet.Core
{
	
	/// <summary>
	/// Models a target for rules or parameters
	/// </summary>
	/// <remarks>
	/// The kind of things that can be allowed in a target depends in the
	/// place where the target is used.<br/>
	/// - Built in chain.<br/>
	/// - User defined chain.<br/>
	/// - Target extension <br/>
	/// </remarks>
	public class NetfilterTarget
	{
	    private string targetName;
	    
	    public string TargetName
	    {
	        get { return this.targetName;}
	        set { this.targetName = value;}
	    }
	    
		public NetfilterTarget(string targetName)
		{
		}
		
		public static bool IsBuiltInTarget(string name)
		{
		    object obj;
		    
		    if(AliasUtil.IsAliasName(typeof(RuleTargets), name, out obj))
		        return true;
		    
		    return false;
		}
		
		public static bool IsBuiltInChain(string name)
		{
		    object obj;
		    
		    if(AliasUtil.IsAliasName(typeof(BuiltInChains), name, out obj))
		        return true;
		    
		    return false;
		}
	}
}
