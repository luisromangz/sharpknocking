// IKnocking.cs
//
//  Copyright (C) 2008 iSharpKnocking project
//  Created by Diego Campoy Collado manrash[at)gmail(doot]com
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

namespace PcapTools.Capturer
{	
	public interface IKnocking
	{	
		// Eval the knocking and return true filter let pass the knocking, 
		// otherwise return false.
		// Actually, the rule eval the knocking and not counterwise thus
		// in almost knocking implementation the knocking's eval will be
		// something like: rule.eval(this)
		// This method exist by the mental way of "A knocking check a rule".
		// TODO: Think about convert this interface IKnocking to a abstract class
		// with this method implemented.
		bool check(IRule rule);
	}
}
