// SkSocketAddress.cs
//
//  Copyright (C)  2007 SharpKnocking project
//  Created by Miguel Angel Perez mangelp@gmail.com
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
using System.Net;
using System.Net.Sockets;

namespace Developer.Common
{
	/// <summary>
	/// Socket address for iSharpKnocking use
	/// </summary>
	/// <remarks>
	/// Experimental. Do not use.
	/// </remarks>
	public class SkSocketAddress: System.Net.SocketAddress
	{
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="family">
		/// A <see cref="AddressFamily"/>
		/// </param>
		public SkSocketAddress(AddressFamily family)
			:base(family)
		{
			
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="family">
		/// A <see cref="AddressFamily"/>
		/// </param>
		/// <param name="size">
		/// A <see cref="System.Int32"/>
		/// </param>
		public SkSocketAddress(AddressFamily family, int size)
			:base(family, size)
		{
			
		}
	}
}
