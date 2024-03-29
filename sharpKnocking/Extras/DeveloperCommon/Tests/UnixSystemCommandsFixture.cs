// SystemCommandsFixture.cs
//
//  Copyright (C) 2008 iSharpKnocking project
//  Created by Miguel Angel Perez <mangelp>at<gmail>dot<com>
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

using NUnit.Core;
using NUnit.Framework;

using Developer.Common.SystemCommands;

using Developer.Common.Unix.SystemCommands;

namespace Developer.Common.Tests
{
	
	[TestFixture]
	public class UnixSystemCommandsFixture
	{
		public UnixSystemCommandsFixture()
		{
		}
		
		[TestFixtureSetUp]
		public void SetupFixture()
		{}
		
		[Test]
		public void ExecWhich()
		{
			Console.WriteLine("Executing command 'which mono'");
			UnixTextOutputSysCmd cmd = new UnixTextOutputSysCmd("which");
			cmd.OutputRead += new EventHandler<OutputReadEventArgs>(this.TextRead);
			cmd.SyncReadMode = ReadMode.All;
			
			cmd.Args = "mono";
			Console.WriteLine("Executing command "+cmd.CmdName);
			cmd.Exec();
			Console.WriteLine("Direct read "+cmd.Read());
			cmd.Args = "monodevelop";
			Console.WriteLine("Executing command "+cmd.CmdName);
			cmd.Exec();
			Console.WriteLine("Direct read "+cmd.Read());
			cmd.SyncReadMode = ReadMode.Line;
			cmd.Args = "mono";
			Console.WriteLine("Executing command "+cmd.CmdName);
			cmd.Exec();
			Console.WriteLine("Direct read "+cmd.Read());
			cmd.Args = "monodevelop";
			Console.WriteLine("Executing command "+cmd.CmdName);
			cmd.Exec();
			Console.WriteLine("Direct read "+cmd.Read());
		}
		
		private void TextRead(object sender, OutputReadEventArgs args)
		{
			Console.WriteLine("Result: "+args.Data);
		}
		
		private void ErrorRead(object sender, OutputReadEventArgs args)
		{
			Console.WriteLine("Error: "+args.Data);
		}
		
		[Test]
		public void ReadIptablesConfig()
		{
			UnixTextOutputSysCmd toc = new UnixTextOutputSysCmd("/sbin/iptables-save", true);
			toc.OutputRead += new EventHandler<OutputReadEventArgs>(this.TextRead);
			toc.ErrorRead += new EventHandler<OutputReadEventArgs>(this.ErrorRead);
			toc.Exec();
			//This doesn't work with gksu as it outputs the command stdout to stderr
//			Console.WriteLine("Iptables-save output:\n"+toc.Read());
		}
		
		[Test]
		public void ReadWithTimeout()
		{
			Console.WriteLine("Reading from a command that doesn't ends");
			UnixTextOutputSysCmd cmd = new UnixTextOutputSysCmd("tail", false);
			cmd.Args = "-f /home/mangelp/.bash_history";
			Console.WriteLine("Starting read, waiting for kill");
			cmd.Read();
		}
	}
}
