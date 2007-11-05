// SetChainPolicyCommand.cs
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

using IptablesNet.Core;

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the set chain policy command.
	/// </summary>
	public class SetChainPolicyCommand: GenericCommand
	{
	    private string target;
	    
	    public string Target
	    {
	        get { return this.target;}
	        set { this.target = value;}
	    }

	    public override bool MustSpecifyRule {
	    	get { return false; }
	    }

	    public SetChainPolicyCommand()
	      :base(RuleCommands.SetChainPolicy)
		{
			throw new NotImplementedException ("This command is not implemented properly to be usable");
		}
		
		protected override string GetValuesAsString ()
		{
			return this.target;
		}

	}
}
