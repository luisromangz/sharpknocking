// Log.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia, mangelp@gmail.com
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

using System;

namespace Developer.Common.Debug
{	
	public static class Log
	{   
	    public static LogLevel VerbLevel = LogLevel.Insane;
	    
	    //Disabled by default.
	    public static bool Enabled = true;
		public static bool LogDate = true;
	    
	    public static void Write(LogCategory cat, OutputTarget outT, string line, LogLevel level)
	    {
	        if(!Enabled || ((int)level)>((int)VerbLevel))
	            return;
	        
			if(outT == OutputTarget.Console)
			{
				if(LogDate)
					Console.Out.WriteLine(cat.ToString().ToUpper()+"["+DateTime.Now+"]: "+line);
				else
					Console.Out.WriteLine(cat.ToString().ToUpper()+": "+line);
			}
			else
				throw new NotImplementedException("This feature was not implemented.");
	    }
		
	    public static void Write(string header, params object[] parts)
	    {
	        if(!Enabled)
	            return;
	        
	        if(String.IsNullOrEmpty(header))
	        {
	            Console.Out.WriteLine("Array["+parts.Length+"]");
	        }
	        else
	        {
	            Console.Out.WriteLine(header);    
	        }
	        
	        for(int i=0;i<parts.Length;i++)
	        {
	            Console.Out.WriteLine("["+i+"]: "+parts[i]);
	        }
	    }
		
		public static void Write(string line)
		{
			Write(LogCategory.Information, OutputTarget.Console, line, LogLevel.Normal);
		}
		
		public static void Debug(string line)
		{
			Write(LogCategory.Debug, OutputTarget.Console, line, LogLevel.Normal);
		}
		
		public static void Debug(string line, LogLevel lv)
		{
			Write(LogCategory.Debug, OutputTarget.Console, line, lv);
		}
		
		public static void Error(string line)
		{
			Write(LogCategory.Error, OutputTarget.Console, line, LogLevel.Normal);
		}
		
		public static void Error(string line, LogLevel lv)
		{
			Write(LogCategory.Error, OutputTarget.Console, line, lv);
		}
		
		public static void Warning(string line)
		{
			Write(LogCategory.Warning, OutputTarget.Console, line, LogLevel.Normal);
		}
		
		public static void Warning(string line, LogLevel lv)
		{
			Write(LogCategory.Warning, OutputTarget.Console, line, lv);
		}
	}
}