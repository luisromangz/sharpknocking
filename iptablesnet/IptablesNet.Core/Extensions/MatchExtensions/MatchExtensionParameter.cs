
using System;
using System.Collections;

using SharpKnocking.Common;
using IptablesNet.Core;

namespace IptablesNet.Core.Extensions.ExtendedMatch
{
	
	public abstract class MatchExtensionParameter: ExtensionParameter<MatchExtensionHandler>
	{   
		public MatchExtensionParameter(MatchExtensionHandler owner, object enumValue)
		  :base(owner, enumValue)
		{

		}
	}
}
