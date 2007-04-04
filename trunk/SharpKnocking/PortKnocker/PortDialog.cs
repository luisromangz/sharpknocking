
using System;
using Gtk;
using Glade;

namespace SharpKnocking.PortKnocker
{
   
	
	public class PortDialog
	{   
	    #region Widget Attributes
	
    	[WidgetAttribute]
    	Dialog portDialog;
    	
    	[WidgetAttribute]
    	SpinButton sbPortNumber;
    	
    	#endregion
		
		/// <summary>
		/// This constructor creates a window to edit a port, receiving a port 
		/// number.
		/// </summary>
		/// <param name="port">
		/// The port number to be edited.
		/// </param>
		public PortDialog(int port)
		{
		    Glade.XML gxml = new Glade.XML (null, "gui.glade", "portDialog", null);
			gxml.Autoconnect (this);
			
			portDialog.Icon=Gdk.Pixbuf.LoadFromResource("sharpknocking22.png");
			
			sbPortNumber.Value=port;
			
			// We force the spinbutton to grab focus, so the content
			// will be overwritten as the user types.
			sbPortNumber.GrabFocus();			
			
        }   
        
        /// <summary>
		/// This constructor creates a window to edit a port, initially set as 0.
		/// </summary>
		public PortDialog(): this(0)
        {
              
        }
        
        /// <summary>
        /// This property allows the value established for the port to be 
        /// retrieved.
        /// </summary>
        public int Value
        {
            get
            {
                return sbPortNumber.ValueAsInt;
            }
        }
        
        /// <summary>
        /// This method allows the caller to wait until a response has been 
        /// generated by the dialog.
        /// </summary>
        public ResponseType Run()
        {
            return (ResponseType)(portDialog.Run());
        
        }        
        
        private void OnBtnCancelClicked(object sender, EventArgs a)
        {
            portDialog.Hide();        
        }	
        
        private void OnBtnOkClicked(object sender, EventArgs a)
        {        
            OnPortAccepted();            
        }
        
        private void OnPortAccepted()
        {            
            sbPortNumber.IsFocus=false;
                    
            portDialog.Respond(ResponseType.Ok);
            portDialog.Hide();                    
        }
       
    }
}