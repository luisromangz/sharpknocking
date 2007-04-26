
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.Common.Net;

using IptablesNet.Net;
using IptablesNet.Core.Extensions;
using IptablesNet.Core.Extensions.ExtendedMatch;

namespace IptablesNet.Extensions.Match
{
	
	public class UdpMatchExtension: MatchExtensionHandler
	{
		
		public UdpMatchExtension()
		  :base(typeof(UdpExtensionOptions), MatchExtensions.Udp)
		{
		}
		
		public override MatchExtensionParameter CreateParameter(string par)
		{
			object type;
			
			if(!TypeUtil.IsAliasName ( typeof(UdpExtensionOptions), par, out type))
				return null;
			
			UdpExtensionOptions option = (UdpExtensionOptions)type;
			
			return this.CreateParameter (option);
		}
			                       
        public UdpParam CreateParameter(UdpExtensionOptions option)
        {
			switch (option)
			{
				case UdpExtensionOptions.SourcePort:
					return new UdpSourceParam (this);
                case UdpExtensionOptions.DestinationPort:
                    return new UdpDestinationParam (this);
                default:
                    throw new ArgumentException ("Invalid option type: "+option);
			}
        }
        
        public UdpParam CreateParameter(UdpExtensionOptions option, string value)
        {
			UdpParam par = this.CreateParameter(option);
            if(par!=null)
                par.SetValues(value);
            return par;
        }        
        
        public abstract class UdpParam: MatchExtensionParameter
        {
            public new UdpMatchExtension Owner
            {
                get { return (UdpMatchExtension)base.Owner;}
            }
			
			public new UdpExtensionOptions Option
			{
				get { return (UdpExtensionOptions)base.Option;}
			}
            
            protected UdpParam(UdpMatchExtension owner, UdpExtensionOptions option)
                :base((MatchExtensionHandler)owner, option)
            {}
        }
		
		public class UdpSourceParam: UdpParam
		{

            private PortRange range;
            
            public PortRange Range
            {
                get { return  this.range;}
                set
                {
                    this.range = value;
                }
            }
			
			public UdpSourceParam(UdpMatchExtension handler)
				:base(handler, UdpExtensionOptions.SourcePort)
			{
				
			}
			
			protected UdpSourceParam(UdpMatchExtension handler, UdpExtensionOptions paramType)
				:base(handler, paramType)
			{
				
			}
            
            protected override string GetValuesAsString ()
            {
                if(this.range.IsValid)
                    return this.range.ToString();
                else
                    return String.Empty;
            }
            
            public override void SetValues (string value)
            {
                this.range = PortRange.Parse (value);
            }

		}
		
		public class UdpDestinationParam: UdpSourceParam
		{
			public UdpDestinationParam(UdpMatchExtension owner)
				:base(owner, UdpExtensionOptions.DestinationPort)
			{
				
			}
		}

	}
}
