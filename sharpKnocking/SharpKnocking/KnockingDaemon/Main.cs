// project created on 11/01/2007 at 17:42
using System;
using System.IO;
using System.Threading;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.KnockingDaemon.PacketFilter;
using SharpKnocking.KnockingDaemon.SequenceDetection;
using SharpKnocking.KnockingDaemon.FirewallAccessor;

namespace SharpKnocking.KnockingDaemon
{
   
    /// <summary>
    /// Main class
    /// </summary>
    /// <remarks>
    /// Exit codes:
    /// 0- Normal exit.
    /// 1- Another instance running.
    /// 2- Can't create lock file.
    /// 3- Unmanaged exception thrown.
    /// 4- Tried to run without root permissions.
    /// 5- WTF
	/// 6 - Invalid argument number or arguments not valid
    /// </remarks>
	class MainClass
	{
        [MTAThread()]
		public static int Main(string[] args)
		{
		    Thread.CurrentThread.Name = "MainDaemonThread";
            //This is the daemon in charge of inter process comunication. It
            //behaves as a remote control for all the functionality.
            KnockingDaemonProcess daemon = new KnockingDaemonProcess();
		    
		    // Here we do parameter processing
		    for(int i=0; i<args.Length; i++)
		    {
		        Debug.VerboseWrite ("Parameters[ "+i+" ] = "+args[i]+"");
		        
		        if(args[i]=="--cfg")
		        {
		            daemon.Accessor.Parameters.Add("cfg",args[++i]);
		        }
		        else if(args[i]=="--dbg")
		        {
		            // Enable debugging
		            Debug.DebugEnabled = true;
		        }
		        else if(args[i]=="-v")
		        {
		            // Print more messages
		            Debug.MoreVerbose = true;
                    Debug.VerbLevel = VerbosityLevels.Normal;
		        }
                else if(args[i]=="--nofwmodify")
                {
                    daemon.Accessor.DryRun =true;
                }
                else if(args[i]=="--nocapture")
                {
                    daemon.DoCapture = false;
                }
                else if(args[i]=="-vv")
                {
                    Debug.MoreVerbose = true;
                    Debug.VerbLevel = VerbosityLevels.High;
                }
                else if(args[i]=="-vvv")
                {
                    Debug.MoreVerbose = true;
                    Debug.VerbLevel = VerbosityLevels.Insane;
                }
                else if(args[i]=="--ldcurrent")
                {
                    daemon.Accessor.Parameters.Add("ldcurrent",null);
                }
                else if(args[i]=="-h" || args[i]=="--help")
                {
                    PrintHelpMessage();
                    return 0;
                }
				else
				{
					Console.Out.WriteLine ("Invalid argument or argument number. Use -h option for help");
					return 6;
				}
                
		    }
		    
		    if(!UnixNative.ExecUserIsRoot())
		    {
		        //If we aren't rut exit mercyfully
		        Console.Out.WriteLine("You need root permissions to run this daemon");
		        return 4;
		    }
		    else if(UnixNative.ExistsLockFile())
		    {
		        //The file already exists. Daemon created.
		        Console.Out.WriteLine("Another instance of the daemon is running. If not "+
		                      "remove the lock file: "+UnixNative.LockFile);
		        return 1;
		    }
            else if(!UnixNative.CreateLockFile() || !File.Exists(UnixNative.LockFile))
            {
                Console.Out.WriteLine("Can't create lock file. Check the permissions to write into"+
                                "the file: "+UnixNative.LockFile );
                return 2;
            }
            else
            {
                Debug.Write("Lock file created!");
            }

            int rCode = 0;
              
            try
            {
            	
                //Run the communication daemon
                Debug.Write("Instantiating communication daemon");
                KnockingDaemonProcess.Run(daemon);
            }
            catch(Exception ex)
            {
                Debug.VerboseWrite("Unexpected fail: "+ex.Message, VerbosityLevels.Insane );
                Debug.Write("Details: \n"+ex);
                rCode = 3;
            }
            
            Debug.Write("Finishing daemon. Removing lock file...");
            UnixNative.RemoveLockFile();
		    
		    return rCode;
		}
        
        private static void PrintHelpMessage()
        {
            Console.Out.WriteLine ("KnockingDaemon service for the SharpKnocking suite.");
            Console.Out.WriteLine ("Released under LGPL terms.");
            Console.Out.WriteLine ("(c)2007 Luís Roman Gutierrez y Miguel Ángel Pérez Valencia"); 
			Console.Out.WriteLine ("Url: http://code.google.com/p/SharpKnocking");
            Console.Out.WriteLine ("Commands: --nofwmodify, --nocapture, --dbg, -v, -vv, -vvv, -h, --help, --cfg, --dry");
            Console.Out.WriteLine ("     --dbg, -v, -vv, -vvv: The first activates debugging and the rest the level of");
            Console.Out.WriteLine ("       detail (more 'v' means more verbose) and is optional.");
            Console.Out.WriteLine ("     --nocapture: Don't start the capture process. This makes daemon unusable");
            Console.Out.WriteLine ("     --nofwmodify: Don't modify current rule set. This makes daemon unusable");
            Console.Out.WriteLine ("     --cfg: The next argument must be a valid iptables configuration file that");
            Console.Out.WriteLine ("       will be loaded (used for debugging purposses).");
        }
	}
}